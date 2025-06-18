# Decorator Pattern Implementation

## Tổng quan

Decorator Pattern đã được áp dụng cho `CartService` để tách biệt các concerns và làm cho code dễ mở rộng hơn.

## Cấu trúc Decorator

```
LoggingCartDecorator
    ↓ (wraps)
StockValidationCartDecorator
    ↓ (wraps)
CartService (base implementation)
```

## Các Decorator Classes

### 1. BaseCartServiceDecorator
- Abstract base class cho tất cả decorators
- Implements tất cả methods của `ICartService`
- Delegates calls đến wrapped service

### 2. StockValidationCartDecorator
- **Chức năng**: Kiểm tra stock trước khi thêm/cập nhật cart
- **Validation rules**:
  - Kiểm tra product có tồn tại không
  - Kiểm tra stock có đủ không
  - Kiểm tra tổng quantity trong cart + quantity mới có vượt quá stock không
- **Methods được override**:
  - `AddToCartAsync()`: Validate trước khi thêm
  - `UpdateCartItemAsync()`: Validate trước khi cập nhật

### 3. LoggingCartDecorator
- **Chức năng**: Log tất cả cart operations
- **Logging**:
  - Log thông tin trước khi thực hiện operation
  - Log kết quả thành công
  - Log lỗi nếu có exception
- **Methods được override**: Tất cả methods của `ICartService`

## Lợi ích của Decorator Pattern

### 1. Single Responsibility Principle
- Mỗi decorator chỉ có một trách nhiệm duy nhất
- `CartService`: Core business logic
- `StockValidationCartDecorator`: Stock validation
- `LoggingCartDecorator`: Logging

### 2. Open/Closed Principle
- Có thể thêm decorators mới mà không cần sửa code hiện tại
- Ví dụ: `CachingCartDecorator`, `RateLimitingCartDecorator`, `AuditCartDecorator`

### 3. Separation of Concerns
- Logic validation tách biệt khỏi business logic
- Logging tách biệt khỏi core functionality

### 4. Testability
- Có thể test từng decorator riêng lẻ
- Có thể mock decorators để test core service

## Cách thêm Decorator mới

### Ví dụ: Thêm CachingDecorator

```csharp
public class CachingCartDecorator : BaseCartServiceDecorator
{
    private readonly IMemoryCache _cache;
    
    public CachingCartDecorator(ICartService cartService, IMemoryCache cache) 
        : base(cartService)
    {
        _cache = cache;
    }
    
    public override async Task<CartDto> GetUserCartAsync(string userId)
    {
        var cacheKey = $"cart_{userId}";
        
        if (_cache.TryGetValue(cacheKey, out CartDto cachedCart))
        {
            return cachedCart;
        }
        
        var cart = await base.GetUserCartAsync(userId);
        _cache.Set(cacheKey, cart, TimeSpan.FromMinutes(5));
        
        return cart;
    }
}
```

### Cập nhật Factory

```csharp
public static ICartService CreateDecoratedCartService(IServiceProvider serviceProvider)
{
    var cartService = serviceProvider.GetRequiredService<CartService>();
    var productRepository = serviceProvider.GetRequiredService<IProductRepository>();
    var cartRepository = serviceProvider.GetRequiredService<ICartRepository>();
    var logger = serviceProvider.GetRequiredService<ILogger<LoggingCartDecorator>>();
    var cache = serviceProvider.GetRequiredService<IMemoryCache>();
    
    // Chain: Logging -> Caching -> StockValidation -> CartService
    var stockValidationDecorator = new StockValidationCartDecorator(cartService, productRepository, cartRepository);
    var cachingDecorator = new CachingCartDecorator(stockValidationDecorator, cache);
    var loggingDecorator = new LoggingCartDecorator(cachingDecorator, logger);
    
    return loggingDecorator;
}
```

## Thứ tự Decorator quan trọng

Thứ tự decorator ảnh hưởng đến behavior:

```
Logging -> Caching -> StockValidation -> CartService
```

- **Logging** sẽ log tất cả operations (bao gồm cả cache hits/misses)
- **Caching** sẽ cache kết quả sau khi validation
- **StockValidation** sẽ validate trước khi gọi core service

## Testing

### Test từng Decorator riêng lẻ

```csharp
[Test]
public async Task StockValidationDecorator_Should_ThrowException_When_InsufficientStock()
{
    // Arrange
    var mockCartService = new Mock<ICartService>();
    var mockProductRepo = new Mock<IProductRepository>();
    var mockCartRepo = new Mock<ICartRepository>();
    
    var decorator = new StockValidationCartDecorator(
        mockCartService.Object, 
        mockProductRepo.Object, 
        mockCartRepo.Object);
    
    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => decorator.AddToCartAsync("user1", new AddToCartDto { ProductId = 1, Quantity = 100 }));
}
```

## Kết luận

Decorator Pattern giúp:
- Code dễ maintain và extend
- Tách biệt concerns rõ ràng
- Dễ dàng thêm tính năng mới
- Testability tốt hơn
- Tuân thủ SOLID principles 