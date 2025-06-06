using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserDACS.Models;

namespace UserDACS.Controllers
{
    [Route("Menu")]
    public class MenuController : Controller
    {
        private readonly ApplicationDBContext _context;

        public MenuController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: /Menu/ShowMenu
        [HttpGet("ShowMenu")]
        public async Task<IActionResult> ShowMenu()
        {
            var menus = await _context.menus.ToListAsync();
            return View(menus);
        }

        // GET: /Menu/SearchMenu?term=xxx
        [HttpGet("SearchMenu")]
        public async Task<IActionResult> SearchMenu(string term)
        {
            IQueryable<Menu> query = _context.menus;

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(m => m.TenMon.Contains(term));
            }

            var result = await query
                .Select(m => new
                {
                    m.MaMon,
                    m.TenMon,
                    m.MoTa,
                    m.Gia,
                    m.ImageUrl
                })
                .ToListAsync();

            return Json(result);
        }

        // GET: /Menu/InforFood/{maMon}
        [HttpGet("InforFood/{maMon}")]
        public async Task<IActionResult> InforFood(string maMon)
        {
            var menu = await _context.menus.FindAsync(maMon);
            if (menu == null)
                return NotFound();

            return View(menu);
        }

        // POST: /Menu/AddItemToOrder
        [HttpPost("AddItemToOrder")]
        public async Task<IActionResult> AddItemToOrder(string maPhong, string maMon)
        {
            var menuItem = await _context.menus.FirstOrDefaultAsync(m => m.MaMon == maMon);
            if (menuItem == null) return NotFound();

            var existing = await _context.TempOrderItems
                .FirstOrDefaultAsync(x => x.MaPhong == maPhong && x.MaMon == maMon);

            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                var tempItem = new TempOrderItem
                {
                    MaPhong = maPhong,
                    MaMon = maMon,
                    Quantity = 1,
                    UnitPrice = menuItem.Gia
                };
                await _context.TempOrderItems.AddAsync(tempItem);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrder(string maPhong)
        {
            var tempItems = await _context.TempOrderItems
                .Where(t => t.MaPhong == maPhong)
                .ToListAsync();

            if (!tempItems.Any()) return BadRequest("Không có món nào được đặt.");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                MaPhong = maPhong,
                IsPaid = false,
                TotalFoodAmount = tempItems.Sum(t => t.Quantity * t.UnitPrice),
                TotalAmount = tempItems.Sum(t => t.Quantity * t.UnitPrice),
                OrderDetails = tempItems.Select(t => new OrderDetail
                {
                    MaMon = t.MaMon,
                    Quantity = t.Quantity,
                    UnitPrice = t.UnitPrice
                }).ToList()
            };

            await _context.orders.AddAsync(order);
            _context.TempOrderItems.RemoveRange(tempItems);

            await _context.SaveChangesAsync();
            return Ok();
        }


        // GET: /Menu/GetBookedRooms?phone=...
        [HttpGet("GetBookedRooms")]
        public IActionResult GetBookedRooms(string phone)
        {
            var rooms = (from b in _context.bookings
                         join c in _context.customers on b.MaKH equals c.MaKH
                         join r in _context.Rooms on b.MaPhong equals r.MaPhong
                         where c.Phone == phone
                         select new
                         {
                             maPhong = r.MaPhong,
                             tenPhong = r.TenPhong
                         })
                        .Distinct()
                        .ToList();

            return Json(rooms);
        }


        // GET: /Menu/GetTempOrderItems?maPhong=...
        [HttpGet("GetTempOrderItems")]
        public IActionResult GetTempOrderItems(string maPhong)
        {
            var items = _context.TempOrderItems
                .Where(x => x.MaPhong == maPhong)
                .Select(x => new
                {
                    tenMon = x.Menu.TenMon,
                    quantity = x.Quantity,
                    unitPrice = x.UnitPrice
                })
                .ToList();

            return Json(items);
        }
    }
}
