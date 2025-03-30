using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    // Represents the address of a user
    public class Address
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        [StringLength(100, ErrorMessage = "Addressline 1 cannot be exceed 100 characters.")]
        public string AddressLine1 { get; set; }

        [StringLength(100, ErrorMessage = "Addressline 2 cannot be exceed 100 characters.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot be exceed 50 characters.")]
        public string  City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot be exceed 50 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot be exceed 50 characters.")]
        public string Country { get; set; }
    }

}
