using Microsoft.AspNetCore.Mvc;

namespace AdminDACS.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult RoomList()
        {
            return View();
        }
    }
}
