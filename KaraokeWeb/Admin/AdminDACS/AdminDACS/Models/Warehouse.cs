using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class Warehouse
    {
        [Key]
        [StringLength(10)]
        public string MaNL { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNL { get; set; } 

        [Range(0, double.MaxValue)]
        public decimal Gia { get; set; } 

        [Range(0, int.MaxValue)]
        public int SL { get; set; }

        public string MaMon { get; set; } 
        public Menu Menu { get; set; }
    }
}
