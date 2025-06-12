namespace UserManagementAPI.DTOs
{
    public class ProductResponseDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public int Power { get; set; }
        public string? Material { get; set; }
        public string? Image { get; set; }
        public string Type { get; set; }
        public string? Scope { get; set; }
        public string? Capacity { get; set; }
        public string? ScreenSize { get; set; }
    }
} 