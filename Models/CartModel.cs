public class CartModel
{
    public int ID { get; set; }
    public int UserID { get; set; }
    public List<CartItemModel> Items { get; set; }
}

public class CartItemModel
{
    public int CartID { get; set; }
    public int ProductID { get; set; }
    public int Amount { get; set; }
} 