using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetAllAsync();
        Task<OrderDetail?> GetByIdAsync(int orderDetailId);
        Task AddAsync(OrderDetail orderDetail);
        Task UpdateAsync(OrderDetail orderDetail);
        Task DeleteAsync(int orderDetailId);
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<OrderDetail?> GetOrderDetailAsync(int orderId, string maMon);
        Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId);

    }
}
