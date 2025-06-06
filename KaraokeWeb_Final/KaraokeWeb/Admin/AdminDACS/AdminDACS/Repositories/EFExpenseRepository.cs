using Microsoft.EntityFrameworkCore;
using AdminDACS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AdminDACS.Repositories
{
    public class EFExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDBContext _context;

        public EFExpenseRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> ShowStaffAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<IEnumerable<ElectricityBill>> ShowElectricityBillAsync()
        {
            return await _context.ElectricityBillRecords.ToListAsync();
        }

        public async Task<IEnumerable<WaterBill>> ShowWaterBillAsync()
        {
            return await _context.WaterBillRecords.ToListAsync();
        }

        // Thêm lấy danh sách nhập nguyên liệu
        public async Task<IEnumerable<MaterialInput>> ShowMaterialInputAsync()
        {
            return await _context.MaterialInputs.ToListAsync();
        }

        public async Task<IEnumerable<ExpenseDTO>> ShowExpenseAsync()
        {
            var staff = await ShowStaffAsync();
            var electricityBills = await ShowElectricityBillAsync();
            var waterBills = await ShowWaterBillAsync();
            var materialInputs = await ShowMaterialInputAsync();

            var expenseDTOList = new List<ExpenseDTO>
            {
                new ExpenseDTO
                {
                    Staff = staff,
                    ElectricityBills = electricityBills,
                    WaterBills = waterBills,
                    MaterialInputs = materialInputs
                }
            };

            return expenseDTOList;
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            var existingStaff = await _context.Staffs.FindAsync(staff.MaNhanVien);
            if (existingStaff != null)
            {
                existingStaff.Luong = staff.Luong;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteStaffAsync(string maNhanVien)
        {
            var staff = await _context.Staffs.FindAsync(maNhanVien);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddStaffAsync(Staff staff)
        {
            await _context.Staffs.AddAsync(staff);
            await _context.SaveChangesAsync();
        }

        public async Task AddElecBillAsync(ElectricityBill electricityBill)
        {
            await _context.ElectricityBillRecords.AddAsync(electricityBill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteElecBillAsync(ElectricityBill electricityBill)
        {
            var bill = await _context.ElectricityBillRecords.FindAsync(electricityBill.MaBillElec);
            if (bill != null)
            {
                _context.ElectricityBillRecords.Remove(bill);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddWaterBillAsync(WaterBill waterBill)
        {
            await _context.WaterBillRecords.AddAsync(waterBill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWaterBillAsync(WaterBill waterBill)
        {
            var bill = await _context.WaterBillRecords.FindAsync(waterBill.MaBillWater);
            if (bill != null)
            {
                _context.WaterBillRecords.Remove(bill);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddMaterialInputAsync(MaterialInput materialInput)
        {
            var nguyenLieu = await _context.Warehouses.FirstOrDefaultAsync(x => x.MaNL == materialInput.MaNL);
            if (nguyenLieu == null)
            {
                throw new Exception("Nguyên liệu không tồn tại.");
            }

            _context.MaterialInputs.Add(materialInput);
            nguyenLieu.SL += materialInput.Quantity;

            await _context.SaveChangesAsync();
        }
    }
}
