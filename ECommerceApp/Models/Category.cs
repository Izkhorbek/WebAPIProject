using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    [Index(nameof(Name), Name = "IX_Name", IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Category name must be less than 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Category description must be less than 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
