using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;

namespace ECommerceApp.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressRequestDTO);
        public Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id);
        public Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto);
        public Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO);
        public Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(int customerId);
    }
}
