using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdminDACS.Repositories;
using AdminDACS.Models;
using static AdminDACS.Models.WarehouseMenuViewModel;

namespace AdminDACS.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly IMenuRepository _menuRepo;

        public WarehouseController(IWarehouseRepository warehouseRepo, IMenuRepository menuRepo)
        {
            _warehouseRepo = warehouseRepo;
            _menuRepo = menuRepo;
        }

        public async Task<IActionResult> WarehouseList()
        {
            var warehouses = await _warehouseRepo.GetAllAsync();
            var menus = await _menuRepo.GetAllAsync();

            var viewModel = warehouses.Join(
                    menus,
                    w => w.MaMon,   // khóa ngoại trong Warehouse
                    m => m.MaMon,   // khóa chính trong Menu
                    (w, m) => new WarehouseMenuViewModel.WarehouseWithLoaiViewModel
                    {
                        TenNL = w.TenNL,
                        Gia = w.Gia,
                        SoLuong = w.SL,
                        LoaiMon = m.LoaiMon
                    })
                .ToList();

            return View(viewModel);
        }
    }
}
