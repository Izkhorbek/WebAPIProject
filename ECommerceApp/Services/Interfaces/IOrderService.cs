
using ECommerceApp.Data;
using ECommerceApp.DTOs.OrderDTOs;
using ECommerceApp.DTOs;

namespace ECommerceApp.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<ApiResponse<OrderResponseDTO>> CreateOrderAsync(OrderCreateDTO orderDto);

        public Task<ApiResponse<OrderResponseDTO>> GetOrderByIdAsync(int orderId);

        public Task<ApiResponse<ConfirmationResponseDTO>> UpdateOrderStatusAsync(OrderStatusUpdateDTO statusDto);

        public Task<ApiResponse<List<OrderResponseDTO>>> GetAllOrdersAsync();

        public Task<ApiResponse<List<OrderResponseDTO>>> GetOrdersByCustomerAsync(int customerId);
    }
}
