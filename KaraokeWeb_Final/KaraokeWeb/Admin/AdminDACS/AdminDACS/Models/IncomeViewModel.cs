using AdminDACS.Models;

namespace AdminDACS.Models
{
    public class IncomeViewModel
    {
        public List<OrderIncomeViewModel> Orders { get; set; } = new();
        public decimal TotalIncome { get; set; }
    }
}
