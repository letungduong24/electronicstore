import { create } from 'zustand';
import { toast } from 'sonner';
import api from '../services/api';

const useProductStore = create((set, get) => ({
  products: [],
  product: null,
  isLoading: false,
  productTypes: [],
  typeDisplayNames: {},
  productProperties: {},
  latestProducts: [],

  getAllProducts: async () => {
    set({ isLoading: true });
    try {
      const response = await api.get('/api/Products');
      set({ products: response.data, isLoading: false });
    } catch (error) {
      set({ isLoading: false });
      console.error('Error fetching products:', error);
    }
  },

  getProductById: async (id) => {
    set({ isLoading: true });
    try {
      const response = await api.get(`/api/Products/${id}`);
      set({ product: response.data, isLoading: false });
    } catch (error) {
      set({ isLoading: false, product: null });
      console.error('Error fetching product:', error);
    }
  },

  getProductTypes: async () => {
    try {
      const response = await api.get('/api/Products/types');
      set({ productTypes: response.data });
    } catch (error) {
      console.error('Error fetching product types:', error);
    }
  },

  getTypeDisplayNames: async () => {
    try {
      const response = await api.get('/api/Products/types/display-names');
      set({ typeDisplayNames: response.data });
    } catch (error) {
      console.error('Error fetching type display names:', error);
    }
  },

  getProductProperties: async (type) => {
    try {
      const response = await api.get(`/api/Products/types/${type}/properties`);
      set(state => ({
        productProperties: { ...state.productProperties, [type]: response.data }
      }));
    } catch (error) {
      console.error('Error fetching product properties:', error);
    }
  },

  getLatestProducts: async () => {
    set({ isLoading: true });
    try {
      const response = await api.get('/api/Products/latest');
      set({ latestProducts: response.data, isLoading: false });
    } catch (error) {
      set({ isLoading: false });
      console.error('Error fetching latest products:', error);
    }
  },

  // Admin functions
  createProduct: async (productData) => {
    set({ isLoading: true });
    try {
      const response = await api.post('/api/Products', productData);
      await get().getAllProducts(); // Refresh products list
      toast.success('Tạo sản phẩm thành công!');
      return { success: true, product: response.data };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Tạo sản phẩm thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  updateProduct: async (id, productData) => {
    set({ isLoading: true });
    try {
      const response = await api.put(`/api/Products/${id}`, productData);
      await get().getAllProducts(); // Refresh products list
      toast.success('Cập nhật sản phẩm thành công!');
      return { success: true, product: response.data };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Cập nhật sản phẩm thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  deleteProduct: async (id) => {
    set({ isLoading: true });
    try {
      await api.delete(`/api/Products/${id}`);
      await get().getAllProducts(); // Refresh products list
      toast.success('Xóa sản phẩm thành công!');
      return { success: true };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Xóa sản phẩm thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  clearProduct: () => {
    set({ product: null });
  }
}));

export default useProductStore; 