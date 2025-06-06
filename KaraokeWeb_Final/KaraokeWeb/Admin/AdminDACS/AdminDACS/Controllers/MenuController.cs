using AdminDACS.Models;
using AdminDACS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult AddMenu()
        {
            return View(new MenuCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu(MenuCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var menu = new Menu
                {
                    TenMon = vm.TenMon,
                    MoTa = vm.MoTa,
                    Gia = vm.Gia,
                    LoaiMon = vm.LoaiMon
                };

                bool result = await _menuRepository.AddMenuWithIngredientAsync(menu, vm.TenNL, vm.GiaNL, vm.SL, vm.ImageFile);

                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm món thành công!";
                    return RedirectToAction("MenuList");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm món thất bại, vui lòng thử lại.");
                }
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "ID món không hợp lệ.";
                return RedirectToAction("MenuList");
            }

            try
            {
                bool result = await _menuRepository.DeleteAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Xóa món thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy món cần xóa.";
                }
            }
            catch (Exception ex)
            {
                // Có thể log lỗi tại đây nếu cần
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa món.";
            }

            return RedirectToAction("MenuList");
        }

    }
}
