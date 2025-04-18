using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CancellationDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class CancellationService
    {
        private readonly ECommerceAppDbContext _context;
        private readonly EmailService _emailService;

        public CancellationService(ECommerceAppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        //Handles a cancellation request from a customer.
        public async Task<ApiResponse<CancellationResponseDTO>> RequestCancellationAsync(CancellationRequestDTOs cancellationRequest)
        {
            try
            {
                //Validate order existance with its items and product details(read-only)
                var order = await _context.Orders
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.Id == cancellationRequest.OrderId && o.CustomerId == cancellationRequest.CustomerId);

                if (order == null)
                {
                    return new ApiResponse<CancellationResponseDTO>(404, "Order not found");
                }

                //Check if order is eligible for cancellation (only processing)                                    
                if (order.OrderStatus != Models.OrderStatus.Processing)
                {
                    return new ApiResponse<CancellationResponseDTO>(400, "Order is not eligible for cancellation");
                }

                //Check if a cancellation request for the order already exists
                var existingCancellation = await _context.Cancellations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.OrderId == cancellationRequest.OrderId);

                if (existingCancellation != null)
                {
                    return new ApiResponse<CancellationResponseDTO>(400, 'A Cancallation request for this order already exists.);
                }

                //Create the new cancellation record
                var cancellation = new Cancellation
                {
                    OrderId = cancellationRequest.OrderId,
                    Reason = cancellationRequest.Reason,
                    Status = CancellationStatus.Pending,
                    RequestedAt = DateTime.UtcNow,
                    OrderAmount = order.TotalAmount,
                    CancellationCharges = 0.00m,   // default zero; admin may update later if needed.
                };

                _context.Cancellations.Add(cancellation);
                await _context.SaveChangesAsync();

                //Mapping from Cancellation to CancellatioResponseDTO
                var cancellationResponseDTO = new CancellationResponseDTO
                {
                    Id = cancellation.Id,
                    OrderId = cancellation.OrderId,
                    Reason = cancellation.Reason,
                    OrderAmount = cancellation.OrderAmount,
                    Status = cancellation.Status,
                    RequestedAt = cancellation.RequestedAt,
                    CancellationCharges = cancellation.CancellationCharges
                };

                return new ApiResponse<CancellationResponseDTO>(200, cancellationResponseDTO);
            }
            catch (Exception ex)
            {
                // Log exception as need 
                return new ApiResponse<CancellationResponseDTO>(500, $"An unexpected error occured: {ex
                    .Message}");
            }
        }

        //Retrieve a cancellation request by its Id
        public async Task<ApiResponse<CancellationResponseDTO>> GetCancellationByIdAsync(int id)
        {
            try
            {
                var cancellation = await _context.Cancellations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cancellation == null)
                {
                    return new ApiResponse<CancellationResponseDTO>(400, "Cancellation request not found.");
                }

                var cancellationResponse = new CancellationResponseDTO
                {
                    Id = cancellation.Id,
                    OrderId = cancellation.OrderId,
                    Reason = cancellation.Reason,
                    Status = cancellation.Status,
                    RequestedAt = cancellation.RequestedAt,
                    ProcessedAt = cancellation.ProcessedAt,
                    ProcessedBy = cancellation.ProcessedBy,
                    Remarks = cancellation.Remarks,
                    OrderAmount = cancellation.OrderAmount,
                    CancellationCharges = cancellation.CancellationCharges
                };

                return new ApiResponse<CancellationResponseDTO>(200, cancellationResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CancellationResponseDTO>(500, $"An unexpected error occured: {ex.Message}");
            }
        }

        //Update tha status of a cancellation request (approval/rejection) by an administrator
        //Also handles order status update and stock restoration if approved
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCancellationStatusAsync(CancellationStatusUpdateDTO statusUpdate)
        {
            // Begin a transaction to ensure atomic operations
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

            }
        }
    }
}
