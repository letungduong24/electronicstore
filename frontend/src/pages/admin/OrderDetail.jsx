import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { ShoppingBag, Calendar, MapPin, Image } from 'lucide-react';
import useOrderStore from '../../stores/orderStore';
import api from '../../services/api';

const AdminOrderDetail = () => {
  const { id } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchOrder = async () => {
      setLoading(true);
      setError(null);
      try {
        // Ưu tiên lấy từ store nếu đã có
        const orders = useOrderStore.getState().orders;
        let found = orders.find(o => String(o.id) === String(id));
        if (!found) {
          // Nếu chưa có, gọi API trực tiếp
          const res = await api.get(`/api/Order/${id}`);
          found = res.data;
        }
        setOrder(found);
      } catch (err) {
        setError('Không thể tải chi tiết đơn hàng');
      } finally {
        setLoading(false);
      }
    };
    fetchOrder();
  }, [id]);

  const getStatusColor = (status) => {
    switch (status) {
      case 'Completed':
      case 'Paid':
        return 'bg-green-100 text-green-800';
      case 'Pending':
        return 'bg-yellow-100 text-yellow-800';
      case 'Processing':
        return 'bg-blue-100 text-blue-800';
      case 'Cancelled':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  if (loading) return <div className="p-6">Đang tải chi tiết đơn hàng...</div>;
  if (error || !order) return <div className="p-6 text-red-600">{error || 'Không tìm thấy đơn hàng'}</div>;

  return (
    <div className="max-w-2xl mx-auto bg-white rounded-lg shadow-md p-6 mt-6">
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center space-x-2">
          <ShoppingBag className="h-5 w-5 text-blue-600" />
          <span className="font-semibold text-gray-900">Đơn hàng #{order.id}</span>
        </div>
        <span className={`px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(order.status)}`}>
          {order.status}
        </span>
      </div>
      <div className="mb-4 text-right">
        <div className="text-lg font-bold text-blue-600">
          {parseFloat(order.totalAmount).toLocaleString('vi-VN')} VND
        </div>
        <div className="text-sm text-gray-500">
          {new Date(order.orderDate).toLocaleDateString('vi-VN')}
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
      <div className="border-t pt-4 mt-4">
        <div className="text-sm text-gray-600">Phương thức thanh toán: Ví</div>
      </div>
    </div>
  );
};

export default AdminOrderDetail; 