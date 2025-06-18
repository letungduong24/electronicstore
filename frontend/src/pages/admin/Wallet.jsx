import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { CreditCard, Search, Plus, Minus, DollarSign, Users } from 'lucide-react';
import { toast } from 'sonner';
import api from '../../services/api';

const Wallet = () => {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [showAddBalanceModal, setShowAddBalanceModal] = useState(false);
  const [showDeductBalanceModal, setShowDeductBalanceModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);

  const {
    register: registerAdd,
    handleSubmit: handleAddSubmit,
    reset: resetAdd,
    formState: { errors: addErrors },
  } = useForm();

  const {
    register: registerDeduct,
    handleSubmit: handleDeductSubmit,
    reset: resetDeduct,
    formState: { errors: deductErrors },
  } = useForm();

  useEffect(() => {
    fetchWallets();
  }, []);

  const fetchWallets = async () => {
    try {
      const response = await api.get('/api/User/all-wallets');
      setUsers(response.data);
    } catch (error) {
      console.error('Error fetching wallets:', error);
      toast.error('Không thể tải danh sách ví người dùng');
    } finally {
      setIsLoading(false);
    }
  };

  const filteredUsers = users.filter(user => {
    const matchesSearch = 
      user.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email?.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesSearch;
  });

  const handleAddBalance = async (data) => {
    try {
      await api.post('/api/Wallet/add-balance', {
        userId: selectedUser.userId || selectedUser.id,
        amount: parseFloat(data.amount),
        reason: data.reason
      });
      toast.success('Nạp tiền thành công');
      setShowAddBalanceModal(false);
      setSelectedUser(null);
      resetAdd();
      fetchWallets();
    } catch (error) {
      const message = error.response?.data?.message || 'Nạp tiền thất bại';
      toast.error(message);
    }
  };

  const handleDeductBalance = async (data) => {
    try {
      await api.post('/api/Wallet/deduct-balance', {
        userId: selectedUser.userId || selectedUser.id,
        amount: parseFloat(data.amount),
        reason: data.reason
      });
      toast.success('Trừ tiền thành công');
      setShowDeductBalanceModal(false);
      setSelectedUser(null);
      resetDeduct();
      fetchWallets();
    } catch (error) {
      const message = error.response?.data?.message || 'Trừ tiền thất bại';
      toast.error(message);
    }
  };

  const openAddBalanceModal = (user) => {
    setSelectedUser(user);
    setShowAddBalanceModal(true);
    resetAdd();
  };

  const openDeductBalanceModal = (user) => {
    setSelectedUser(user);
    setShowDeductBalanceModal(true);
    resetDeduct();
  };

  const getTotalBalance = () => {
    return users.reduce((sum, user) => sum + (user.balance || 0), 0);
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <h1 className="text-3xl font-bold text-gray-900">Quản lý ví</h1>
        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="animate-pulse space-y-4">
            {[...Array(5)].map((_, index) => (
              <div key={index} className="flex items-center space-x-4">
                <div className="bg-gray-300 h-10 w-10 rounded-full"></div>
                <div className="flex-1 space-y-2">
                  <div className="bg-gray-300 h-4 rounded w-1/4"></div>
                  <div className="bg-gray-300 h-3 rounded w-1/2"></div>
                </div>
                <div className="bg-gray-300 h-8 w-20 rounded"></div>
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Quản lý ví</h1>
        <div className="flex items-center space-x-2">
          <CreditCard className="h-6 w-6 text-blue-600" />
          <span className="text-gray-600">Tổng cộng: {filteredUsers.length} người dùng</span>
        </div>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="flex items-center">
            <div className="p-2 bg-blue-100 rounded-lg">
              <Users className="h-6 w-6 text-blue-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Tổng người dùng</p>
              <p className="text-2xl font-bold text-gray-900">{users.length}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="flex items-center">
            <div className="p-2 bg-green-100 rounded-lg">
              <DollarSign className="h-6 w-6 text-green-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Tổng số dư</p>
              <p className="text-2xl font-bold text-gray-900">{getTotalBalance().toLocaleString('vi-VN')} VND</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-md p-6">
          <div className="flex items-center">
            <div className="p-2 bg-yellow-100 rounded-lg">
              <CreditCard className="h-6 w-6 text-yellow-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Số dư trung bình</p>
              <p className="text-2xl font-bold text-gray-900">
                {users.length > 0 ? (getTotalBalance() / users.length).toLocaleString('vi-VN') : '0'} VND
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Search */}
      <div className="bg-white rounded-lg shadow-md p-6">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
          <input
            type="text"
            placeholder="Tìm kiếm người dùng..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
      </div>

      {/* Users Table */}
      <div className="bg-white rounded-lg shadow-md">
        <div className="p-6 border-b">
          <h2 className="text-lg font-semibold text-gray-900">Danh sách ví người dùng</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  ID
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Người dùng
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Email
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Số dư
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Thao tác
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {filteredUsers.map((user) => (
                <tr key={user.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-mono">{user.userId || user.id}</td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center">
                      <div className="">
                        <div className="text-sm font-medium text-gray-900">
                          {user.name || 'N/A'}
                        </div>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {user.email}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-lg font-semibold text-gray-900">
                      {user.balance?.toLocaleString('vi-VN') || '0'} VND
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                    <div className="flex space-x-2">
                      <button
                        onClick={() => openAddBalanceModal(user)}
                        className="text-green-600 hover:text-green-900 flex items-center"
                      >
                        <Plus className="h-4 w-4 mr-1" />
                        Nạp tiền
                      </button>
                      <button
                        onClick={() => openDeductBalanceModal(user)}
                        className="text-red-600 hover:text-red-900 flex items-center"
                      >
                        <Minus className="h-4 w-4 mr-1" />
                        Trừ tiền
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Add Balance Modal */}
      {showAddBalanceModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-full max-w-md shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                Nạp tiền cho {selectedUser?.name || selectedUser?.email}
              </h3>
              <form onSubmit={handleAddSubmit(handleAddBalance)} className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Số tiền
                  </label>
                  <input
                    {...registerAdd('amount', {
                      required: 'Số tiền là bắt buộc',
                      min: { value: 0.01, message: 'Số tiền phải lớn hơn 0' }
                    })}
                    type="number"
                    step="0.01"
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="0.00"
                  />
                  {addErrors.amount && (
                    <p className="text-red-500 text-sm mt-1">{addErrors.amount.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Lý do
                  </label>
                  <textarea
                    {...registerAdd('reason')}
                    rows={3}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Nhập lý do nạp tiền"
                  />
                </div>

                <div className="flex space-x-3">
                  <button
                    type="submit"
                    className="flex-1 bg-green-600 text-white py-2 px-4 rounded-md hover:bg-green-700 transition-colors"
                  >
                    Nạp tiền
                  </button>
                  <button
                    type="button"
                    onClick={() => {
                      setShowAddBalanceModal(false);
                      setSelectedUser(null);
                      resetAdd();
                    }}
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

      {/* Deduct Balance Modal */}
      {showDeductBalanceModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-full max-w-md shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                Trừ tiền cho {selectedUser?.name || selectedUser?.email}
              </h3>
              <form onSubmit={handleDeductSubmit(handleDeductBalance)} className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Số tiền
                  </label>
                  <input
                    {...registerDeduct('amount', {
                      required: 'Số tiền là bắt buộc',
                      min: { value: 0.01, message: 'Số tiền phải lớn hơn 0' }
                    })}
                    type="number"
                    step="0.01"
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="0.00"
                  />
                  {deductErrors.amount && (
                    <p className="text-red-500 text-sm mt-1">{deductErrors.amount.message}</p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Lý do
                  </label>
                  <textarea
                    {...registerDeduct('reason')}
                    rows={3}
                    className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
                    placeholder="Nhập lý do trừ tiền"
                  />
                </div>

                <div className="flex space-x-3">
                  <button
                    type="submit"
                    className="flex-1 bg-red-600 text-white py-2 px-4 rounded-md hover:bg-red-700 transition-colors"
                  >
                    Trừ tiền
                  </button>
                  <button
                    type="button"
                    onClick={() => {
                      setShowDeductBalanceModal(false);
                      setSelectedUser(null);
                      resetDeduct();
                    }}
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

export default Wallet; 