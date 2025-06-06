using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class Staff
    {
        [Key]
        public string MaNhanVien { get; set; }
        public string Ten { get; set; }
        public string SDT_NhanVien { get; set; }
        public string Email { get; set; }
        public decimal Luong {  get; set; }
    }
}
