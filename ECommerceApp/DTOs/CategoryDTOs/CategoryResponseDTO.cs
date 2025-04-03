namespace ECommerceApp.DTOs.CategoryDTOs
{
    public class CategoryResponseDTO
    {
        // DTO for returning category details.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
