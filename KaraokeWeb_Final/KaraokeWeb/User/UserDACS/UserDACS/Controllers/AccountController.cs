using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDACS.Models;

public class AccountController : Controller
{
    private readonly ApplicationDBContext _context;

    public AccountController(ApplicationDBContext context)
    {
        _context = context;
    }

    // GET: /Account/Register
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                return View();
            }

            user.Password = HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
        return View(user);
    }

    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        string hashedPassword = HashPassword(password);

        var user = _context.Users
            .FirstOrDefault(u =>
                (u.Username == username || u.Email == username || u.PhoneNumber == username)
                && u.Password == hashedPassword);

        if (user != null)
        {
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            TempData["SuccessMessage"] = "Đăng nhập thành công.";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Thông tin đăng nhập không đúng.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public IActionResult Information()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        return View(user);
    }

    public IActionResult EditInformation()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null) return RedirectToAction("Login");

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return RedirectToAction("Login");

        return View(user);
    }

    [HttpPost]
    public IActionResult EditInformation(User updatedUser)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null) return RedirectToAction("Login");

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null) return RedirectToAction("Login");

        user.FullName = updatedUser.FullName;
        user.Gender = updatedUser.Gender;
        user.DateOfBirth = updatedUser.DateOfBirth;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Email = updatedUser.Email;
        user.Occupation = updatedUser.Occupation;
        user.Address = updatedUser.Address;

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
        return RedirectToAction("Information");
    }
}
