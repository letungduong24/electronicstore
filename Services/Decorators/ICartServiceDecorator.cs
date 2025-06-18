using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services.Decorators
{
    public interface ICartServiceDecorator : ICartService
    {
        ICartService CartService { get; }
    }
} 