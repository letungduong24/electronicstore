import { create } from 'zustand';
import { toast } from 'sonner';
import api from '../services/api';

const useCartStore = create((set, get) => ({
  cart: null,
  isLoading: false,

  getCart: async () => {
    set({ isLoading: true });
    try {
      const response = await api.get('/api/Cart');
      set({ cart: response.data, isLoading: false });
    } catch (error) {
      set({ isLoading: false });
      console.error('Error fetching cart:', error);
    }
  },

  addToCart: async (productId, quantity = 1) => {
    set({ isLoading: true });
    try {
      const response = await api.post('/api/Cart/add', {
        productId,
        quantity
      });
      set({ cart: response.data, isLoading: false });
      toast.success('Đã thêm vào giỏ hàng!');
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Thêm vào giỏ hàng thất bại';
      toast.error(message);
    }
  },

  updateCartItem: async (cartItemId, quantity) => {
    set({ isLoading: true });
    try {
      const response = await api.put(`/api/Cart/items/${cartItemId}`, {
        quantity
      });
      set({ cart: response.data, isLoading: false });
      toast.success('Cập nhật giỏ hàng thành công!');
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Cập nhật giỏ hàng thất bại';
      toast.error(message);
    }
  },

  removeFromCart: async (cartItemId) => {
    set({ isLoading: true });
    try {
      await api.delete(`/api/Cart/items/${cartItemId}`);
      await get().getCart(); // Refresh cart
      toast.success('Đã xóa khỏi giỏ hàng!');
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Xóa khỏi giỏ hàng thất bại';
      toast.error(message);
    }
  },

  clearCart: async () => {
    set({ isLoading: true });
    try {
      await api.delete('/api/Cart/clear');
      set({ cart: null, isLoading: false });
      toast.success('Đã xóa giỏ hàng!');
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Xóa giỏ hàng thất bại';
      toast.error(message);
    }
  },

  getCartItemCount: () => {
    const { cart } = get();
    if (!cart?.items) return 0;
    return cart.items.reduce((total, item) => total + item.quantity, 0);
  },

  getCartTotal: () => {
    const { cart } = get();
    if (!cart?.items) return 0;
    return cart.items.reduce((total, item) => total + (item.productPrice * item.quantity), 0);
  }
}));

export default useCartStore; 