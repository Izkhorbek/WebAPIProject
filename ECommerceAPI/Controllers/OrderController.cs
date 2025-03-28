using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrderController(ECommerceDbContext context)
        {
            _context = context;
        }

        // Create a new order 
        // Demonstrates [FromBody] binding
        // Endpoint: POST: /api/order/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            // Validate the order
            var order = new Order
            {
                CustomerId = orderDTO.CustomerId,
                OrderStatus = "Processing",
                OrderAmount = 0, // Will calculate  based on OrderItems
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            // Iterate through order items and add to order 
            foreach (var item in orderDTO.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with ID{item.ProductId} does not exit");
                }

                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"Product {product.Name} is out of stock");
                }

                // Deduct stock
                product.Stock -= item.Quantity;

                // Calculate tolat amount
                totalAmount += product.Price * item.Quantity;

                // Create OrderItem
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
            }

            order.OrderAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // Get an order by ID
        // Demonstrates [FromRoute] binding
        // Endpoint: GET: /api/order/GetOrderById/{id}
        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<Order>> GetOrderById([FromRoute] int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
