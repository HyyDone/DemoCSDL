using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserDACS.Models;
using System.Linq;

namespace UserDACS.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDBContext _context;

        public BookingController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitBooking(string FullName, string Phone, string Email, DateTime BookingTime, string RoomName)
        {
            // Tìm phòng theo tên
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.TenPhong == RoomName);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            var customer = await _context.customers.FirstOrDefaultAsync(c => c.Phone == Phone);

            if (customer == null)
            {
                // Tạo mã khách hàng tự động: KH + số thứ tự 2 chữ số
                var maxMaKH = await _context.customers
                    .Where(c => c.MaKH.StartsWith("KH"))
                    .OrderByDescending(c => c.MaKH)
                    .Select(c => c.MaKH)
                    .FirstOrDefaultAsync();

                int newNumber = 1;
                if (!string.IsNullOrEmpty(maxMaKH) && maxMaKH.Length > 2)
                {
                    var numberPart = maxMaKH.Substring(2);
                    if (int.TryParse(numberPart, out int currentNumber))
                    {
                        newNumber = currentNumber + 1;
                    }
                }
                string newMaKH = "KH" + newNumber.ToString("D2");

                customer = new Customer
                {
                    MaKH = newMaKH,
                    FullName = FullName,
                    Phone = Phone,
                    Email = Email
                };
                await _context.customers.AddAsync(customer);
            }
            else
            {
                customer.FullName = FullName;
                customer.Email = Email;
                _context.customers.Update(customer);
            }

            // Tạo mã booking tự động: HD + số thứ tự 2 chữ số
            var maxMaBooking = await _context.bookings
                .Where(b => b.MaBooking.StartsWith("HD"))
                .OrderByDescending(b => b.MaBooking)
                .Select(b => b.MaBooking)
                .FirstOrDefaultAsync();

            int newBookingNumber = 1;
            if (!string.IsNullOrEmpty(maxMaBooking) && maxMaBooking.Length > 2)
            {
                var numberPart = maxMaBooking.Substring(2);
                if (int.TryParse(numberPart, out int currentNumber))
                {
                    newBookingNumber = currentNumber + 1;
                }
            }
            string newMaBooking = "HD" + newBookingNumber.ToString("D2");

            var booking = new Booking
            {
                MaBooking = newMaBooking,
                MaPhong = room.MaPhong,
                MaKH = customer.MaKH,
                BookingTime = BookingTime,
                FullName = FullName,
                Phone = Phone,
                Email = Email
            };

            await _context.bookings.AddAsync(booking);

            room.TinhTrang = true;
            _context.Rooms.Update(room);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Đặt phòng thành công!";

            return RedirectToAction("List", "Room", new { type = room.MaLoaiPhong });
        }
    }
}
