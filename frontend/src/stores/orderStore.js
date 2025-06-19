import { create } from 'zustand';
import { fetchAdminOrders, fetchRecentOrders } from '../services/orderService';

const useOrderStore = create((set) => ({
  orders: [],
  recentOrders: [],
  isLoading: false,
  error: null,
  fetchOrders: async () => {
    set({ isLoading: true, error: null });
    try {
      const data = await fetchAdminOrders();
      set({ orders: data, isLoading: false });
    } catch (error) {
      set({ error: error.message || 'Lỗi khi tải đơn hàng', isLoading: false });
    }
  },
  fetchRecentOrders: async (limit = 5) => {
    set({ isLoading: true, error: null });
    try {
      const data = await fetchRecentOrders(limit);
      set({ recentOrders: data, isLoading: false });
    } catch (error) {
      set({ error: error.message || 'Lỗi khi tải đơn hàng', isLoading: false });
    }
  },
}));

export default useOrderStore; 