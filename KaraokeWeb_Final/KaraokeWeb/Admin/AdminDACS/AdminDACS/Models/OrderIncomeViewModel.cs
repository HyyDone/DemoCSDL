namespace AdminDACS.Models
{
    public class OrderIncomeViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
