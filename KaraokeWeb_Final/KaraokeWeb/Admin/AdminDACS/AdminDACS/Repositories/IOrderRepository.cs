using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IOrderRepository
    {
        Task<Room> GetTableByMaPhongAsync(string maPhong);
        Task AddOrder(Order order);
        Task<List<Order>> GetPaidOrdersAsync();
        Task<List<Order>> GetUnpaidOrdersAsync();
        Task<List<Menu>> GetMenusAsync();
        Task<Order?> GetOrderByRoomAsync(string maPhong);
        Task UpdateOrder(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<decimal?> CalculateIncomeAsync();
    }
}
