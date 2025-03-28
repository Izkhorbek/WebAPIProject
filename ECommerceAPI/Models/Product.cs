using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public  string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        public string Category { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100000")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock can not be a Negative value")]
        public int Stock{ get; set; }
    }
}
