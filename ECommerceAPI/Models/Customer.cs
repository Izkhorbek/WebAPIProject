using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Please enter password length in range (6, 100)")]
        [Required(ErrorMessage = "Customer password is required")]
        public string Password { get; set; }

        [JsonIgnore]
        public List<Order> Orders{ get; set; }

    }
}
