using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressesDTOs;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class AddressService : IAddressService
    {
        private readonly ECommerceAppDbContext _context;

        public AddressService(ECommerceAppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressRequestDTO)
        {
            try
            {
                //Check if customer exists
                var customer = await _context.Customers.FindAsync(addressRequestDTO.CustomerId);

                if (customer == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Customer not found");
                }

                //Manual mapping from DTO to Model
                var address = new Address
                {
                    CustomerId = addressRequestDTO.CustomerId,
                    AddressLine1 = addressRequestDTO.AddressLine1,
                    AddressLine2 = addressRequestDTO.AddressLine2,
                    City = addressRequestDTO.City,
                    State = addressRequestDTO.State,
                    PostalCode = addressRequestDTO.PostalCode,
                    Country = addressRequestDTO.Country
                };

                //Add address to the database
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                //Map to AddressResponseDTO
                var addressResponseDTO = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                };

                return new ApiResponse<AddressResponseDTO>(200, addressResponseDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDeleteDTO)
        {
            try
            {
                var address = await _context.Addresses
                    .FirstOrDefaultAsync(add => add.Id == addressDeleteDTO.AddressId && add.CustomerId == addressDeleteDTO.CustomerId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");
                }
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                // Prepare confirmation message
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDeleteDTO.AddressId} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, "An unexpected error occurred while processing your request.");
            }
        }

        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.Id == id);
                if (address == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Address not found");
                }

                //Map to AddressResponseDTO
                var addressResponseDTO = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                };

                return new ApiResponse<AddressResponseDTO>(200, addressResponseDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<AddressResponseDTO>>(404, "Customer not found.");
                }
                var addresses = customer.Addresses.Select(a => new AddressResponseDTO
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country
                }).ToList();

                return new ApiResponse<List<AddressResponseDTO>>(200, addresses);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<List<AddressResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            try
            {
                var address = await _context.Addresses.FindAsync(addressDto.AddressId);

                if(address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found");
                }

                //Update address properties
                address.AddressLine1 = addressDto.AddressLine1;
                address.AddressLine2 = addressDto.AddressLine2;
                address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
                address.Country = addressDto.Country;

                await _context.SaveChangesAsync();

                //Prepare confirmation response
                var confirmationResponse = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully",
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
