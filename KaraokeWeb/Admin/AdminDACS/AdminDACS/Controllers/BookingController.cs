using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AdminDACS.Models;
using System.Linq;
using System.Collections.Generic;

namespace AdminDACS.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDBContext _context;

        public BookingController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCustomerInfo(string roomId)
        {
            var booking = _context.bookings
                                  .Where(b => b.MaPhong == roomId)
                                  .Select(b => new
                                  {
                                      customerName = b.FullName,
                                      phone = b.Phone,
                                      email = b.Email,
                                      bookingDate = b.BookingTime
                                  })
                                  .FirstOrDefault();

            if (booking == null)
            {
                return Json(new { hasCustomer = false });
            }
            else
            {
                return Json(new { hasCustomer = true, data = booking });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitBooking(string FullName, string Phone, string Email, DateTime BookingTime, string RoomName)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.TenPhong == RoomName);
            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction("RoomList", "Room");
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
                // Cập nhật thông tin khách hàng
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
                BookingTime = BookingTime,
                FullName = FullName,
                Phone = Phone,
                Email = Email
            };

            await _context.bookings.AddAsync(booking);

            // Cập nhật trạng thái phòng đã được đặt
            room.TinhTrang = true;
            _context.Rooms.Update(room);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt phòng thành công!";

            return RedirectToAction("RoomList", "Room", new { type = room.MaLoaiPhong });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBookingConfirmed(string roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction("RoomList", "Room");
            }

            var booking = await _context.bookings.FirstOrDefaultAsync(b => b.MaPhong == roomId && room.TinhTrang == true);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Phòng chưa được đặt.";
                return RedirectToAction("RoomList", "Room");
            }

            _context.bookings.Remove(booking);

            room.TinhTrang = false;
            _context.Rooms.Update(room);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hủy đặt phòng thành công.";
            return RedirectToAction("RoomList", "Room");
        }
    }
}
