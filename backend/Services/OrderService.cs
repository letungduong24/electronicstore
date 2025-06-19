using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;
using UserManagementAPI.Mapper;

namespace UserManagementAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly OrderMapper _orderMapper;

        public OrderService(IUnitOfWork unitOfWork, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _orderMapper = new OrderMapper();
        }

        public async Task<OrderDto> CreateOrderFromCartAsync(string userId, CreateOrderDto createOrderDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Get user's cart
                var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    throw new InvalidOperationException("Cart is empty or not found");
                }

                // Validate stock availability
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(cartItem.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product {cartItem.ProductId} not found");
                    }

                    if (product.Stock < cartItem.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                    }
                }

                // Calculate total amount
                decimal totalAmount = 0;
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(cartItem.ProductId);
                    totalAmount += product.Price * cartItem.Quantity;
                }

                // Check if user has sufficient balance
                var currentBalance = await _walletService.GetUserBalanceAsync(userId);
                Console.WriteLine($"Current balance: {currentBalance}, Total amount: {totalAmount}");
                
                if (!await _walletService.HasSufficientBalanceAsync(userId, totalAmount))
                {
                    throw new InvalidOperationException($"Insufficient balance in wallet. Current: {currentBalance}, Required: {totalAmount}");
                }

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    OrderNumber = await _unitOfWork.Orders.GenerateOrderNumberAsync(),
                    OrderDate = DateTime.UtcNow,
                    Status = "Paid", // Directly set as paid since payment is automatic
                    TotalAmount = totalAmount,
                    ShippingAddress = createOrderDto.ShippingAddress,
                    Notes = createOrderDto.Notes,
                    PaymentId = $"WALLET_PAY_{DateTime.UtcNow:yyyyMMddHHmmss}",
                    PayerId = userId,
                    OrderItems = new List<OrderItem>()
                };

                // Create order items and reduce stock
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(cartItem.ProductId);
                    
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductImageUrl = product.ImageUrl,
                        UnitPrice = product.Price,
                        Quantity = cartItem.Quantity,
                        TotalPrice = product.Price * cartItem.Quantity
                    };

                    order.OrderItems.Add(orderItem);

                    // Reduce stock
                    product.Stock -= cartItem.Quantity;
                    await _unitOfWork.Products.UpdateProductAsync(product);
                }

                // Save order
                await _unitOfWork.Orders.CreateOrderAsync(order);

                // Deduct amount from user's wallet
                var paymentSuccess = await _walletService.DeductFromBalanceAsync(userId, totalAmount);
                if (!paymentSuccess)
                {
                    throw new InvalidOperationException("Payment failed - insufficient balance");
                }

                // Clear cart
                await _unitOfWork.Carts.ClearCartAsync(cart.Id);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _orderMapper.ToOrderDto(order);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
            
            if (order == null || order.UserId != userId)
            {
                throw new InvalidOperationException("Order not found");
            }

            return _orderMapper.ToOrderDto(order);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
            return orders.Select(o => _orderMapper.ToOrderDto(o));
        }

        public async Task<OrderDto> CancelOrderAsync(int orderId, string userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                
                if (order == null || order.UserId != userId)
                {
                    throw new InvalidOperationException("Order not found");
                }

                if (order.Status != "Pending")
                {
                    throw new InvalidOperationException("Order cannot be cancelled");
                }

                // Restore stock
                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(orderItem.ProductId);
                    if (product != null)
                    {
                        product.Stock += orderItem.Quantity;
                        await _unitOfWork.Products.UpdateProductAsync(product);
                    }
                }

                // Update order status
                order.Status = "Cancelled";
                await _unitOfWork.Orders.UpdateOrderAsync(order);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _orderMapper.ToOrderDto(order);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllOrdersAsync();
            return orders.Select(o => _orderMapper.ToOrderDto(o));
        }
    }
} 