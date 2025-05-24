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

        public OrderController(IOrderRepository orderRepository, IMenuRepository menuRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _orderDetailRepository = orderDetailRepository;
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
                    IsPaid = false
                };

                await _orderRepository.AddOrder(order);
                order = await _orderRepository.GetOrderByRoomAsync(model.MaPhong);
            }

            foreach (var menuItem in menuItems)
            {
                int quantity = 0;
                if (model.Quantities != null && model.Quantities.TryGetValue(menuItem.MaMon, out int q))
                {
                    quantity = q;
                }

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
            order.TotalAmount = updatedOrderDetails.Sum(od => od.Quantity * od.UnitPrice);
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

        [HttpGet("Order/Pay/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Không tìm thấy ID này.");
            }

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.IsPaid = true;
            await _orderRepository.UpdateOrder(order);

            return RedirectToAction("GetPaidOrders");
        }

        public async Task<IActionResult> CalculateIncome()
        {
            try
            {
                var orders = await _orderRepository.GetPaidOrdersAsync() ?? new List<Order>();
                var totalIncome = orders.Sum(o => o.TotalAmount);

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

    }
}
