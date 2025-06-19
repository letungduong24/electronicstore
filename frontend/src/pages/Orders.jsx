import { useEffect, useState } from 'react';
import { ShoppingBag, Calendar, MapPin, DollarSign, Image } from 'lucide-react';
import { toast } from 'sonner';
import api from '../services/api';
import { Link } from 'react-router-dom';

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchOrders();
  }, []);

  const fetchOrders = async () => {
    try {
      const response = await api.get('/api/Order/my-orders');
      setOrders(response.data);
    } catch (error) {
      console.error('Error fetching orders:', error);
      toast.error('Không thể tải danh sách đơn hàng');
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancelOrder = async (orderId) => {
    if (!confirm('Bạn có chắc chắn muốn hủy đơn hàng này?')) {
      return;
    }

    try {
      await api.post(`/api/Order/${orderId}/cancel`);
      toast.success('Hủy đơn hàng thành công');
      fetchOrders();
    } catch (error) {
      const message = error.response?.data?.message || 'Hủy đơn hàng thất bại';
      toast.error(message);
    }
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'Completed':
        return 'bg-green-100 text-green-800';
      case 'Pending':
        return 'bg-yellow-100 text-yellow-800';
      case 'Processing':
        return 'bg-blue-100 text-blue-800';
      case 'Cancelled':
        return 'bg-red-100 text-red-800';
      case 'Paid':
        return 'bg-green-100 text-green-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status) => {
    switch (status) {
      case 'Completed':
        return 'Hoàn thành';
      case 'Pending':
        return 'Chờ xử lý';
      case 'Processing':
        return 'Đang xử lý';
      case 'Cancelled':
        return 'Đã hủy';
      case 'Paid':
        return 'Đã thanh toán';
      default:
        return status;
    }
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <h1 className="text-3xl font-bold text-gray-900">Đơn hàng của tôi</h1>
        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="animate-pulse space-y-4">
            {[...Array(3)].map((_, index) => (
              <div key={index} className="border rounded-lg p-4">
                <div className="bg-gray-300 h-4 rounded w-1/4 mb-2"></div>
                <div className="bg-gray-300 h-3 rounded w-1/2 mb-2"></div>
                <div className="bg-gray-300 h-3 rounded w-1/3"></div>
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Đơn hàng của tôi</h1>
        <div className="flex items-center space-x-2">
          <ShoppingBag className="h-6 w-6 text-blue-600" />
          <span className="text-gray-600">Tổng cộng: {orders.length} đơn hàng</span>
        </div>
      </div>

      {orders.length === 0 ? (
        <div className="text-center py-12">
          <div className="text-6xl mb-4">📦</div>
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Chưa có đơn hàng nào</h2>
          <p className="text-gray-600 mb-8">Bạn chưa đặt đơn hàng nào. Hãy bắt đầu mua sắm!</p>
          <a
            href="/products"
            className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors inline-block"
          >
            Mua sắm ngay
          </a>
        </div>
      ) : (
        <div className="space-y-4">
          {orders.map((order) => (
            <div key={order.id} className="bg-white rounded-lg shadow-md overflow-hidden">
              <div className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <div className="flex items-center space-x-4">
                    <div className="flex items-center space-x-2">
                      <ShoppingBag className="h-5 w-5 text-blue-600" />
                      <span className="font-semibold text-gray-900">
                        Đơn hàng #{order.id}
                      </span>
                    </div>
                    <span className={`px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(order.status)}`}>
                      {getStatusText(order.status)}
                    </span>
                  </div>
                  <div className="text-right">
                    <div className="text-lg font-bold text-blue-600">
                      {parseFloat(order.totalAmount).toLocaleString('vi-VN')} VND
                    </div>
                    <div className="text-sm text-gray-500">
                      {new Date(order.orderDate).toLocaleDateString('vi-VN')}
                    </div>
                  </div>
                </div>

                <div className="space-y-3 mb-4">
                  <div className="flex items-center space-x-2 text-sm text-gray-600">
                    <MapPin className="h-4 w-4" />
                    <span>{order.shippingAddress}</span>
                  </div>
                  <div className="flex items-center space-x-2 text-sm text-gray-600">
                    <Calendar className="h-4 w-4" />
                    <span>Đặt hàng: {new Date(order.orderDate).toLocaleString('vi-VN')}</span>
                  </div>
                </div>

                {/* Order Items */}
                <div className="border-t pt-4">
                  <h4 className="font-medium text-gray-900 mb-3">Sản phẩm:</h4>
                  <div className="space-y-2">
                    {order.orderItems?.map((item) => (
                      <div key={item.id} className="flex items-center justify-between text-sm">
                        <div className="flex items-center space-x-3">
                          <Link to={`/products/${item.productId}`} className="h-10 w-10 bg-gray-200 rounded flex items-center justify-center overflow-hidden">
                            {item.productImageUrl ? (
                              <img 
                                src={item.productImageUrl} 
                                alt={item.productName}
                                className="h-full w-full object-cover"
                                onError={(e) => {
                                  e.target.style.display = 'none';
                                  e.target.nextSibling.style.display = 'flex';
                                }}
                              />
                            ) : null}
                            <div className="text-gray-500 text-xs" style={{ display: item.productImageUrl ? 'none' : 'flex' }}>
                              <Image className="h-4 w-4" />
                            </div>
                          </Link>
                          <div>
                            <Link to={`/products/${item.productId}`} className="font-medium text-gray-900 hover:underline">
                              {item.productName}
                            </Link>
                            <div className="text-gray-500">Số lượng: {item.quantity}</div>
                          </div>
                        </div>
                        <div className="text-right">
                          <div className="font-medium text-gray-900">{parseFloat(item.unitPrice).toLocaleString('vi-VN')} VND</div>
                          <div className="text-gray-500">Tổng: {parseFloat(item.totalPrice).toLocaleString('vi-VN')} VND</div>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>

                {/* Actions */}
                <div className="border-t pt-4 mt-4">
                  <div className="flex justify-between items-center">
                    <div className="text-sm text-gray-600">
                      Phương thức thanh toán: Ví
                    </div>
                    {order.status === 'Pending' && (
                      <button
                        onClick={() => handleCancelOrder(order.id)}
                        className="px-4 py-2 text-red-600 border border-red-600 rounded-md hover:bg-red-50 transition-colors"
                      >
                        Hủy đơn hàng
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Orders; 