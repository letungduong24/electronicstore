public class OrderModel
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public int VoucherID { get; set; }
    public string Status { get; set; }
    public List<OrderItemModel> Items { get; set; }
}

public class OrderItemModel
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Amount { get; set; }
} 