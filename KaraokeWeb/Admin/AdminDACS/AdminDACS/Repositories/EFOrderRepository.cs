using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<EFOrderRepository> _logger;

        public EFOrderRepository(ApplicationDBContext context, ILogger<EFOrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Order>> GetPaidOrdersAsync()
        {
            try
            {
                var orders = await _context.orders
                    .Where(o => o.IsPaid)
                    .Include(o => o.OrderDetails)      // Sửa tên property tại đây
                    .ThenInclude(od => od.Menu)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return orders ?? new List<Order>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn dữ liệu đơn hàng.");
                throw;
            }
        }

        public async Task<Room> GetTableByMaPhongAsync(string maPhong)
        {
            try
            {
                var room = await _context.Rooms
                    .FirstOrDefaultAsync(t => t.MaPhong == maPhong);

                if (room == null)
                {
                    throw new Exception($"Phòng với mã {maPhong} không tồn tại.");
                }

                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn thông tin phòng");
                throw;
            }
        }

        public async Task<List<Menu>> GetMenusAsync()
        {
            try
            {
                var menus = await _context.menus.ToListAsync();
                return menus ?? new List<Menu>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn danh sách món ăn");
                throw;
            }
        }

        public async Task AddOrder(Order order)
        {
            try
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order), "Đơn hàng không hợp lệ.");

                _context.orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm đơn hàng.");
                throw;
            }
        }

        public async Task<Order?> GetOrderByRoomAsync(string maPhong)
        {
            try
            {
                var order = await _context.orders
                    .Include(o => o.OrderDetails)      // Sửa tên property tại đây
                    .ThenInclude(od => od.Menu)
                    .Where(o => o.MaPhong == maPhong && !o.IsPaid)
                    .OrderByDescending(o => o.OrderDate)
                    .FirstOrDefaultAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy đơn hàng cho phòng {maPhong}");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _context.orders
                    .Include(o => o.OrderDetails)      // Sửa tên property tại đây
                    .ThenInclude(od => od.Menu)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception($"Đơn hàng với ID {orderId} không tồn tại.");

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy đơn hàng theo OrderId {orderId}");
                throw;
            }
        }

        public async Task UpdateOrder(Order order)
        {
            try
            {
                _context.orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật đơn hàng.");
                throw;
            }
        }

        public async Task<List<Order>> GetUnpaidOrdersAsync()
        {
            try
            {
                var orders = await _context.orders
                    .Where(o => !o.IsPaid)
                    .Include(o => o.OrderDetails)      // Sửa tên property tại đây
                    .ThenInclude(od => od.Menu)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return orders ?? new List<Order>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn dữ liệu đơn hàng chưa thanh toán.");
                throw;
            }
        }

        public async Task<decimal> CalculateIncomeAsync()
        {
            try
            {
                var totalIncome = await _context.orders
                    .Where(o => o.IsPaid)
                    .SumAsync(o => o.TotalAmount);

                return totalIncome;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tính toán thu nhập.");
                throw;
            }
        }
    }
}
