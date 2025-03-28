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
    public class ProductController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public ProductController(ECommerceDbContext context)
        {
            _context = context;
        }

        //Get all products with optional filtering by name, category and price range
        // Demonstrates [FromQuery] and default binding
        // Endpoint: GET: /api/products/GetProducts?name={name}&category={category}&minPrice={minPrice}&maxPrice={maxPrice}

        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? name, // Explicitly binding from quert string
            [FromQuery] string? category, // Explicitly binding from query string
            [FromQuery] decimal? minPrice, // Explicitly binding from query string
            decimal? maxPrice   // Default binding; since it's a simple type, binds from query string   
            )
        {
            var query = _context.Products.AsQueryable();
            
            if(!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if(!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if(minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            var products = await query.ToListAsync();

            return Ok(products);
        }


        // Get a specific product by ID
        // Demonstrates [FromRoute] and default binding
        // Endpoint: GET: /api/products/GetProductById/{id}
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Create a new product
        // Demonstrates [FromBody] binding
        // Endpoint: POST: /api/products/CreateProduct
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            // mapping from ProductCreateDTO to Product Entity

            var product = new Product
            {
                Name = productCreateDTO.Name,
                Description = productCreateDTO.Description,
                Category = productCreateDTO.Category,
                Price = productCreateDTO.Price,
                Stock = productCreateDTO.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Return 201 Created status with the location header
            return CreatedAtAction("GetProductById", new { id = product.Id }, product);
        }

        // Update an existing product's price
        // Demonstrates multiple binding attributes
        // Endpoint: PUT: /api/products/UpdateProductPrice/{id}?price={ newprice}
        [HttpPut("UpdateProductPrice/{id}")]
        public async Task<IActionResult> UpdateProductPrice(
            [FromRoute] int id,          // Binding from route
            [FromQuery] decimal price)   // Binding from query string
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            product.Price = price;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Demonstrates [FromQuery] for pagination
        // Endpoint: GET: /api/products/paged?pageNumber={pageNumber}&pageSize={pageSize}
        [HttpGet("paged")]
        public async Task<ActionResult<List<Product>>> GetProductsPaged(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize )
        {
            var products = await _context.Products
                .Skip((pageNumber -1 ) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(products);
        }

        // Upload product image
        // Demonstrates [FromForm] binding
        // Endpoint: POST: api/products/{id}/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UplaodProductImage(
            [FromRoute] int id,  // Binding from route
            IFormFile file )      // Binding from form data
        {
            if(file == null || file.Length == 0)
                return BadRequest("No file upload");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // For demonstration, we'll just read the file name
            // In a real application, you would save the file to storage and update the product's image URL

            var fileName = Path.GetFileName(file.FileName);
            // TODO: Save the file to storage and update the product.ImageUrl

            return Ok(new { Message = "Image uploaded successfully.", FileName = fileName });
        }
    }
}
