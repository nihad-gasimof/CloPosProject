using CloPosProject.Application.Abstract.Payment;
using CloPosProject.Application.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CloPosProject.Persistence.Concurate.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string username;
        private readonly string password;
        public PaymentService(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            var username = configuration.GetSection("KapitalBankSettings").GetValue<string>("Username");
            var password = configuration.GetSection("KapitalBankSettings").GetValue<string>("Password");
        }
        
        // Interface method
        public async Task<PurchaseDto> CreatePaymentRequest(OrderCreateDto dto)
        {
            return await GeneratePaymentRequest(dto);
        }

        public async Task<PurchaseDto> GeneratePaymentRequest(OrderCreateDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            var payload = new
            {
                order = new
                {
                    typeRid = "Order_SMS",
                    amount = dto.Amount.ToString().Replace(",","."),
                    currency = dto.Currency ?? string.Empty,
                    language = "az",
                    description = dto.Description ?? string.Empty,
                    hppRedirectUrl = dto.RedirectUrl ?? string.Empty,
                    hppCofCapturePurposes = new[] { "Cit" }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // If Authorization header is required set it on the HttpClient prior to calling this method
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://txpgtst.kapitalbank.az/api/order/")
            {
                Content = content
            };

            // Ensure Authorization header is present. If HttpClient already has authorization, keep it.
            if (request.Headers.Authorization == null && _httpClient.DefaultRequestHeaders.Authorization == null)
            {
                // Basic auth credentials (TerminalSys/kapital:kapital123)
                var credentials = $"{username}:{password}";
                var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var contentt = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<PurchaseDto>(contentt);
            if (result is null)
                throw new InvalidOperationException("Failed to deserialize payment response.");

            return result;
        }

        public async Task<PaymentStatusDto> GetPaymentStatus(int purchaseId)
        {
            if (purchaseId <= 0)
                throw new ArgumentException("Invalid purchaseId", nameof(purchaseId));

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://txpgtst.kapitalbank.az/api/order/{purchaseId}"
            );

            // Authorization əlavə et (əgər HttpClient-də yoxdursa)
            if (request.Headers.Authorization == null &&
                _httpClient.DefaultRequestHeaders.Authorization == null)
            {
                var credentials = "TerminalSys/kapital:kapital123";
                var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
            }

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentStatusDto
                {
                    IsSuccess = false,
                };
            }

            var result = JsonConvert.DeserializeObject<PurchaseDto>(content);

            if (result?.Order == null)
            {
                return new PaymentStatusDto
                {
                    IsSuccess = false,
                };
            }

            var orderStatus = result.Order.Status?.ToLowerInvariant();

            var isSuccess = orderStatus == "approved" ||
                            orderStatus == "completed" ||
                            orderStatus == "fullypaid";

            return new PaymentStatusDto
            {
                IsSuccess = isSuccess,
                TransactionId = result.Order.Id.ToString(),
            };
        }
    }
}
