using AdminDACS.Models;

namespace AdminDACS.Models
{
    public class IncomeViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
