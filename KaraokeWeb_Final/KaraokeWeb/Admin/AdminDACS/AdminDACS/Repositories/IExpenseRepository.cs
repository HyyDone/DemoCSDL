using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<ExpenseDTO>> ShowExpenseAsync();
        Task<IEnumerable<Staff>> ShowStaffAsync();
        Task<IEnumerable<ElectricityBill>> ShowElectricityBillAsync();
        Task<IEnumerable<WaterBill>> ShowWaterBillAsync();
        Task<IEnumerable<MaterialInput>> ShowMaterialInputAsync();
        Task AddStaffAsync(Staff staff);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(string MaNhanVien);
        Task AddElecBillAsync(ElectricityBill electricityBill);
        Task DeleteElecBillAsync(ElectricityBill electricityBill);
        Task AddWaterBillAsync(WaterBill waterBill);
        Task DeleteWaterBillAsync(WaterBill waterBill);
        Task AddMaterialInputAsync(MaterialInput materialInput);
    }
}
