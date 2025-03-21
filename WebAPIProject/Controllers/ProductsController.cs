using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // In-memory list to store products (for demonstration purposes)
        // In Real-time, we will use database
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000.00m, Category = "Electronics" },
            new Product { Id = 2, Name = "Desktop", Price = 2000.00m, Category = "Electronics" },
            new Product { Id = 3, Name = "Mobile", Price = 300.00m, Category = "Electronics" },
            // Additional products can be added here
        };


        //GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_products);
        }

        //GET: api/Products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new {Message = $"Product with ID {id} not found"});
            }

            return Ok(product);
        }

        //POST: api/Products
        [HttpPost]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new {Message = "Product data is missing"});
            }

            //Basic ID assignment logic (for demonstration purposes)
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);

            return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product updatedProduct)
        {
            if(id !=updatedProduct.Id)
            {
                return BadRequest(new { Message = "Product ID mismatch" });
            }

            var axistingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (axistingProduct == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found" });
            }

            // Update the existing product
            axistingProduct.Name = updatedProduct.Name;
            axistingProduct.Price = updatedProduct.Price;
            axistingProduct.Category = updatedProduct.Category;

            // In real-time, we will update the product in the database

            return NoContent();
        }

        // DELETE: api/Products/{id}
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }
            _products.Remove(product);
            // In a real application, remove the product from the database here
            return NoContent();
        }
    }
}
