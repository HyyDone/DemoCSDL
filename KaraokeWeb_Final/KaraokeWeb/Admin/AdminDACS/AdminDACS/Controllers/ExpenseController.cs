using AdminDACS.Models;
using AdminDACS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDACS.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ApplicationDBContext _context;

        public ExpenseController(IExpenseRepository expenseRepository, ApplicationDBContext context)
        {
            _expenseRepository = expenseRepository;
            _context = context;
        }

        public async Task<IActionResult> ShowExpense()
        {
            var expenses = await _expenseRepository.ShowExpenseAsync();
            if (expenses == null || !expenses.Any())
            {
                return View("Error", new { message = "Không có dữ liệu chi phí." });
            }
            return View(expenses);
        }

        public async Task<IActionResult> ShowStaff()
        {
            var staffList = await _expenseRepository.ShowStaffAsync();
            if (staffList == null || !staffList.Any())
            {
                return View("Error", new { message = "Không có dữ liệu nhân viên." });
            }
            return View(staffList);
        }

        public async Task<IActionResult> ShowElecBill()
        {
            var elecBills = await _expenseRepository.ShowElectricityBillAsync();
            if (elecBills == null || !elecBills.Any())
            {
                return View("Error", new { message = "Không có dữ liệu hóa đơn điện." });
            }
            return View(elecBills);
        }

        public async Task<IActionResult> ShowWaterBill()
        {
            var waterBills = await _expenseRepository.ShowWaterBillAsync();
            if (waterBills == null || !waterBills.Any())
            {
                return View("Error", new { message = "Không có dữ liệu hóa đơn nước." });
            }
            return View(waterBills);
        }

        public async Task<IActionResult> ShowMaterialInput()
        {
            var list = await _expenseRepository.ShowMaterialInputAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> EditWage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Thiếu mã nhân viên.");
            }

            var staffList = await _expenseRepository.ShowStaffAsync();
            var selectedStaff = staffList.FirstOrDefault(s => s.MaNhanVien == id);

            if (selectedStaff == null)
            {
                return NotFound("Không tìm thấy nhân viên.");
            }

            return View(selectedStaff); // Trả về View sửa lương chỉ 1 Staff
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWage(Staff model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var staffList = await _expenseRepository.ShowStaffAsync();
            var existingStaff = staffList.FirstOrDefault(s => s.MaNhanVien == model.MaNhanVien);

            if (existingStaff == null)
            {
                return NotFound("Không tìm thấy nhân viên để cập nhật.");
            }

            existingStaff.Luong = model.Luong;
            await _expenseRepository.UpdateStaffAsync(existingStaff);

            return RedirectToAction("ShowStaff");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStaff(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Thiếu mã nhân viên.");
            }

            var staff = await _expenseRepository.ShowStaffAsync();
            var staffToDelete = staff.FirstOrDefault(s => s.MaNhanVien == id);

            if (staffToDelete == null)
            {
                return NotFound("Không tìm thấy nhân viên để xóa.");
            }

            // Xóa nhân viên
            await _expenseRepository.DeleteStaffAsync(id);

            // Quay lại trang danh sách nhân viên
            return RedirectToAction("ShowStaff");
        }

        // GET: Thêm nhân viên
        public IActionResult AddStaff()
        {
            return View();
        }

        // POST: Xử lý thêm nhân viên mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                // Thêm nhân viên mới vào cơ sở dữ liệu
                await _expenseRepository.AddStaffAsync(staff);

                // Chuyển hướng về trang danh sách nhân viên sau khi thêm thành công
                return RedirectToAction("ShowStaff");
            }
            return View(staff);
        }

        [HttpGet]
        public IActionResult AddBillElec()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddBillWater()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillElec(ElectricityBill electricityBill)
        {
            if (electricityBill.Ngay.Month > DateTime.Now.Month && electricityBill.Ngay.Year == DateTime.Now.Year)
            {
                ModelState.AddModelError("Ngay", "Tháng trong ngày hóa đơn không được lớn hơn tháng hiện tại.");
            }
            if (ModelState.IsValid)
            {
                await _expenseRepository.AddElecBillAsync(electricityBill);

                return RedirectToAction("ShowElecBill");
            }
            return View(electricityBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillWater(WaterBill waterBill)
        {
            if (waterBill.Ngay.Month > DateTime.Now.Month && waterBill.Ngay.Year == DateTime.Now.Year)
            {
                ModelState.AddModelError("Ngay", "Tháng trong ngày hóa đơn không được lớn hơn tháng hiện tại.");
            }
            if (ModelState.IsValid)
            {
                // Thêm hóa đơn điện mới vào cơ sở dữ liệu
                await _expenseRepository.AddWaterBillAsync(waterBill);

                // Chuyển hướng về trang danh sách hóa đơn điện sau khi thêm thành công
                return RedirectToAction("ShowWaterBill");
            }
            return View(waterBill);
        }

        // POST: Xóa hóa đơn điện
        [HttpPost]
        public async Task<IActionResult> DeleteElecBill(string id)
        {
            var electricityBill = await _expenseRepository.ShowElectricityBillAsync();
            var billToDelete = electricityBill.FirstOrDefault(b => b.MaBillElec == id);

            if (billToDelete == null)
            {
                return NotFound("Không tìm thấy hóa đơn điện để xóa.");
            }

            // Xóa hóa đơn điện
            await _expenseRepository.DeleteElecBillAsync(billToDelete);

            // Quay lại trang danh sách hóa đơn điện
            return RedirectToAction("ShowElecBill");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteWaterBill(string id)
        {
            var waterBill = await _expenseRepository.ShowWaterBillAsync();
            var billToDelete = waterBill.FirstOrDefault(b => b.MaBillWater == id);

            if (billToDelete == null)
            {
                return NotFound("Không tìm thấy hóa đơn điện để xóa.");
            }

            // Xóa hóa đơn điện
            await _expenseRepository.DeleteWaterBillAsync(billToDelete);

            // Quay lại trang danh sách hóa đơn điện
            return RedirectToAction("ShowWaterBill");
        }
        public async Task<IActionResult> CalculateExpense()
        {
            var staff = (await _expenseRepository.ShowStaffAsync()).ToList();
            var elecBills = (await _expenseRepository.ShowElectricityBillAsync()).ToList();
            var waterBills = (await _expenseRepository.ShowWaterBillAsync()).ToList();

            var model = new Tuple<List<Staff>, List<ElectricityBill>, List<WaterBill>>(staff, elecBills, waterBills);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddMaterial()
        {
            var warehouses = await _context.Warehouses.ToListAsync();
            var vm = new AddMaterialViewModel
            {
                DanhSachNguyenLieu = warehouses ?? new List<Warehouse>()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial(AddMaterialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.DanhSachNguyenLieu = await _context.Warehouses.ToListAsync();
                return View(vm);
            }

            var nguyenLieu = await _context.Warehouses.FirstOrDefaultAsync(x => x.MaNL == vm.SelectedMaNL);
            if (nguyenLieu == null)
            {
                ModelState.AddModelError("", "Nguyên liệu không tồn tại.");
                vm.DanhSachNguyenLieu = await _context.Warehouses.ToListAsync();
                return View(vm);
            }

            var input = new MaterialInput
            {
                MaNL = vm.SelectedMaNL,
                Quantity = vm.SoLuongNhap,
                UnitPrice = nguyenLieu.Gia
            };

            await _expenseRepository.AddMaterialInputAsync(input);

            TempData["SuccessMessage"] = "Nhập nguyên liệu thành công!";
            return RedirectToAction("ShowMaterialInput");
        }
    }
}

