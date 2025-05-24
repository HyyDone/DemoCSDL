using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDACS.Models;
using System.Collections.Generic;
using System.Linq;

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

            ViewData["RoomType"] = type ?? "Tất cả";
            return View(rooms);
        }
    }
}
