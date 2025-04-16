using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.PaymentDTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class PaymentService
    {
        private readonly ECommerceAppDbContext _context;
        private readonly EmailService _emailService;
        
        public PaymentService(ECommerceAppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ApiResponse<PaymentResponseDTO>> ProcessPaymentAsync(PaymentRequestDTO paymentRequest)
        {
            //Use a transaction to guarantee atomic operations on Order and Payment
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //Retrieve the order along with any payment record 
                    var order = await _context.Orders
                        .Include(o => o.Payment)
                        .FirstOrDefaultAsync(p => p.Id == paymentRequest.OrderId &&
                        p.CustomerId == paymentRequest.CustomerId);

                    if(order == null)
                    {
                        return new ApiResponse<PaymentResponseDTO>(404, "Order not found");
                    }

                    if(Math.Round(paymentRequest))

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}
