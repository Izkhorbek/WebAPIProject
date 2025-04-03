using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;

namespace ECommerceApp.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO createDTO);

        public Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id);
        public Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto);
        public Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(int id);
        public Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
    }
}
