using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class OrderDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<OrderItemDTO> Items { get; set; }
    }
}
