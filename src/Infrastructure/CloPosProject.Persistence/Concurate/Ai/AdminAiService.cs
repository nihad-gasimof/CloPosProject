using CloPosProject.Application.Abstract.Ai;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Domain.Entities;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace CloPosProject.Infrastructure.Concurate.Ai
{
    public class AdminAiService : IAdminAIService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AdminAiService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _apiKey = config["AiSettings:ApiKey"] ?? "";
            _httpClient = new HttpClient();
        }

        public async Task<SimpleResponse<string>> ProcessAdminRequestAsync(string adminQuery)
        {
            try
            {
                var allIngredients = await _context.Ingredients.Include(x=>x.Inventory).ToListAsync() ?? new List<Ingredient>(); ;

                // Sonra yaddaşda filtrləyirik
                var lowStockItems = allIngredients
                    .Where(i => i.CurrentStock < 10)
                    .Select(i => $"{i.Name}: {i.CurrentStock} qalıb")
                    .ToList();
                var today = DateTime.Today;

                // Satışlar
                var totalSales = await _context.Orders.Where(o => o.OrderDate >= today).SumAsync(o => o.TotalAmount);
                var orderCount = await _context.Orders.Where(o => o.OrderDate >= today).CountAsync();

                // Stok (Ingredientlər və Məhsullar)
                var ingredients = await _context.Ingredients.ToListAsync();
                var criticalStock = ingredients.Where(i => i.CurrentStock < 10)
                                               .Select(i => $"{i.Name} ({i.CurrentStock} {i.Unit})").ToList();

                // İşçi və İstifadəçi fəaliyyəti
                var activeUsers = await _context.Users.CountAsync();

                // Ən çox satılan 3 məhsul (Top Sellers)
                var topProducts = await _context.OrderItems
                    .GroupBy(oi => oi.MenuItemName)
                    .Select(g => new { Name = g.Key, Count = g.Sum(x => x.Quantity) })
                    .OrderByDescending(x => x.Count)
                    .Take(3).ToListAsync();

                // 2. AI-ya kontekst veririk (Sistem Təlimatı)
                var fullContext = $@"
    Sən CloPOS sisteminin MASTER ADMIN köməkçisisən. Sistemin hal-hazırkı vəziyyəti:
    
    [MALİYYƏ]: Bu gün {orderCount} sifarişdən cəmi {totalSales} AZN qazanc əldə edilib.
    [STOK]: Kritik vəziyyətdə olanlar: {(criticalStock.Any() ? string.Join(", ", criticalStock) : "Yoxdur")}.
    [MƏHSULDARLIQ]: Ən çox satılan məhsullar: {string.Join(", ", topProducts.Select(p => p.Name))}.
    [İSTİFADƏÇİ]: Sistemdə cəmi {activeUsers} qeydiyyatlı şəxs var.
    
    Sənə nə sual verilsə, bu məlumatlar işığında analiz et. Əgər sual gələcəklə bağlıdırsa (məsələn: 'nə qədər mal alım?'), stok və satış trendinə baxaraq tövsiyə ver.";
                var requestBody = new
                {
                    contents = new[] {
                        new { parts = new[] { new { text = $"{fullContext}\n\nAdminin sualı: {adminQuery}" } } }
                    }
                };
                var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_apiKey}";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);

                // Əgər Gemini-dən xəta gəlibsə və ya cavab yoxdursa yoxlayırıq
                if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                {
                    // Əgər API-dan xəta mesajı gəlibsə onu tuturuq
                    if (doc.RootElement.TryGetProperty("error", out var error))
                    {
                        return new SimpleResponse<string>($"API Xətası: {error.GetProperty("message").GetString()}");
                    }
                    return new SimpleResponse<string>("AI uyğun cavab tapa bilmədi (Naməlum xəta).");
                }

                // Əgər hər şey qaydasındadırsa, məlumatı çıxarırıq
                var aiResponse = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return new SimpleResponse<string>("Uğurlu", aiResponse);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<string>($"Xəta: {ex.Message}");
            }
        }
    }
}