namespace AdminDACS.Models
{
    public class ExpenseDTO
    {
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<ElectricityBill> ElectricityBills { get; set; }
        public IEnumerable<WaterBill> WaterBills { get; set; }
        public IEnumerable<MaterialInput>  MaterialInputs { get; set; }
    }
}
