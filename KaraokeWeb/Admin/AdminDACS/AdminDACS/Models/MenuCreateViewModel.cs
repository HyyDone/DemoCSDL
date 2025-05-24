using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class MenuCreateViewModel
    {
        [Required]
        public string TenMon { get; set; }

        public string MoTa { get; set; }

        [Required]
        public decimal Gia { get; set; }

        [Required]
        public string LoaiMon { get; set; }

        public IFormFile ImageFile { get; set; }

        // Nguyên liệu
        [Required]
        public string TenNL { get; set; }

        [Required]
        public decimal GiaNL { get; set; }

        [Required]
        public int SL { get; set; }
    }

}
