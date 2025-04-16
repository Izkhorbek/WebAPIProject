using ECommerceApp.DTOs.ProductDTOs;
using ECommerceApp.DTOs;

namespace ECommerceApp.Services
{
    public interface IProductService
    {
        public  Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productDto);

        public  Task<ApiResponse<ProductResponseDTO>> GetProductByIdAsync(int id);
        public  Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto);
        public  Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(int id);
        public Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync();
        public Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(int categoryId);
        public Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStatusAsync(ProductStatusUpdateDTO productStatusUpdateDTO);

    }
}
