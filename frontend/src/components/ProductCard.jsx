import { Link } from 'react-router-dom';
import { ShoppingCart } from 'lucide-react';
import useCartStore from '../stores/cartStore';
import useAuthStore from '../stores/authStore';
import { getPropertyLabel, getPropertyUnit } from '../utils/productUtils';

const ProductCard = ({ product, typeDisplayNames }) => {
  const { addToCart } = useCartStore();
  const { isAuthenticated } = useAuthStore();

  const handleAddToCart = async (e) => {
    e.preventDefault();
    if (!isAuthenticated) {
      window.location.href = '/login';
      return;
    }
    await addToCart(product.id, 1);
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow flex flex-col h-full">
      {/* Product Image */}
      <div className="h-48 bg-gray-200 flex items-center justify-center overflow-hidden">
        {product.imageUrl ? (
          <img 
            src={product.imageUrl} 
            alt={product.name}
            className="h-full w-full object-cover"
          />
        ) : (
          <span className="text-gray-500 text-lg">Hình ảnh sản phẩm</span>
        )}
      </div>

      {/* Product Info */}
      <div className="p-6 flex flex-col flex-grow">
        <h3 className="text-lg font-semibold text-gray-900 mb-2">
          {product.name}
        </h3>
        <p className="text-gray-600 text-sm mb-4 line-clamp-2">
          {product.description}
        </p>
        
        <div className="mt-auto">
          {/* Specific Properties */}
          {product.properties && Object.keys(product.properties).length > 0 && (
            <div className="mb-3">
              {Object.entries(product.properties).map(([key, value]) => (
                <div key={key} className="text-xs text-gray-500 mb-1">
                  <span className="font-medium">{getPropertyLabel(key)}:</span> {value} {getPropertyUnit(key)}
                </div>
              ))}
            </div>
          )}
          
          <div className="flex items-center justify-between mb-2">
            <span className="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded-full">
              {typeDisplayNames[product.type] || product.type}
            </span>
          </div>
          
          <div className="flex items-center justify-between mb-4">
            <span className="text-xl font-bold text-blue-600">
              {product.price.toLocaleString('vi-VN')} VND
            </span>
          </div>

          <div className="flex space-x-2">
            <Link
              to={`/products/${product.id}`}
              className="flex-1 bg-gray-100 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-200 transition-colors text-center text-sm font-medium"
            >
              Chi tiết
            </Link>
            <button
              onClick={handleAddToCart}
              disabled={product.stock === 0}
              className="flex-1 bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
            >
              <ShoppingCart className="h-4 w-4 mr-1" />
              {product.stock === 0 ? 'Hết hàng' : 'Thêm'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductCard; 