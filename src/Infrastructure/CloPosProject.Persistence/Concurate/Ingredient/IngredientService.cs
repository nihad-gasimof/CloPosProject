using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using CloPosProject.Application.Exceptions.Common;
using CloPosProject.Domain.Entities;
using CloPosProject.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Ingredient
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;
        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SimpleResponse<string>> AddStockAsync(Guid ingredientId, decimal quantity, decimal unitprice)
        {
            var existIngredient =await _context.Ingredients.
                Include(x=>x.Inventory).
                FirstOrDefaultAsync(x => x.Id == ingredientId);
            if (existIngredient is null)
            {
                return new SimpleResponse<string>("Məhsul tapılmadı");
            }
            if (existIngredient is not null)
            {
                existIngredient.Inventory.AddStock(quantity,unitprice);
            }
            else
            {
                var inventory = new Inventory(ingredientId, quantity, unitprice);
                await _context.Inventories.AddAsync(inventory);
            }
            await _context.SaveChangesAsync();
            return new SimpleResponse<string>("Stok uğurla əlavə edildi");

        }

        public async Task<SimpleResponse<Guid>> CreateIngredietAsync(CreateIngredientDto dto)
        {
            var existingIngredient = await _context.Ingredients
               .Include(i => i.Inventory)
               .FirstOrDefaultAsync(i => i.Name.ToLower() == dto.Name.ToLower());

            if (existingIngredient !=null)
            {
                if (dto.InitialQuantity>0)
                {
                    if (existingIngredient.Inventory!=null)
                    {
                        existingIngredient.Inventory.AddStock(dto.InitialQuantity, dto.UnitPrice);

                    }
                    else
                    {
                        var newInventory = new Inventory(existingIngredient.Id,
                            dto.InitialQuantity,
                            dto.UnitPrice);
                        await _context.Inventories.AddAsync(newInventory);
                    }
                    await _context.SaveChangesAsync();
                    return new SimpleResponse<Guid>("Məhsul mövcud idi, stok əlavə edildi",existingIngredient.Id);

                }
                return new SimpleResponse<Guid>("Bu adda məhsul artıq mövcuddur",existingIngredient.Id);
            }
            var ingredient =new  CloPosProject.Domain.Entities.Ingredient(dto.Name,dto.Unit,dto.MinimumStock,dto.Category);
            await _context.Ingredients.AddAsync(ingredient);
            if (dto.InitialQuantity>0)
            {
                var inventory = new Inventory(ingredient.Id, dto.InitialQuantity, dto.UnitPrice);
                await _context.Inventories.AddAsync(inventory);
            }
            await _context.SaveChangesAsync();
            return new SimpleResponse<Guid>($"Yeni məhsul yaradıldı: {ingredient.Name} (ID: {ingredient.Id}), İlkin stok: {dto.InitialQuantity}", ingredient.Id);
        }

        public async  Task<SimpleResponse<IngredientResponseDto>> GetByIdAsync(Guid id)
        {
            var ingredient=await _context.Ingredients.Include(x=>x.Inventory).FirstOrDefaultAsync(x=>x.Id==id);
            if (ingredient is null)
            {
                throw new NotFoundException("Məhsul tapılmadı");
            }
            var response = MapToResponse(ingredient);
            return new SimpleResponse<IngredientResponseDto>(response);
        }
        private IngredientResponseDto MapToResponse(CloPosProject.Domain.Entities.Ingredient ingredient)
        {
            return new IngredientResponseDto(
                ingredient.Id,
                ingredient.Name,
                ingredient.Unit,
                ingredient.Category,
                ingredient.MinimumStock,
                ingredient.CurrentStock,
                ingredient.CurrentPrice,
                ingredient.IsLowStock,
                ingredient.IsActive,
                ingredient.CreatedAt
            );
        }
        public async Task<SimpleResponse<string>> UseStockAsync(Guid ingredientId, decimal quantity)
        {
            var ingredient = await _context.Ingredients.Include(x => x.Inventory).FirstOrDefaultAsync(x => x.Id == ingredientId);
            if (ingredient is null)
            {
                return new SimpleResponse<string>("Məhsul tapılmadı");
            }
            if (ingredient.Inventory is null)
            {
                return new SimpleResponse<string>("Məhsulun stoku yoxdur");

            }
                ingredient.Inventory.RemoveStock(quantity);
                await _context.SaveChangesAsync();
            return new SimpleResponse<string>("Stok uğurla istifadə edildi");
        }

        public async Task<SimpleResponse<List<IngredientResponseDto>>> GetAllAsync(bool? isActive = null)
        {
            var query = _context.Ingredients.Include(x => x.Inventory).AsQueryable();
            if (isActive.HasValue)
            {
              query=  query.Where(x => x.IsActive == isActive.Value);
            }
            var ingredients = await query.ToListAsync();
            var responses = ingredients.Select(MapToResponse).ToList();
            if (responses is null)
            {
                throw new NotFoundException("Məhsullar siyahısı alınarkən xəta");
            }
            return new SimpleResponse<List<IngredientResponseDto>>(responses);
        }

        public async Task<SimpleResponse<List<LowStockResponseDto>>> GetLowStockIngredientsAsync()
        {
            var ingredients = await _context.Ingredients.Include(x => x.Inventory).Where(i => i.IsActive).ToListAsync();
            var lowstockitems = ingredients.Where(x => x.IsLowStock).Select(x => new LowStockResponseDto(
                x.Id,
                x.Name,
                x.CurrentStock,
                x.MinimumStock,
                x.Unit.ToString(),
                x.MinimumStock - x.CurrentStock


                )).ToList();
            if (lowstockitems is null)
            {
                throw new NotFoundException("Məhsullar siyahısı alınarkən xəta");

            }
            return new SimpleResponse<List<LowStockResponseDto>>(lowstockitems);
        }

        public async Task<SimpleResponse<string>> UpdateIngredientAsync(Guid id, UpdateIngredientDto dto)
        {
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ingredient is null)
                throw new NotFoundException("Məhsul tapılmadı");

            var nameExists = await _context.Ingredients
                .AnyAsync(x => x.Name.ToLower() == dto.Name.ToLower() && x.Id != id);

            if (nameExists)
                return new SimpleResponse<string>("Bu adda başqa məhsul artıq mövcuddur");

            ingredient.Update(
                dto.Name,
                dto.Unit,
                dto.MinimumStock,
                dto.Category
            );

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Məhsul uğurla yeniləndi");
        }

        public async Task<SimpleResponse<string>> DeleteIngredientAsync(Guid id)
        {
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ingredient is null)
                throw new NotFoundException("Məhsul tapılmadı");

            ingredient.Deactivate();

            await _context.SaveChangesAsync();

            return new SimpleResponse<string>("Məhsul silindi");
        }
    }
}
