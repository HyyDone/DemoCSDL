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
        public async Task<IActionResult> SubmitBooking(string FullName, string Phone, string Email, DateTime BookingTime, DateTime EndTime, string RoomName)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.TenPhong == RoomName);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra trùng giờ đặt phòng
            bool isOverlap = await _context.bookings
                .Where(b => b.MaPhong == room.MaPhong)
                .AnyAsync(b =>
                    (BookingTime < b.EndTime && EndTime > b.BookingTime)
                );

            if (isOverlap)
            {
                TempData["ErrorMessage"] = "Phòng đã được đặt trong khoảng thời gian này.";
                return RedirectToAction("List", "Room", new { type = room.MaLoaiPhong });
            }

            var customer = await _context.customers.FirstOrDefaultAsync(c => c.Phone == Phone);

            if (customer == null)
            {
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
                EndTime = EndTime,
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

        [HttpPost]
        public async Task<IActionResult> CancelBooking(string RoomName, string Phone)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.TenPhong == RoomName);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phòng.";
                return RedirectToAction("Index", "Home");
            }

            var booking = await _context.bookings
                .Where(b => b.MaPhong == room.MaPhong && b.Phone == Phone && b.BookingTime > DateTime.Now)
                .OrderByDescending(b => b.BookingTime)
                .FirstOrDefaultAsync();

            if (booking != null)
            {
                _context.bookings.Remove(booking);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Hủy đặt phòng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn đặt phòng phù hợp với số điện thoại đã nhập hoặc đơn đặt đã bắt đầu.";
            }

            return RedirectToAction("List", "Room", new { type = room.MaLoaiPhong });
        }


    }
}
