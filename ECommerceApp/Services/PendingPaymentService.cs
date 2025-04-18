
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECommerceApp.Services
{
    public class PendingPaymentService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public PendingPaymentService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredKeyedService<ECommerceAppDbContext>();

                        // Retrieve payments with status "Pending"
                        var pendingPayments = await context.Payments
                            .Include(p => p.Order)
                            .Where(p => p.Status == Models.PaymentStatus.Pending && !p.PaymentMethod.Equals("COD", StringComparison.CurrentCultureIgnoreCase))
                            .ToListAsync(stoppingToken);

                        //List to track orders for which we need to send confirmation
                        var ordersToEmail = new List<int>();

                        foreach (var payment in pendingPayments)
                        {
                            //Simulate checking payment status
                            string updatedStatus = SimulatePaymentGatewayResponse();

                            if (updatedStatus == "Completed")
                            {
                                payment.Status = Models.PaymentStatus.Completed;
                                payment.Order.OrderStatus = Models.OrderStatus.Processing;
                                ordersToEmail.Add(payment.Order.Id);

                            }
                            else if (updatedStatus == "Failed")
                            {
                                payment.Status = PaymentStatus.Failed;
                            }

                            // If "Pending", no change

                            context.Payments.Update(payment);
                            context.Orders.Update(payment.Order);
                        }

                        //Save all status updates

                        await context.SaveChangesAsync(stoppingToken);

                        // If there are any orders that have been updated to Processing, send order confirmation emails.
                        if (ordersToEmail.Any())
                        {
                            //Retrieve the paymentService which has our email sending method
                            var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();

                            foreach (var orderId in ordersToEmail)
                            {
                                //Send the order confirmation email
                                await paymentService.SendOrderConfirmationEmailAsync(orderId);
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                //Wait for the next interval 
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        // Simulate a response from the payment gateway for pending payments
        // Returns updated payment status:  "Compelted" "Failed". or "Pending".
        private string SimulatePaymentGatewayResponse()
        {
            // Simulate payment gateway response:
            // 50% chance of "Comleted". 30% chance of "failed". 20% chance of remains Pending

            Random rnd = new Random();
            int chance = rnd.Next(1, 10);

            if (chance <= 50)
                return "Completed";
            else if (chance <= 80)
                return "Failed";
            else
                return "Pending";
        }
    }
}
