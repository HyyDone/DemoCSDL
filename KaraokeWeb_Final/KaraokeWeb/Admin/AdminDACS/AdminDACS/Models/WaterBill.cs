using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class WaterBill
    {
        [Key]
        public string MaBillWater { get; set; }
        public DateTime Ngay { get; set; }
        public decimal SoTien { get; set; }
    }
}
