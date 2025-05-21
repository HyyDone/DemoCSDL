using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminDACS.Models;

public class RoomController : Controller
{
    private readonly ApplicationDBContext _context;

    public RoomController(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> RoomList()
    {
        var roomTypes = await _context.RoomsTypes
            .Include(rt => rt.Rooms)
            .ToListAsync();

        return View(roomTypes);
    }

    public async Task<IActionResult> AddRoom(Room room)
    {
        return View(room);
    }

    public async Task<IActionResult> UpdateRoom(Room room)
    {
        return View(room);
    }
}
