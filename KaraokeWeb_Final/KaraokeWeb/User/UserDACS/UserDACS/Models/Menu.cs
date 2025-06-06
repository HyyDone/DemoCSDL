using System.ComponentModel.DataAnnotations;

namespace UserDACS.Models
{
    public class Menu
    {
        [Key]
        [StringLength(10)]
        public string MaMon { get; set; }

        [Required]
        [StringLength(100)]
        public string TenMon { get; set; }

        [StringLength(255)]
        public string MoTa { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Gia { get; set; }

        [Required]
        [StringLength(20)]
        public string LoaiMon { get; set; } = "Food";

        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
