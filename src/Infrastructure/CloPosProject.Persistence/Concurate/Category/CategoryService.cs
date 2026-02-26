using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Category
{
    public class CategoryService(ApplicationDbContext _context) : ICategoryService
    {
        public async Task<SimpleResponse<string>> ActivateCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return new SimpleResponse<string>("Kateqoriya tapılmadı");

            category.Activate();
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Kateqoriya aktivləşdirildi");
        }

        public async Task<SimpleResponse<Guid>> CreateCategoryAsync(string name, string description, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new SimpleResponse<Guid>("Kateqoriya adı boş ola bilməz");

            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());

            if (exists)
                return new SimpleResponse<Guid>("Bu adda kateqoriya artıq mövcuddur");

            var category = new CloPosProject.Domain.Entities.Category(
                name,
                description,
                displayOrder
            );

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new SimpleResponse<Guid>("Kateqoriya uğurla yaradıldı", category.Id);
        }

        public async Task<SimpleResponse<string>> DeactivateCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return new SimpleResponse<string>("Kateqoriya tapılmadı");

            category.Deactivate();
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Kateqoriya deaktivləşdirildi");
        }

        public async Task<SimpleResponse<string>> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories
                   .Include(c => c.MenuItems)
                   .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return new SimpleResponse<string>("Kateqoriya tapılmadı");

            if (category.MenuItems.Any())
                return new SimpleResponse<string>("Bu kateqoriyada məhsullar var, silinə bilməz. Deaktiv edin.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Kateqoriya uğurla silindi");
        }

        public async Task<SimpleResponse<List<CategoryResponse>>> GetActiveCategoriesAsync()
        {
            return await GetAllAsync(isActive: true);
        }

        public async Task<SimpleResponse<List<CategoryResponse>>> GetAllAsync(bool? isActive = null)
        {
            var query = _context.Categories
                              .Include(c => c.MenuItems)
                              .AsQueryable();

            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);

            var categories = await query
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();

            var responses = categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive,
                MenuItemsCount = c.MenuItems.Count
            }).ToList();

            return new SimpleResponse<List<CategoryResponse>>(responses);
        }

        public async  Task<SimpleResponse<CategoryResponse>> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories
                 .Include(c => c.MenuItems)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return new SimpleResponse<CategoryResponse>("Kateqoriya tapılmadı");

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder,
                IsActive = category.IsActive,
                MenuItemsCount = category.MenuItems.Count
            };

            return new SimpleResponse<CategoryResponse>(response);
        }

        public async Task<SimpleResponse<string>> UpdateCategoryAsync(Guid id, string name, string description, int displayOrder)
        {
            var category = await _context.Categories.FindAsync(id) ;

            if (category == null)
                return new SimpleResponse<string>("Kateqoriya tapılmadı");

            if (string.IsNullOrWhiteSpace(name))
                return new SimpleResponse<string>("Kateqoriya adı boş ola bilməz");

            var duplicateExists = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == name.ToLower());

            if (duplicateExists)
                return new SimpleResponse<string>("Bu adda başqa kateqoriya mövcuddur");

            category.UpdateDetails(name, description, displayOrder);
            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Kateqoriya uğurla yeniləndi");
        }
    }
}
