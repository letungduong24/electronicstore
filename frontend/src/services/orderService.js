import api from './api';

export const fetchAdminOrders = async () => {
  const response = await api.get('/api/Order/all');
  return response.data;
};

export const fetchRecentOrders = async (limit = 5) => {
  const response = await api.get('/api/Order/all');
  const orders = response.data;
  return orders.slice(0, limit);
}; 