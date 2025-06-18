import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ProductCard from '../components/ProductCard';
import useProductStore from '../stores/productStore';

const Home = () => {
  const { latestProducts, getLatestProducts, getTypeDisplayNames, typeDisplayNames, isLoading } = useProductStore();
  const navigate = useNavigate();

  useEffect(() => {
    getLatestProducts();
    getTypeDisplayNames();
  }, [getLatestProducts, getTypeDisplayNames]);

  // Xử lý click vào danh mục
  const handleCategoryClick = (type) => {
    navigate(`/products?type=${type}`);
  };

  return (
    <div className="space-y-8">
      {/* Hero Section */}
      <div className="bg-gradient-to-r from-blue-600 to-purple-600 text-white py-16 px-4 rounded-lg">
        <div className="max-w-4xl mx-auto text-center">
          <h1 className="text-4xl md:text-6xl font-bold mb-4">
            Chào mừng đến với ElectronicStore
          </h1>
          <p className="text-xl mb-8 text-blue-100">
            Khám phá các sản phẩm điện tử chất lượng cao với giá cả hợp lý
          </p>
          <a
            href="/products"
            className="bg-white text-blue-600 px-8 py-3 rounded-lg font-semibold hover:bg-gray-100 transition-colors inline-block"
          >
            Xem tất cả sản phẩm
          </a>
        </div>
      </div>

      {/* Latest Products */}
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-6">Sản phẩm mới nhất</h2>
        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            {[...Array(4)].map((_, index) => (
              <div key={index} className="bg-white rounded-lg shadow-md p-6 animate-pulse">
                <div className="bg-gray-300 h-48 rounded-lg mb-4"></div>
                <div className="bg-gray-300 h-4 rounded mb-2"></div>
                <div className="bg-gray-300 h-4 rounded mb-2 w-3/4"></div>
                <div className="bg-gray-300 h-6 rounded w-1/2"></div>
              </div>
            ))}
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            {latestProducts.map((product) => (
              <ProductCard key={product.id} product={product} typeDisplayNames={typeDisplayNames} />
            ))}
          </div>
        )}
      </div>

      {/* Categories Section */}
      <div className="bg-white rounded-lg shadow-md p-6">
        <h2 className="text-2xl font-bold text-gray-900 mb-6">Danh mục sản phẩm</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
          <div className="text-center p-4 bg-blue-50 rounded-lg hover:bg-blue-100 transition-colors cursor-pointer" onClick={() => handleCategoryClick('tv')}>
            <div className="text-3xl mb-2">📺</div>
            <h3 className="font-semibold">Tivi</h3>
          </div>
          <div className="text-center p-4 bg-green-50 rounded-lg hover:bg-green-100 transition-colors cursor-pointer" onClick={() => handleCategoryClick('airconditioner')}>
            <div className="text-3xl mb-2">❄️</div>
            <h3 className="font-semibold">Điều hòa</h3>
          </div>
          <div className="text-center p-4 bg-purple-50 rounded-lg hover:bg-purple-100 transition-colors cursor-pointer" onClick={() => handleCategoryClick('washingmachine')}>
            <div className="text-3xl mb-2">🌀</div>
            <h3 className="font-semibold">Máy giặt</h3>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home; 