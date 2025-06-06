using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UserDACS.Models;

namespace UserDACS.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RoomController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult List(string type)
        {
            List<Room> rooms;

            if (string.IsNullOrEmpty(type))
            {
                rooms = _context.Rooms
                                .Include(r => r.LoaiPhong)
                                .ToList();
            }
            else
            {
                rooms = _context.Rooms
                                .Include(r => r.LoaiPhong)
                                .Where(r => r.MaLoaiPhong == type)
                                .ToList();
            }

            // Lấy booking còn hiệu lực theo phòng (BookingTime > DateTime.Now), đồng thời lấy tên phòng join với Rooms
            var currentBookings = _context.bookings
                .Where(b => b.BookingTime > DateTime.Now)
                .Join(_context.Rooms,
                      b => b.MaPhong,
                      r => r.MaPhong,
                      (b, r) => new
                      {
                          RoomName = r.TenPhong,
                          b.BookingTime,
                          b.EndTime
                      })
                .ToList();

            // Tạo dictionary phòng (Tên phòng) => thời gian đặt (chuỗi)
            var roomBookingTimes = currentBookings
                .GroupBy(b => b.RoomName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(b => $"{b.BookingTime.ToString("HH:mm")} - {b.EndTime.ToString("HH:mm")}")
                          .Aggregate((a, b) => a + "; " + b)
                );

            // Truyền dữ liệu JSON sang View
            ViewBag.RoomBookingTimesJson = JsonConvert.SerializeObject(roomBookingTimes);

            ViewData["RoomType"] = type ?? "Tất cả";

            return View(rooms);
        }

    }
}
