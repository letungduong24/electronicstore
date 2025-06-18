import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { Package, Plus, Edit, Trash2, Search, Upload, X } from 'lucide-react';
import { toast } from 'sonner';
import useProductStore from '../../stores/productStore';
import { uploadImage } from '../../services/uploadImage';

const Products = () => {
  const { 
    products, 
    getAllProducts, 
    getProductTypes, 
    getTypeDisplayNames,
    getProductProperties,
    productTypes, 
    typeDisplayNames,
    productProperties,
    createProduct, 
    updateProduct, 
    deleteProduct, 
    isLoading 
  } = useProductStore();
  const [showModal, setShowModal] = useState(false);
  const [editingProduct, setEditingProduct] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedType, setSelectedType] = useState('');
  const [imageFile, setImageFile] = useState(null);
  const [imagePreview, setImagePreview] = useState('');
  const [isUploading, setIsUploading] = useState(false);
  const [selectedProductType, setSelectedProductType] = useState('');
  const [specificProperties, setSpecificProperties] = useState({});

  const {
    register,
    handleSubmit,
    reset,
    watch,
    formState: { errors },
  } = useForm();

  const watchedProductType = watch('productType');

  useEffect(() => {
    getAllProducts();
    getProductTypes();
    getTypeDisplayNames();
  }, [getAllProducts, getProductTypes, getTypeDisplayNames]);

  useEffect(() => {
    if (watchedProductType && watchedProductType !== selectedProductType) {
      setSelectedProductType(watchedProductType);
      getProductProperties(watchedProductType);
      setSpecificProperties({});
    }
  }, [watchedProductType, selectedProductType, getProductProperties]);

  const filteredProducts = products.filter(product => {
    const matchesSearch = product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          product.description.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesType = !selectedType || product.type === selectedType;
    return matchesSearch && matchesType;
  });

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      const reader = new FileReader();
      reader.onload = (e) => {
        setImagePreview(e.target.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const removeImage = () => {
    setImageFile(null);
    setImagePreview('');
  };

  const handleCreateProduct = async (data) => {
    setIsUploading(true);
    try {
      let imageUrl = '';
      if (imageFile) {
        const formData = new FormData();
        formData.append('file', imageFile);
        formData.append('upload_preset', import.meta.env.VITE_CLOUDINARY_UPLOAD_PRESET || 'ml_default');
        
        imageUrl = await uploadImage(formData);
        if (!imageUrl) {
          toast.error('Tải ảnh lên thất bại');
          setIsUploading(false);
          return;
        }
      }

      // Convert frontend data to backend format
      const productData = {
        name: data.name,
        description: data.description,
        price: parseFloat(data.price),
        stock: parseInt(data.stockQuantity),
        brand: data.brand || '',
        model: data.model || '',
        imageUrl: imageUrl,
        type: data.productType.toLowerCase(), // Convert to backend format
        properties: specificProperties // Use the specific properties
      };

      const result = await createProduct(productData);
      if (result.success) {
        setShowModal(false);
        reset();
        setImageFile(null);
        setImagePreview('');
        setSpecificProperties({});
      }
    } catch (error) {
      console.error('Error creating product:', error);
      toast.error('Tạo sản phẩm thất bại');
    } finally {
      setIsUploading(false);
    }
  };

  const handleUpdateProduct = async (data) => {
    setIsUploading(true);
    try {
      let imageUrl = editingProduct.imageUrl || '';
      if (imageFile) {
        const formData = new FormData();
        formData.append('file', imageFile);
        formData.append('upload_preset', import.meta.env.VITE_CLOUDINARY_UPLOAD_PRESET || 'ml_default');
        
        imageUrl = await uploadImage(formData);
        if (!imageUrl) {
          toast.error('Tải ảnh lên thất bại');
          setIsUploading(false);
          return;
        }
      }

      // Convert frontend data to backend format
      const productData = {
        id: editingProduct.id,
        name: data.name,
        description: data.description,
        price: parseFloat(data.price),
        stock: parseInt(data.stockQuantity),
        brand: data.brand || '',
        model: data.model || '',
        imageUrl: imageUrl,
        type: data.productType.toLowerCase(), // Convert to backend format
        properties: specificProperties // Use the specific properties
      };

      const result = await updateProduct(editingProduct.id, productData);
      if (result.success) {
        setShowModal(false);
        setEditingProduct(null);
        reset();
        setImageFile(null);
        setImagePreview('');
        setSpecificProperties({});
      }
    } catch (error) {
      console.error('Error updating product:', error);
      toast.error('Cập nhật sản phẩm thất bại');
    } finally {
      setIsUploading(false);
    }
  };

  const handleDeleteProduct = async (id) => {
    if (!confirm('Bạn có chắc chắn muốn xóa sản phẩm này?')) {
      return;
    }

    const result = await deleteProduct(id);
    if (result.success) {
      toast.success('Xóa sản phẩm thành công');
    }
  };

  const openCreateModal = () => {
    setEditingProduct(null);
    setShowModal(true);
    reset();
    setImageFile(null);
    setImagePreview('');
    setSpecificProperties({});
    setSelectedProductType('');
  };

  const openEditModal = (product) => {
    setEditingProduct(product);
    setShowModal(true);
    reset({
      name: product.name,
      description: product.description,
      price: product.price,
      stockQuantity: product.stock,
      productType: product.type,
      brand: product.brand,
      model: product.model
    });
    setImageFile(null);
    setImagePreview(product.imageUrl || '');
    setSpecificProperties(product.properties || {});
    setSelectedProductType(product.type);
    getProductProperties(product.type);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingProduct(null);
    reset();
    setImageFile(null);
    setImagePreview('');
    setSpecificProperties({});
    setSelectedProductType('');
  };

  const handlePropertyChange = (propertyName, value) => {
    setSpecificProperties(prev => ({
      ...prev,
      [propertyName]: value
    }));
  };

  const getPropertyLabel = (propertyName) => {
    const labels = {
      'ScreenSize': 'Kích thước màn hình',
      'Scope': 'Công suất',
      'Capacity': 'Dung tích'
    };
    return labels[propertyName] || propertyName;
  };

  const getPropertyUnit = (propertyName) => {
    const units = {
      'ScreenSize': 'inch',
      'Scope': 'BTU',
      'Capacity': 'kg'
    };
    return units[propertyName] || '';
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Quản lý sản phẩm</h1>
        <button
          onClick={openCreateModal}
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors flex items-center"
        >
          <Plus className="h-4 w-4 mr-2" />
          Thêm sản phẩm
        </button>
      </div>

      <div className="bg-white rounded-lg shadow-md p-6">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm sản phẩm..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
            />
          </div>

          <select
            value={selectedType}
            onChange={(e) => setSelectedType(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="">Tất cả loại</option>
            {productTypes.map(type => (
              <option key={type} value={type}>
                {typeDisplayNames[type] || type}
              </option>
            ))}
          </select>

          <button
            onClick={() => {
              setSearchTerm('');
              setSelectedType('');
            }}
            className="px-4 py-2 bg-gray-100 text-gray-700 rounded-md hover:bg-gray-200 transition-colors"
          >
            Xóa bộ lọc
          </button>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow-md">
        <div className="p-6 border-b">
          <h2 className="text-lg font-semibold text-gray-900">
            Danh sách sản phẩm ({filteredProducts.length})
          </h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Sản phẩm
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Loại
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Giá
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Tồn kho
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Thao tác
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {filteredProducts.map((product) => (
                <tr key={product.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center">
                      <div className="h-10 w-10 bg-gray-200 rounded-lg flex items-center justify-center overflow-hidden">
                        {product.imageUrl ? (
                          <img 
                            src={product.imageUrl} 
                            alt={product.name}
                            className="h-full w-full object-cover"
                          />
                        ) : (
                          <Package className="h-5 w-5 text-gray-500" />
                        )}
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {product.name}
                        </div>
                        <div className="text-sm text-gray-500">
                          {product.description}
                        </div>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-blue-100 text-blue-800">
                      {typeDisplayNames[product.type] || product.type}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    ${product.price}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {product.stock}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                    <div className="flex space-x-2">
                      <button
                        onClick={() => openEditModal(product)}
                        className="text-blue-600 hover:text-blue-900 flex items-center"
                      >
                        <Edit className="h-4 w-4 mr-1" />
                        Sửa
                      </button>
                      <button
                        onClick={() => handleDeleteProduct(product.id)}
                        className="text-red-600 hover:text-red-900 flex items-center"
                      >
                        <Trash2 className="h-4 w-4 mr-1" />
                        Xóa
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-full max-w-2xl shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                {editingProduct ? 'Chỉnh sửa sản phẩm' : 'Thêm sản phẩm mới'}
              </h3>
              
              <form onSubmit={handleSubmit(editingProduct ? handleUpdateProduct : handleCreateProduct)} className="space-y-6">
                {/* Image Upload */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Hình ảnh sản phẩm
                  </label>
                  <div className="space-y-4">
                    {imagePreview && (
                      <div className="relative">
                        <img
                          src={imagePreview}
                          alt="Preview"
                          className="h-32 w-32 object-cover rounded-lg border"
                        />
                        <button
                          type="button"
                          onClick={removeImage}
                          className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full p-1 hover:bg-red-600"
                        >
                          <X className="h-4 w-4" />
                        </button>
                      </div>
                    )}
                    <div className="flex items-center justify-center w-full">
                      <label className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100">
                        <div className="flex flex-col items-center justify-center pt-5 pb-6">
                          <Upload className="w-8 h-8 mb-4 text-gray-500" />
                          <p className="mb-2 text-sm text-gray-500">
                            <span className="font-semibold">Click để tải ảnh</span> hoặc kéo thả
                          </p>
                          <p className="text-xs text-gray-500">PNG, JPG, GIF (MAX. 10MB)</p>
                        </div>
                        <input
                          type="file"
                          className="hidden"
                          accept="image/*"
                          onChange={handleImageChange}
                        />
                      </label>
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Tên sản phẩm
                    </label>
                    <input
                      {...register('name', { required: 'Tên sản phẩm là bắt buộc' })}
                      type="text"
                      className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Nhập tên sản phẩm"
                    />
                    {errors.name && (
                      <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>
                    )}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Loại sản phẩm
                    </label>
                    <select
                      {...register('productType', { required: 'Loại sản phẩm là bắt buộc' })}
                      className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value="">Chọn loại sản phẩm</option>
                      {productTypes.map(type => (
                        <option key={type} value={type}>
                          {typeDisplayNames[type] || type}
                        </option>
                      ))}
                    </select>
                    {errors.productType && (
                      <p className="text-red-500 text-sm mt-1">{errors.productType.message}</p>
                    )}
                  </div>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Mô tả
                  </label>
                  <textarea
                    {...register('description')}
                    rows={3}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Nhập mô tả sản phẩm"
                  />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Giá
                    </label>
                    <input
                      {...register('price', { 
                        required: 'Giá là bắt buộc',
                        min: { value: 0, message: 'Giá phải lớn hơn 0' }
                      })}
                      type="number"
                      step="0.01"
                      className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                      placeholder="0.00"
                    />
                    {errors.price && (
                      <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>
                    )}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Số lượng tồn kho
                    </label>
                    <input
                      {...register('stockQuantity', { 
                        required: 'Số lượng tồn kho là bắt buộc',
                        min: { value: 0, message: 'Số lượng phải lớn hơn hoặc bằng 0' }
                      })}
                      type="number"
                      className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                      placeholder="0"
                    />
                    {errors.stockQuantity && (
                      <p className="text-red-500 text-sm mt-1">{errors.stockQuantity.message}</p>
                    )}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Thương hiệu
                    </label>
                    <input
                      {...register('brand')}
                      type="text"
                      className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Nhập thương hiệu"
                    />
                  </div>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Model
                  </label>
                  <input
                    {...register('model')}
                    type="text"
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Nhập model"
                  />
                </div>

                {/* Dynamic Properties based on Product Type */}
                {selectedProductType && productProperties[selectedProductType] && (
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Thuộc tính đặc biệt
                    </label>
                    <div className="space-y-3">
                      {productProperties[selectedProductType][selectedProductType]?.map(property => (
                        <div key={property}>
                          <label className="block text-sm font-medium text-gray-700 mb-1">
                            {getPropertyLabel(property)}
                          </label>
                          <input
                            type="text"
                            value={specificProperties[property] || ''}
                            onChange={(e) => handlePropertyChange(property, e.target.value)}
                            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                            placeholder={`Nhập ${getPropertyLabel(property).toLowerCase()} ${getPropertyUnit(property) ? `(${getPropertyUnit(property)})` : ''}`}
                          />
                        </div>
                      ))}
                    </div>
                  </div>
                )}

                <div className="flex space-x-3">
                  <button
                    type="submit"
                    disabled={isLoading || isUploading}
                    className="flex-1 bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50"
                  >
                    {isLoading || isUploading ? 'Đang xử lý...' : (editingProduct ? 'Cập nhật' : 'Tạo sản phẩm')}
                  </button>
                  <button
                    type="button"
                    onClick={closeModal}
                    className="flex-1 bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400 transition-colors"
                  >
                    Hủy
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Products; 