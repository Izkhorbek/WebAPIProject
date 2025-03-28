using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public CustomersController(ECommerceDbContext context)
        {
            _context = context;
        }


        // Register a new customer
        // Demonstrates [FromForm]
        // Endpoint: POST: /api/customers/register
        [HttpPost("register")]
        public async Task<ActionResult<Customer>> RegisterCustomer([FromForm] CustomerRegistrationDTO registrationDTO)
        {
            //check if email already exists
            if(await _context.Customers.AnyAsync(c => c.Email == registrationDTO.Email))
            {
                return BadRequest("Email already exists");
            }

            // Create a new customer
            var customer = new Customer
            {
                Name = registrationDTO.Name,
                Email = registrationDTO.Email,
                Password = registrationDTO.Password
            };

            // Add the new customer to the database
            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Return the newly created customer with Http Create status code
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer([FromHeader(Name ="X-Client-ID")] string clientId, // Binding from header
            [FromBody] CustomerLoginDTO loginDTO) //Binding from body
        {
            // Check the custom header
            if(string.IsNullOrWhiteSpace(clientId))
            {
                return BadRequest("Missing X-Client-ID header");
            }

            // Check if the customer exists
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == loginDTO.Email && c.Password == loginDTO.Password);

            if (customer == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate a JWT or other token
            return Ok(new {Message = "Athentication successful." });
        }

        // Get customer details.
        // Demonstrates default binding (from route or query).
        // Endpoint: GET /api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
    }
}
