using CloPosProject.Application.Abstract.ICloudinary;
using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using CloPosProject.Application.Exceptions.Common;
using CloPosProject.Domain.Entities;
using CloPosProject.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.MenuItem
{
    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _imageservice;

        public MenuItemService(ApplicationDbContext context, ICloudinaryService service)
        {
            _context = context;
            _imageservice = service;
        }

        public async Task<SimpleResponse<bool>> CheckIfCanBePreparedAsync(Guid menuItemId)
        {
                var menuItem = await _context.MenuItems
                    .Include(m => m.MenuItemIngredients)
                        .ThenInclude(mi => mi.Ingredient)
                        .ThenInclude(i => i.Inventory)
                    .FirstOrDefaultAsync(m => m.Id == menuItemId);

                if (menuItem == null)
                    return new SimpleResponse<bool>("Menyu tapılmadı");

                var canBePrepared = await CheckIfCanBePreparedInternal(menuItem);
            return new SimpleResponse<bool>($" {(canBePrepared ? "Olar" : "Olmaz")} ", canBePrepared);
            }


        public async Task<SimpleResponse<Guid>> CreateMenuItemAsync(string name, string description, decimal price, int preparationTime, Guid categoryId, IFormFile imageFile,
            List<MenuItemIngredientRequest> ingredients)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new SimpleResponse<Guid>("Menyu adı boş ola bilməz");

            if (price <= 0)
                return new SimpleResponse<Guid>("Qiymət müsbət olmalıdır");

            if (preparationTime <= 0)
                return new SimpleResponse<Guid>("Hazırlanma müddəti müsbət olmalıdır");

            var exists = await _context.MenuItems
                .AnyAsync(m => m.Name.ToLower() == name.ToLower());

            if (exists)
                return new SimpleResponse<Guid>("Bu adda menyu artıq mövcuddur");


            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == categoryId && c.IsActive);

            if (!categoryExists)
                return new SimpleResponse<Guid>("Kateqoriya tapılmadı və ya aktiv deyil");

            if (ingredients != null && ingredients.Any())
            {
                var ingredientIds = ingredients.Select(x => x.IngredientId).ToList();
                var existingIngredients = await _context.Ingredients
                  .Where(i => ingredientIds.Contains(i.Id) && i.IsActive)
                  .Select(i => i.Id)
                  .ToListAsync();
                var missingIngredients = ingredientIds.Except(existingIngredients).ToList();
                if (missingIngredients.Any())
                    return new SimpleResponse<Guid>("Bəzi ingredient-lər tapılmadı və ya aktiv deyil");
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.Quantity <= 0)
                        return new SimpleResponse<Guid>("Ingredient miqdarı müsbət olmalıdır");
                }
            }
            string imageUrl = null;
            if (imageFile != null)
            {
                var uploadResult = await _imageservice.FileCreateAsync(imageFile);
                if (uploadResult is  null)
                {
                    return new SimpleResponse<Guid>("Yukleme ugursuz oldu");
                }

                imageUrl = uploadResult;
            }
            var MenuItem = new CloPosProject.Domain.Entities.MenuItem
                (name,
                description,
                price,
                preparationTime,
                categoryId,
                imageUrl);
            await _context.MenuItems.AddAsync(MenuItem);
            if (ingredients != null && ingredients.Any())
            {
                foreach (var ingredient in ingredients)
                {
                    var menuItemIngredient = new CloPosProject.Domain.Entities.MenuItemIngredient(MenuItem.Id,
                        ingredient.IngredientId,
                        ingredient.Quantity);
                    _context.Set<CloPosProject.Domain.Entities.MenuItemIngredient>().Add(menuItemIngredient);
                }
            }
            await _context.SaveChangesAsync();
            return new SimpleResponse<Guid>("Menyu uğurla yaradıldı", MenuItem.Id);
            throw new NotImplementedException();
        }

        public async Task<SimpleResponse<string>> DeductIngredientsForOrderAsync(Guid menuItemId, int quantity)
        {
            if (quantity <= 0)
                return new SimpleResponse<string>("Miqdar müsbət olmalıdır");

            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .FirstOrDefaultAsync(m => m.Id == menuItemId);

            if (menuItem == null)
                return new SimpleResponse<string>("Menyu tapılmadı");

            var canBePrepared = await CheckIfCanBePreparedInternal(menuItem, quantity);
            if (!canBePrepared)
                return new SimpleResponse<string>("Kifayət qədər ingredient yoxdur");

            foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
            {
                var requiredQuantity = menuItemIngredient.Quantity * quantity;

                if (menuItemIngredient.Ingredient.Inventory != null)
                {
                    menuItemIngredient.Ingredient.Inventory.RemoveStock(requiredQuantity);
                }
            }

            await _context.SaveChangesAsync();


            return new SimpleResponse<string>("Ingredient-lər uğurla çıxarıldı");
        }

        public async Task<SimpleResponse<string>> DeleteMenuItemAsync(Guid id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                return new SimpleResponse<string>("Menyu tapılmadı");

            menuItem.MakeUnavailable();
            await _context.SaveChangesAsync();


            return new SimpleResponse<string>("Menyu uğurla silindi");
        }

        public async  Task<SimpleResponse<List<MenuItemSummaryResponse>>> GetAllAsync(bool? isAvailable = null, Guid? categoryId = null)
        {
            var query = _context.MenuItems
           .Include(m => m.Category)
           .Include(m => m.MenuItemIngredients)
               .ThenInclude(mi => mi.Ingredient)
               .ThenInclude(i => i.Inventory)
           .AsQueryable();

            if (isAvailable.HasValue)
                query = query.Where(m => m.IsAvailable == isAvailable.Value);

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            var menuItems = await query.ToListAsync();
            var responses = new List<MenuItemSummaryResponse>();
            foreach (var menuItem in menuItems)
            {
                var canpreparedcheck =await CheckIfCanBePreparedInternal(menuItem);
                var response = new MenuItemSummaryResponse(
      menuItem.Id,
      menuItem.Name,
      menuItem.Description,
      menuItem.Price,
      menuItem.ImageUrl,
      menuItem.IsAvailable,
      menuItem.PreparationTime,
      menuItem.Category.Name,
      canpreparedcheck
  );
                responses.Add(response);
            }
            if (responses is null)
            {
                throw new NotFoundException();
            }
            return new SimpleResponse<List<MenuItemSummaryResponse>>(responses);
            
        }

        public async Task<SimpleResponse<List<MenuItemSummaryResponse>>> GetAvailableMenuItemsAsync()
        {
            return await GetAllAsync(isAvailable: true);
        }

        public async Task<SimpleResponse<MenuItemResponse>> GetByIdAsync(Guid id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                    .ThenInclude(i => i.Inventory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return new SimpleResponse<MenuItemResponse>("Menyu tapılmadı");
            }

            var menuitemingredientresponse = menuItem.MenuItemIngredients.Select(x=>new MenuItemIngredientResponse(x.IngredientId,x.Ingredient.Name,x.Quantity,x.Ingredient.CurrentStock,x.Ingredient.Unit.ToString(),x.Ingredient.CurrentStock>=x.Quantity)).ToList();
            bool checkk = await CheckIfCanBePreparedInternal(menuItem);
            var response = new MenuItemResponse(id,menuItem.Name,menuItem.Description,menuItem.Price,menuItem.ImageUrl,menuItem.IsAvailable,menuItem.PreparationTime,menuItem.CategoryId,menuItem.Category.Name, checkk, menuitemingredientresponse);
            if (response is null)
            {
                throw new NotFoundException();
            }
            return new SimpleResponse<MenuItemResponse>(response);
        }

        public async Task<SimpleResponse<string>> SetAvailabilityAsync(Guid id, bool isAvailable)
        {
                var menuItem = await _context.MenuItems
                    .FirstOrDefaultAsync(m => m.Id == id);


            if (menuItem == null)
                return new SimpleResponse<string>("Menyu tapılmadı");

            menuItem.SetAvailability(isAvailable);
                await _context.SaveChangesAsync();


                return new SimpleResponse<string>($"Menyu {(isAvailable ? "aktiv" : "deaktiv")} edildi");
            }
            
           
        
        public async Task<SimpleResponse<string>> UpdateMenuItemAsync(Guid id, string name, string description, decimal price, int preparationTime, Guid categoryId, IFormFile imageFile)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                return new SimpleResponse<string>("Menyu tapılmadı");

            if (string.IsNullOrWhiteSpace(name))
                return new SimpleResponse<string>("Menyu adı boş ola bilməz");

            if (price <= 0)
                return new SimpleResponse<string>("Qiymət müsbət olmalıdır");

            if (preparationTime <= 0)
                return new SimpleResponse<string>("Hazırlanma müddəti müsbət olmalıdır");


            var duplicateExists = await _context.MenuItems
                .AnyAsync(m => m.Id != id && m.Name.ToLower() == name.ToLower());

            if (duplicateExists)
                return new SimpleResponse<string>("Bu adda başqa menyu mövcuddur");

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == categoryId && c.IsActive);

            if (!categoryExists)
                return new SimpleResponse<string>("Kateqoriya tapılmadı və ya aktiv deyil");

            string newImageUrl = menuItem.ImageUrl;
            if (imageFile != null)
            {
                if (!string.IsNullOrWhiteSpace(menuItem.ImageUrl))
                {
                        await _imageservice.FileDeleteAsync(menuItem.ImageUrl);
                }

                var uploadResult = await _imageservice.FileCreateAsync(imageFile);
                if (uploadResult is null)
                {
                    return new SimpleResponse<string>("Yukleme ugursuz oldu");
                }
                newImageUrl = uploadResult;
            }
            menuItem.UpdateDetails(name, description, price, preparationTime, newImageUrl);
            menuItem.UpdateCategory(categoryId);

            await _context.SaveChangesAsync();


            return new SimpleResponse<string>("Menyu uğurla yeniləndi");
        }



        public async Task<SimpleResponse<string>> UpdateMenuItemIngredientsAsync(Guid menuItemId, List<MenuItemIngredientRequest> ingredients)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                .FirstOrDefaultAsync(m => m.Id == menuItemId);

            if (menuItem == null)
                return new SimpleResponse<string>("Menyu tapılmadı");

            if (ingredients == null || !ingredients.Any())
                return new SimpleResponse<string>("Ən azı bir ingredient əlavə edilməlidir");

            var ingredientIds = ingredients.Select(i => i.IngredientId).ToList();
            var existingIngredients = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.Id) && i.IsActive)
                .Select(i => i.Id)
                .ToListAsync();

            var missingIngredients = ingredientIds.Except(existingIngredients).ToList();
            if (missingIngredients.Any())
                return new SimpleResponse<string>("Bəzi ingredient-lər tapılmadı və ya aktiv deyil");

            foreach (var ingredient in ingredients)
            {
                if (ingredient.Quantity <= 0)
                    return new SimpleResponse<string>("Ingredient miqdarı müsbət olmalıdır");
            }

            _context.Set<MenuItemIngredient>().RemoveRange(menuItem.MenuItemIngredients);

            foreach (var ingredient in ingredients)
            {
                var menuItemIngredient = new MenuItemIngredient(
                    menuItemId,
                    ingredient.IngredientId,
                    ingredient.Quantity
                );
                _context.Set<MenuItemIngredient>().Add(menuItemIngredient);
            }

            await _context.SaveChangesAsync();


            return new SimpleResponse<string>("Ingredient-lər uğurla yeniləndi");
        }

        private async Task<bool> CheckIfCanBePreparedInternal(CloPosProject.Domain.Entities.MenuItem menuItem, int quantity = 1)
        {
            if (!menuItem.MenuItemIngredients.Any())
                return true;

            foreach (var menuItemIngredient in menuItem.MenuItemIngredients)
            {
                var requiredQuantity = menuItemIngredient.Quantity * quantity;
                var availableQuantity = menuItemIngredient.Ingredient.CurrentStock;

                if (availableQuantity < requiredQuantity)
                    return false;
            }

            return true;
        }
    }
}

