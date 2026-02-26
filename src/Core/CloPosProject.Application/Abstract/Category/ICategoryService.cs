using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Category
{
    public interface ICategoryService
    {
        Task<SimpleResponse<Guid>> CreateCategoryAsync(string name, string description, int displayOrder);
        Task<SimpleResponse<string>> UpdateCategoryAsync(Guid id, string name, string description, int displayOrder);
        Task<SimpleResponse<string>> DeleteCategoryAsync(Guid id);
        Task<SimpleResponse<string>> ActivateCategoryAsync(Guid id);
        Task<SimpleResponse<string>> DeactivateCategoryAsync(Guid id);
        Task<SimpleResponse<CategoryResponse>> GetByIdAsync(Guid id);
        Task<SimpleResponse<List<CategoryResponse>>> GetAllAsync(bool? isActive = null);
        Task<SimpleResponse<List<CategoryResponse>>> GetActiveCategoriesAsync();
    }

}
