using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;
using ECommerceApp.Services.Interfaces;

namespace ECommerceApp.Services
{
    public class AddressService : IAddressService
    {
        public Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            throw new NotImplementedException();
        }
    }
}
