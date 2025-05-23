using AdminDACS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdminDACS.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IActionResult> MenuList()
        {
            var menuItems = await _menuRepository.ShowAsync();
            return View(menuItems); 
        }
    }
}
