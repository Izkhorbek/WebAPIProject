using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CustomerDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerceApp.Services
{
    public class CustomerService
    {
        private readonly ECommerceAppDbContext _context;

        public CustomerService(ECommerceAppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CustomerResponseDTO>> RegisterCustomerAsync(
            CustomerRegistrationDTO customerDTO)
        {
            try
            {
                //Check if email already exists
                if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == customerDTO.Email.ToLower()))
                {
                    return new ApiResponse<CustomerResponseDTO>(400, "Email already exists.");
                }

                // Manually mapping from DTO to Model
                var customer = new Customer
                {
                    FirstName = customerDTO.FirstName,
                    LastName = customerDTO.LastName,
                    Email = customerDTO.Email,
                    PhoneNumber = customerDTO.PhoneNumber,
                    DateOfBirth = customerDTO.DateOfBirth,
                    IsActive = true,

                    //Hash the password before storing it by using BCrypt
                    Password = BCrypt.Net.BCrypt.HashPassword(customerDTO.Password)
                };

                // Add the customer to the database
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                // Prepare response data
                var customerResponseDTO = new CustomerResponseDTO
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };

                return new ApiResponse<CustomerResponseDTO>(200, customerResponseDTO);
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                // Find customer by email with AsNoTracking for performance since we don't need to track changes
                var customer = await _context.Customers.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Email.ToLower() == loginDTO.Email.ToLower());

                // Check if customer exists
                if (customer == null)
                {
                    return new ApiResponse<LoginResponseDTO>(HttpStatusCode.BadRequest, "Invalid email or password.");
                }

                // Verify the password using BCrypt
                bool isPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, customer.Password);

                if (!isPassword)
                {
                    return new ApiResponse<LoginResponseDTO>(HttpStatusCode.BadRequest, "Invalid email or password.");
                }

                // Prepare response data
                var loginResponse = new LoginResponseDTO
                {
                    Message = "Login successful.",
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}"
                };

                return new ApiResponse<LoginResponseDTO>(HttpStatusCode.OK, loginResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);
                if (customer == null)
                {
                    return new ApiResponse<CustomerResponseDTO>(404, "Customer not found.");
                }
                // Map to CustomerResponseDTO
                var customerResponse = new CustomerResponseDTO
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth
                };
                return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(
            CustomerUpdateDTO customerDTO)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerDTO.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }

                //Check if email is being updated to an existing one
                if (customer.Email != customerDTO.Email && 
                    await _context.Customers.AnyAsync(c => c.Email ==  customerDTO.Email))
                {
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Email already in use.");
                }

                // Update customer properties manually
                customer.FirstName = customerDTO.FirstName;
                customer.LastName = customerDTO.LastName;
                customer.Email = customerDTO.Email;
                customer.PhoneNumber = customerDTO.PhoneNumber;
                customer.DateOfBirth = customerDTO.DateOfBirth;

                await _context.SaveChangesAsync();

                //Prepare response data
                var confirmationResponse = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {customerDTO.CustomerId} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");
                }
                //Soft Delete
                customer.IsActive = false;
                await _context.SaveChangesAsync();
                // Prepare confirmation message
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Customer with Id {id} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        // Changes the password for an existing customer.
        public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(changePasswordDto.CustomerId);
                if (customer == null || !customer.IsActive)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive.");
                }
                // Verify current password
                bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
                if (!isCurrentPasswordValid)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(401, "Current password is incorrect.");
                }
                // Hash the new password
                customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                await _context.SaveChangesAsync();
                // Prepare confirmation message
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = "Password changed successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
