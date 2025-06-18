import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Trash2, Minus, Plus, ShoppingBag } from 'lucide-react';
import useCartStore from '../stores/cartStore';
import useProductStore from '../stores/productStore';
import useAuthStore from '../stores/authStore';
import api from '../services/api';
import { toast } from 'sonner';

const Cart = () => {
  const { cart, getCart, updateCartItem, removeFromCart, clearCart, getCartTotal, isLoading } = useCartStore();
  const { getTypeDisplayNames, typeDisplayNames } = useProductStore();
  const { user, isAuthenticated } = useAuthStore();
  const navigate = useNavigate();
  const [isCreatingOrder, setIsCreatingOrder] = useState(false);

  useEffect(() => {
    if (isAuthenticated) {
      getCart();
    }
    getTypeDisplayNames();
  }, [isAuthenticated, getCart, getTypeDisplayNames]);

  const handleQuantityChange = async (cartItemId, newQuantity) => {
    if (newQuantity < 1) return;
    await updateCartItem(cartItemId, newQuantity);
  };

  const handleRemoveItem = async (cartItemId) => {
    await removeFromCart(cartItemId);
  };

  const handleClearCart = async () => {
    await clearCart();
  };

  const handleCreateOrder = async () => {
    if (!cart?.items || cart.items.length === 0) {
      toast.error('Giỏ hàng trống!');
      return;
    }

    setIsCreatingOrder(true);
    try {
      const orderData = {
        shippingAddress: user?.address || 'Default Address',
        paymentMethod: 'Wallet',
        notes: ''
      };

      const response = await api.post('/api/Order/create', orderData);
      toast.success('Đặt hàng thành công!');
      navigate('/orders');
    } catch (error) {
      const message = error.response?.data?.message || 'Đặt hàng thất bại';
      toast.error(message);
    } finally {
      setIsCreatingOrder(false);
    }
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <h1 className="text-3xl font-bold text-gray-900">Giỏ hàng</h1>
        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="animate-pulse space-y-4">
            {[...Array(3)].map((_, index) => (
              <div key={index} className="flex items-center space-x-4">
                <div className="bg-gray-300 h-20 w-20 rounded"></div>
                <div className="flex-1 space-y-2">
                  <div className="bg-gray-300 h-4 rounded w-3/4"></div>
                  <div className="bg-gray-300 h-4 rounded w-1/2"></div>
                </div>
                <div className="bg-gray-300 h-8 w-20 rounded"></div>
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  if (!cart || !cart.items || cart.items.length === 0) {
    return (
      <div className="text-center py-12">
        <div className="text-6xl mb-4">🛒</div>
        <h2 className="text-2xl font-bold text-gray-900 mb-4">Giỏ hàng trống</h2>
        <p className="text-gray-600 mb-8">Bạn chưa có sản phẩm nào trong giỏ hàng</p>
        <Link
          to="/products"
          className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors inline-block"
        >
          Mua sắm ngay
        </Link>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Giỏ hàng</h1>
        <button
          onClick={handleClearCart}
          className="text-red-600 hover:text-red-700 text-sm font-medium"
        >
          Xóa tất cả
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2">
          <div className="bg-white rounded-lg shadow-md">
            <div className="p-6 border-b">
              <h2 className="text-lg font-semibold text-gray-900">
                Sản phẩm ({cart.items.length})
              </h2>
            </div>
            <div className="divide-y">
              {cart.items.map((item) => (
                <div key={item.id} className="p-6 flex items-center space-x-4">
                  <div className="h-20 w-20 bg-gray-200 rounded-lg flex items-center justify-center overflow-hidden">
                    {item.productImage ? (
                      <img 
                        src={item.productImage} 
                        alt={item.productName}
                        className="h-full w-full object-cover"
                        onError={(e) => {
                          e.target.style.display = 'none';
                          e.target.nextSibling.style.display = 'flex';
                        }}
                      />
                    ) : null}
                    <div className="text-gray-500 text-sm" style={{ display: item.productImage ? 'none' : 'flex' }}>
                      Hình ảnh
                    </div>
                  </div>
                  <div className="flex-1">
                    <h3 className="text-lg font-medium text-gray-900">{item.productName}</h3>
                    <p className="text-gray-600 text-sm">{parseFloat(item.productPrice).toLocaleString('vi-VN')} VND mỗi sản phẩm</p>
                    <p className="text-gray-500 text-sm">Còn lại: {item.stock}</p>
                    <span className="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded-full">
                      {typeDisplayNames[item.productType] || item.productType}
                    </span>
                  </div>
                  <div className="flex flex-col items-end min-w-[160px]">
                    <div className="flex items-center space-x-2 mb-2">
                      <button
                        onClick={() => handleQuantityChange(item.id, item.quantity - 1)}
                        disabled={item.quantity <= 1}
                        className="p-1 rounded-md hover:bg-gray-100 disabled:opacity-50"
                      >
                        <Minus className="h-4 w-4" />
                      </button>
                      <span className="w-12 text-center font-medium">{item.quantity}</span>
                      <button
                        onClick={() => handleQuantityChange(item.id, item.quantity + 1)}
                        disabled={item.quantity >= item.stock}
                        className="p-1 rounded-md hover:bg-gray-100 disabled:opacity-50"
                      >
                        <Plus className="h-4 w-4" />
                      </button>
                    </div>
                    <div className="flex items-center space-x-2">
                      <p className="text-lg font-semibold text-gray-900 min-w-[90px] text-right">
                        {parseFloat(item.productPrice * item.quantity).toLocaleString('vi-VN')} VND
                      </p>
                      <button
                        onClick={() => handleRemoveItem(item.id)}
                        className="text-red-600 hover:text-red-700 text-sm"
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="lg:col-span-1">
          <div className="bg-white rounded-lg shadow-md p-6 sticky top-6">
            <h2 className="text-lg font-semibold text-gray-900 mb-4">Tóm tắt đơn hàng</h2>
            
            <div className="space-y-3 mb-6">
              <div className="flex justify-between">
                <span className="text-gray-600">Tạm tính:</span>
                <span className="font-medium">{parseFloat(getCartTotal()).toLocaleString('vi-VN')} VND</span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Phí vận chuyển:</span>
                <span className="font-medium">0 VND</span>
              </div>
              <div className="border-t pt-3">
                <div className="flex justify-between">
                  <span className="text-lg font-semibold">Tổng cộng:</span>
                  <span className="text-lg font-bold text-blue-600">
                    {parseFloat(getCartTotal()).toLocaleString('vi-VN')} VND
                  </span>
                </div>
              </div>
            </div>

            <button
              onClick={handleCreateOrder}
              disabled={isCreatingOrder}
              className="w-full bg-blue-600 text-white py-3 px-4 rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
            >
              {isCreatingOrder ? (
                <>
                  <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                  Đang xử lý...
                </>
              ) : (
                <>
                  <ShoppingBag className="h-4 w-4 mr-2" />
                  Đặt hàng ngay
                </>
              )}
            </button>

            <div className="mt-4 text-center">
              <Link
                to="/products"
                className="text-blue-600 hover:text-blue-700 text-sm font-medium"
              >
                Tiếp tục mua sắm
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Cart; 