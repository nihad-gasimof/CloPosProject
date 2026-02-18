using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Ingredient
{
    public interface IIngredientService
    {
        Task<SimpleResponse<Guid>> CreateIngredietAsync(CreateIngredientDto dto);
        Task<SimpleResponse<string>> AddStockAsync(Guid ingredientId, decimal quantity, decimal unitprice);
        Task<SimpleResponse<string>> UseStockAsync(Guid ingredientId, decimal quantity);
        Task<SimpleResponse<IngredientResponseDto>> GetByIdAsync(Guid id);
        Task<SimpleResponse<List<IngredientResponseDto>>> GetAllAsync(bool? isActive = null);
        Task<SimpleResponse<List<LowStockResponseDto>>> GetLowStockIngredientsAsync();
        Task<SimpleResponse<string>> UpdateIngredientAsync(Guid id, UpdateIngredientDto dto);
        Task<SimpleResponse<string>> DeleteIngredientAsync(Guid id);
    }
}
