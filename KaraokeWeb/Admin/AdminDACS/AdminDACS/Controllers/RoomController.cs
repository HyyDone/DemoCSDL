using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminDACS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

public class RoomController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly IWebHostEnvironment _env;

    public RoomController(ApplicationDBContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> RoomList()
    {
        var roomTypes = await _context.RoomsTypes
            .Include(rt => rt.Rooms) 
            .ToListAsync();

        return View(roomTypes);
    }

    [HttpGet]
    public async Task<IActionResult> AddRoom()
    {
        ViewBag.LoaiPhongList = await _context.RoomsTypes
            .Select(lp => new SelectListItem
            {
                Value = lp.MaLoaiPhong,
                Text = lp.LoaiPhong
            }).ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom(Room room)
    {
        // Load lại danh sách loại phòng để trả về View nếu có lỗi
        ViewBag.LoaiPhongList = await _context.RoomsTypes
            .Select(lp => new SelectListItem
            {
                Value = lp.MaLoaiPhong,
                Text = lp.LoaiPhong
            }).ToListAsync();

        // Kiểm tra ModelState trước (bao gồm các thuộc tính bắt buộc trong Room)
        if (!ModelState.IsValid)
        {
            return View(room);
        }

        // Kiểm tra file ảnh upload
        if (room.ImageFile == null || room.ImageFile.Length == 0)
        {
            ModelState.AddModelError("ImageFile", "Vui lòng chọn ảnh.");
            return View(room);
        }

        // Tạo mã phòng tự động tăng
        var lastRoom = await _context.Rooms
            .Where(r => r.MaLoaiPhong == room.MaLoaiPhong)
            .OrderByDescending(r => r.MaPhong)
            .FirstOrDefaultAsync();

        int nextIndex = 1;

        if (lastRoom != null)
        {
            var parts = lastRoom.MaPhong.Split('_');
            if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
            {
                nextIndex = lastNumber + 1;
            }
        }

        room.MaPhong = room.MaLoaiPhong + "_" + nextIndex;

        // Lưu file ảnh vào wwwroot/ImagesRoom
        string imageFolderPath = @"C:\Users\emmot\Downloads\KaraokeWeb\User\UserDACS\UserDACS\wwwroot\ImagesRoom";

        if (!Directory.Exists(imageFolderPath))
        {
            Directory.CreateDirectory(imageFolderPath);
        }

        string fileName = $"room{nextIndex}_{room.MaLoaiPhong}{Path.GetExtension(room.ImageFile.FileName)}";
        string imagePath = Path.Combine(imageFolderPath, fileName);

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await room.ImageFile.CopyToAsync(stream);
        }

        room.ImageUrl = "/ImagesRoom/" + fileName;
        room.TinhTrang = false;

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm phòng thành công!";

        return RedirectToAction("RoomList");
    }


    [HttpGet]
    public async Task<IActionResult> UpdateRoom(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID không hợp lệ.");
        }

        var room = await _context.Rooms
            .Include(r => r.LoaiPhong)
            .FirstOrDefaultAsync(r => r.MaPhong == id);

        if (room == null)
        {
            return NotFound("Không tìm thấy phòng.");
        }

        ViewBag.RoomTypeList = await _context.RoomsTypes
            .Select(lp => new SelectListItem
            {
                Value = lp.MaLoaiPhong,
                Text = lp.LoaiPhong
            }).ToListAsync();

        return View(room);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoom(Room room)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.LoaiPhongList = await _context.RoomsTypes
                .Select(lp => new SelectListItem
                {
                    Value = lp.MaLoaiPhong,
                    Text = lp.LoaiPhong
                }).ToListAsync();

            return View(room);
        }

        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Cập nhật phòng thành công!";

        return RedirectToAction("EditRoom", new { id = room.MaPhong });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        string imageFolderPath = @"C:\Users\emmot\Downloads\KaraokeWeb\User\UserDACS\UserDACS\wwwroot\ImagesRoom";

        if (!string.IsNullOrEmpty(room.ImageUrl))
        {
            string fileName = Path.GetFileName(room.ImageUrl);

            string imagePath = Path.Combine(imageFolderPath, fileName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Xóa phòng thành công!";

        return RedirectToAction("RoomList");
    }
}
