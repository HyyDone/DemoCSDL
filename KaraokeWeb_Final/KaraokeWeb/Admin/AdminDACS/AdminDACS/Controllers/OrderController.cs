using System.Linq;
using System.Threading.Tasks;
using AdminDACS.Models;
using AdminDACS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDACS.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ApplicationDBContext _context;

        public OrderController(IOrderRepository orderRepository, IMenuRepository menuRepository, IOrderDetailRepository orderDetailRepository, IRoomRepository roomRepository, ApplicationDBContext context)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _orderDetailRepository = orderDetailRepository;
            _roomRepository = roomRepository;
            _context = context;
        }

        public async Task<IActionResult> GetPaidOrders()
        {
            var orders = await _orderRepository.GetPaidOrdersAsync();
            if (orders == null || !orders.Any())
            {
                return Content("Không có hóa đơn nào.");
            }
            return View(orders);
        }

        [HttpGet("order/add/{roomId}")]
        public async Task<IActionResult> OrderPage(string roomId)
        {
            var room = await _orderRepository.GetTableByMaPhongAsync(roomId);
            var menuItems = (await _menuRepository.ShowAsync()).ToList();
            var order = await _orderRepository.GetOrderByRoomAsync(roomId);
            List<OrderDetail> orderDetails = new();

            if (order == null)
            {
                order = new Order
                {
                    MaPhong = roomId,
                    OrderDate = DateTime.Now,
                    IsPaid = false
                };

                await _orderRepository.AddOrder(order);
            }

            if (order != null)
            {
                orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(order.OrderId);

                foreach (var od in orderDetails)
                {
                    od.Menu = menuItems.FirstOrDefault(m => m.MaMon == od.MaMon);
                }
            }

            if (room == null || menuItems == null || !menuItems.Any())
            {
                ViewBag.ErrorMessage = "Bàn hoặc món ăn không hợp lệ!";
                return View();
            }

            var doAnList = menuItems.Where(m => m.LoaiMon == "Food").ToList();
            var doUongList = menuItems.Where(m => m.LoaiMon == "Beverage").ToList();

            var model = new OrderViewModel
            {
                MaPhong = roomId,
                DanhSachMon = menuItems,
                DoAnList = doAnList,
                DoUongList = doUongList,
                OrderDetails = orderDetails
            };
            return View(model);
        }

        [HttpPost("order/add/{roomId}")]
        public async Task<IActionResult> OrderPage(OrderViewModel model)
        {
            var menuItems = (await _menuRepository.ShowAsync()).ToList();

            var doAnList = menuItems.Where(m => m.LoaiMon == "Food").ToList();
            var doUongList = menuItems.Where(m => m.LoaiMon == "Beverage").ToList();

            model.DanhSachMon = menuItems;
            model.DoAnList = doAnList;
            model.DoUongList = doUongList;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Dữ liệu không hợp lệ!";
                return View("OrderPage", model);
            }

            var order = await _orderRepository.GetOrderByRoomAsync(model.MaPhong);
            if (order == null)
            {
                order = new Order
                {
                    MaPhong = model.MaPhong,
                    OrderDate = DateTime.Now,
                    IsPaid = false,
                    TotalFoodAmount = 0
                };

                await _orderRepository.AddOrder(order);
                order = await _orderRepository.GetOrderByRoomAsync(model.MaPhong);
            }

            foreach (var menuItem in menuItems)
            {
                int quantity = model.Quantities != null && model.Quantities.ContainsKey(menuItem.MaMon)
                    ? model.Quantities[menuItem.MaMon]
                    : 0;

                if (quantity > 0)
                {
                    var existingDetail = await _orderDetailRepository.GetOrderDetailAsync(order.OrderId, menuItem.MaMon);

                    if (existingDetail != null)
                    {
                        existingDetail.Quantity += quantity;
                        await _orderDetailRepository.UpdateAsync(existingDetail);
                    }
                    else
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            MaMon = menuItem.MaMon,
                            Quantity = quantity,
                            UnitPrice = menuItem.Gia
                        };
                        await _orderDetailRepository.AddAsync(orderDetail);
                    }
                }
            }

            var updatedOrderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(order.OrderId);
            var totalFoodAmount = updatedOrderDetails
                .Where(od => menuItems.FirstOrDefault(m => m.MaMon == od.MaMon)?.LoaiMon == "Food")
                .Sum(od => od.Quantity * od.UnitPrice);

            order.TotalFoodAmount = totalFoodAmount;
            await _orderRepository.UpdateOrder(order);
            foreach (var od in updatedOrderDetails)
            {
                od.Menu = menuItems.FirstOrDefault(m => m.MaMon == od.MaMon);
            }

            var resultModel = new OrderViewModel
            {
                MaPhong = model.MaPhong,
                DanhSachMon = menuItems,
                DoAnList = doAnList,
                DoUongList = doUongList,
                OrderDetails = updatedOrderDetails
            };

            return View("OrderPage", resultModel);
        }




        public async Task<IActionResult> PayOrder()
        {
            var orders = await _orderRepository.GetUnpaidOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return Content("Không có hóa đơn chưa thanh toán.");
            }

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("ID không hợp lệ.");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
                return NotFound("Không tìm thấy đơn hàng.");

            decimal totalFood = order.OrderDetails?.Sum(d => d.Quantity * d.UnitPrice) ?? 0m;

            var now = DateTime.Now;
            var durationHours = Math.Max(1, (int)Math.Round((now - order.OrderDate).TotalHours));

            decimal roomPrice = order.Room?.Gia ?? 0m;
            decimal totalRoom = durationHours * roomPrice;

            order.TotalFoodAmount = totalFood;
            order.TotalRoomAmount = totalRoom;
            order.TotalAmount = totalFood + totalRoom;
            order.IsPaid = true;

            foreach (var detail in order.OrderDetails)
            {
                var warehouseItem = await _context.Warehouses
                                         .FirstOrDefaultAsync(w => w.MaMon == detail.Menu.MaMon);

                if (warehouseItem != null)
                {
                    warehouseItem.SL -= detail.Quantity;
                    _context.Warehouses.Update(warehouseItem);
                }
            }

            await _orderRepository.UpdateOrder(order);
            return RedirectToAction("GetPaidOrders");
        }


        public async Task<IActionResult> CalculateIncome()
        {
            try
            {
                var paidOrders = await _orderRepository.GetPaidOrdersAsync();

                var orders = paidOrders.Select(o => new OrderIncomeViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    IsPaid = o.IsPaid
                }).ToList();

                var totalIncome = orders.Sum(o => o.TotalAmount ?? 0);

                var model = new IncomeViewModel
                {
                    Orders = orders,
                    TotalIncome = totalIncome
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", model: ex.Message);
            }
        }


        [HttpPost("order/remove")]
        public async Task<IActionResult> RemoveFromCart(int orderDetailId, string maPhong)
        {
            var detail = await _orderDetailRepository.GetOrderDetailByIdAsync(orderDetailId);
            if (detail != null)
            {
                await _orderDetailRepository.DeleteAsync(orderDetailId);

                var order = await _orderRepository.GetOrderByIdAsync(detail.OrderId);
                var updatedDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(detail.OrderId);
                order.TotalAmount = updatedDetails.Sum(d => d.Quantity * d.UnitPrice);
                await _orderRepository.UpdateOrder(order);
            }

            // sửa tên action cho đúng với route bạn đang dùng
            return RedirectToAction("OrderPage", new { roomId = maPhong });
        }


        [HttpPost("order/place")]
        public async Task<IActionResult> PlaceOrder(string maPhong)
        {
            var order = await _orderRepository.GetOrderByRoomAsync(maPhong);
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng cho bàn này.");
            }

            var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(order.OrderId);
            if (orderDetails == null || !orderDetails.Any())
            {
                return Content("Giỏ hàng trống, không thể đặt món.");
            }

            order.OrderDate = DateTime.Now;
            order.TotalAmount = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            await _orderRepository.UpdateOrder(order);

            TempData["SuccessMessage"] = "Đã xác nhận đặt món thành công.";
            return RedirectToAction("OrderPage", new { roomId = maPhong });
        }

        [HttpGet("order/payorderpage/{orderId}")]
        public async Task<IActionResult> PayOrderPage(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var od in orderDetails)
            {
                od.Menu = await _menuRepository.GetByIdAsync(od.MaMon);
            }

            var model = new OrderViewModel
            {
                MaPhong = order.MaPhong,
                OrderDetails = orderDetails,
                TongTien = order.TotalAmount ?? 0m,
            };

            ViewBag.OrderId = orderId;
            return View("PayOrderPage", model);
        }

    }
}
