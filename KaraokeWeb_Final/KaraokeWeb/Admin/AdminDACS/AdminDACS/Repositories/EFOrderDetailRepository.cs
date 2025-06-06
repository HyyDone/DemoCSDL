using Microsoft.EntityFrameworkCore;
using AdminDACS.Models;  // Sửa lại theo đúng nơi chứa model OrderDetail

namespace AdminDACS.Repositories
{
    public class EFOrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDBContext _context;

        public EFOrderDetailRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetAllAsync()
        {
            return await _context.ordersDetail
                                 .Include(od => od.Menu)
                                 .ToListAsync();
        }

        public async Task<OrderDetail?> GetByIdAsync(int orderDetailId)
        {
            return await _context.ordersDetail
                                 .Include(od => od.Menu)
                                 .FirstOrDefaultAsync(od => od.OrderDetailId == orderDetailId);
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            try
            {
                await _context.ordersDetail.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm OrderDetail: " + ex.Message);
            }
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.ordersDetail.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int orderDetailId)
        {
            var orderDetail = await _context.ordersDetail.FindAsync(orderDetailId);
            if (orderDetail != null)
            {
                _context.ordersDetail.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.ordersDetail
                                 .Include(od => od.Menu)
                                 .Where(od => od.OrderId == orderId)
                                 .ToListAsync();
        }

        public async Task<OrderDetail?> GetOrderDetailAsync(int orderId, string maMon)
        {
            return await _context.ordersDetail
                                 .Include(od => od.Menu)
                                 .FirstOrDefaultAsync(od => od.OrderId == orderId && od.MaMon == maMon);
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            var orderDetail = await _context.ordersDetail
                                            .Include(od => od.Menu)
                                            .FirstOrDefaultAsync(od => od.OrderDetailId == orderDetailId);

            if (orderDetail == null)
            {
                throw new Exception($"OrderDetail với ID {orderDetailId} không tồn tại.");
            }
            return orderDetail;
        }
    }
}
