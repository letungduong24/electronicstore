import { create } from 'zustand';
import { toast } from 'sonner';
import api from '../services/api';

const useAuthStore = create((set, get) => ({
  user: null,
  isAuthenticated: false,
  isLoading: false,

  login: async (credentials) => {
    set({ isLoading: true });
    try {
      const response = await api.post('/api/User/login', credentials);
      const { user } = response.data;
      set({ user, isAuthenticated: true, isLoading: false });
      toast.success('Đăng nhập thành công!');
      return { success: true };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Đăng nhập thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  register: async (userData) => {
    set({ isLoading: true });
    try {
      const response = await api.post('/api/User/register', userData);
      const { user } = response.data;
      set({ user, isAuthenticated: true, isLoading: false });
      toast.success('Đăng ký thành công!');
      return { success: true };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Đăng ký thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  logout: async () => {
    try {
      await api.post('/api/User/logout');
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      set({ user: null, isAuthenticated: false, isLoading: false });
      toast.success('Đăng xuất thành công!');
    }
  },

  getCurrentUser: async () => {
    try {
      const response = await api.get('/api/User/me');
      const user = response.data;
      set({ user, isAuthenticated: true });
    } catch (error) {
      set({ user: null, isAuthenticated: false });
    }
  },

  updateUser: async (userData) => {
    set({ isLoading: true });
    try {
      const response = await api.put('/api/User/update', userData);
      const { user } = response.data;
      set({ user, isLoading: false });
      toast.success('Cập nhật thông tin thành công!');
      return { success: true };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Cập nhật thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  changePassword: async (passwordData) => {
    set({ isLoading: true });
    try {
      await api.post('/api/User/change-password', passwordData);
      set({ isLoading: false });
      toast.success('Đổi mật khẩu thành công!');
      return { success: true };
    } catch (error) {
      set({ isLoading: false });
      const message = error.response?.data?.message || 'Đổi mật khẩu thất bại';
      toast.error(message);
      return { success: false, message };
    }
  },

  isAdmin: () => {
    const { user } = get();
    return user?.roles?.includes('Admin') || false;
  },

  setUser: (user) => set({ user, isAuthenticated: !!user })
}));

export default useAuthStore; 