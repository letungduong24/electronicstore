import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { ShoppingCart, ArrowLeft } from 'lucide-react';
import useProductStore from '../stores/productStore';
import useCartStore from '../stores/cartStore';
import useAuthStore from '../stores/authStore';
import { getPropertyLabel, getPropertyUnit } from '../utils/productUtils';

const ProductDetail = () => {
  const { id } = useParams();
  const { product, getProductById, getTypeDisplayNames, typeDisplayNames, clearProduct, isLoading } = useProductStore();
  const { addToCart } = useCartStore();
  const { isAuthenticated } = useAuthStore();
  const [quantity, setQuantity] = useState(1);

  useEffect(() => {
    if (id) {
      getProductById(id);
    }
    getTypeDisplayNames();
    return () => clearProduct();
  }, [id, getProductById, getTypeDisplayNames, clearProduct]);

  const handleAddToCart = async () => {
    if (!isAuthenticated) {
      window.location.href = '/login';
      return;
    }
    await addToCart(product.id, quantity);
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div className="animate-pulse">
          <div className="bg-gray-300 h-8 w-32 rounded mb-4"></div>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            <div className="bg-gray-300 h-96 rounded-lg"></div>
            <div className="space-y-4">
              <div className="bg-gray-300 h-8 rounded"></div>
              <div className="bg-gray-300 h-4 rounded w-3/4"></div>
              <div className="bg-gray-300 h-6 rounded w-1/2"></div>
              <div className="bg-gray-300 h-32 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="text-center py-12">
        <div className="text-6xl mb-4">❌</div>
        <h2 className="text-2xl font-bold text-gray-900 mb-4">Không tìm thấy sản phẩm</h2>
        <p className="text-gray-600 mb-8">Sản phẩm bạn đang tìm kiếm không tồn tại</p>
        <Link
          to="/products"
          className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors inline-block"
        >
          Quay lại danh sách sản phẩm
        </Link>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Breadcrumb */}
      <div className="flex items-center space-x-2 text-sm text-gray-600">
        <Link to="/" className="hover:text-blue-600">Trang chủ</Link>
        <span>/</span>
        <Link to="/products" className="hover:text-blue-600">Sản phẩm</Link>
        <span>/</span>
        <span className="text-gray-900">{product.name}</span>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Product Image */}
        <div className="bg-gray-200 rounded-lg h-96 flex items-center justify-center overflow-hidden">
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
        <div className="space-y-6">
          <div>
            <span className="px-3 py-1 bg-blue-100 text-blue-800 text-sm font-medium rounded-full">
              {typeDisplayNames[product.type] || product.type}
            </span>
          </div>

          <h1 className="text-3xl font-bold text-gray-900">{product.name}</h1>

          <div className="flex items-center space-x-4">
            <span className="text-gray-600">Còn lại: {product.stock}</span>
          </div>

          <div className="text-3xl font-bold text-blue-600">
            {product.price.toLocaleString('vi-VN')} VND
          </div>

          {/* Product Description */}
          <div className="space-y-4">
            <h3 className="text-lg font-semibold text-gray-900">Mô tả sản phẩm</h3>
            <div className="bg-gray-50 p-4 rounded-lg">
              <p className="text-gray-600 leading-relaxed">
                {product.description || 'Chưa có mô tả chi tiết cho sản phẩm này.'}
              </p>
            </div>
          </div>

          {/* Product Details */}
          <div className="space-y-3">
            {product.brand && (
              <div className="flex">
                <span className="font-medium text-gray-700 w-24">Thương hiệu:</span>
                <span className="text-gray-600">{product.brand}</span>
              </div>
            )}
            {product.model && (
              <div className="flex">
                <span className="font-medium text-gray-700 w-24">Model:</span>
                <span className="text-gray-600">{product.model}</span>
              </div>
            )}
            <div className="flex">
              <span className="font-medium text-gray-700 w-24">Loại:</span>
              <span className="text-gray-600">{typeDisplayNames[product.type] || product.type}</span>
            </div>
            
            {/* Specific Properties */}
            {product.properties && Object.keys(product.properties).length > 0 && (
              <div className="mt-4 pt-4 border-t border-gray-200">
                <h4 className="font-medium text-gray-700 mb-3">Thông số kỹ thuật:</h4>
                <div className="space-y-2">
                  {Object.entries(product.properties).map(([key, value]) => (
                    <div key={key} className="flex">
                      <span className="font-medium text-gray-700 w-32">
                        {getPropertyLabel(key)}:
                      </span>
                      <span className="text-gray-600">
                        {value} {getPropertyUnit(key)}
                      </span>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>

          {/* Add to Cart */}
          <div className="space-y-4">
            <div className="flex items-center space-x-4">
              <label className="font-medium text-gray-700">Số lượng:</label>
              <div className="flex items-center border border-gray-300 rounded-md">
                <button
                  onClick={() => setQuantity(Math.max(1, quantity - 1))}
                  className="px-3 py-2 hover:bg-gray-100"
                  disabled={quantity <= 1}
                >
                  -
                </button>
                <span className="px-4 py-2 border-x border-gray-300 min-w-[60px] text-center">
                  {quantity}
                </span>
                <button
                  onClick={() => setQuantity(Math.min(product.stock, quantity + 1))}
                  className="px-3 py-2 hover:bg-gray-100"
                  disabled={quantity >= product.stock}
                >
                  +
                </button>
              </div>
            </div>

            <button
              onClick={handleAddToCart}
              disabled={product.stock === 0}
              className="w-full bg-blue-600 text-white py-3 px-6 rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
            >
              <ShoppingCart className="h-5 w-5 mr-2" />
              {product.stock === 0 ? 'Hết hàng' : 'Thêm vào giỏ hàng'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail; 