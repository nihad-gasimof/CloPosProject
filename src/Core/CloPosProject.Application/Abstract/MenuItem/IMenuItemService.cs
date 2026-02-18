using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.MenuItem
{
    public interface IMenuItemService
    {
        Task<SimpleResponse<Guid>> CreateMenuItemAsync(string name,
        string description,
        decimal price,
        int preparationTime,
        Guid categoryId,
        IFormFile imageFile,
        List<MenuItemIngredientRequest> ingredients);
        Task<SimpleResponse<string>> UpdateMenuItemAsync(Guid id,
        string name,
        string description,
        decimal price,
        int preparationTime,
        Guid categoryId,
        IFormFile imageFile);
        Task<SimpleResponse<string>> UpdateMenuItemIngredientsAsync(Guid menuItemId,
        List<MenuItemIngredientRequest> ingredients);
        Task<SimpleResponse<string>> SetAvailabilityAsync(Guid id, bool isAvailable);
        Task<SimpleResponse<string>> DeleteMenuItemAsync(Guid id);

        Task<SimpleResponse<MenuItemResponse>> GetByIdAsync(Guid id);

        Task<SimpleResponse<List<MenuItemSummaryResponse>>> GetAllAsync(bool? isAvailable = null, Guid? categoryId = null);

        Task<SimpleResponse<List<MenuItemSummaryResponse>>> GetAvailableMenuItemsAsync();

        Task<SimpleResponse<bool>> CheckIfCanBePreparedAsync(Guid menuItemId);

        Task<SimpleResponse<string>> DeductIngredientsForOrderAsync(Guid menuItemId, int quantity);
    }
}
