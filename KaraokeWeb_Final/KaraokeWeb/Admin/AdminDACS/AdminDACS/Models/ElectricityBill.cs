using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace AdminDACS.Models
{
    public class ElectricityBill
    {
        [Key]
        public string MaBillElec { get; set; }
        public DateTime Ngay { get; set; }
        public decimal SoTien { get; set; }
    }
}
