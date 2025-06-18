namespace UserManagementAPI.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string? PaymentId { get; set; }
        public string? PayerId { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
} 