using ECommerceApp.DTOs.ShoppingCartDTOs;
using ECommerceApp.DTOs;
using ECommerceApp.Models;

namespace ECommerceApp.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public  Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(int customerId);

        public  Task<ApiResponse<CartResponseDTO>> AddToCartAsync(AddToCartDTO addToCartDTO);

        public  Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO);

        public  Task<ApiResponse<CartResponseDTO>> RemoveCartItemAsync(RemoveCartItemDTO removeCartItemDTO);

        public  Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(int customerId);
    }
}
