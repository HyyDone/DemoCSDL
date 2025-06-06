using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class AddMaterialViewModel
    {
        [Required]
        public string SelectedMaNL { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuongNhap { get; set; }
        public decimal GiaNL { get; set; }

        public List<Warehouse> DanhSachNguyenLieu { get; set; } = new List<Warehouse>();
    }
}
