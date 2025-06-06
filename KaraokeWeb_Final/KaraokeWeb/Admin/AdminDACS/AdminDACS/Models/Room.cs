using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AdminDACS.Models
{
    public class Room
    {
        [Key]
        public string? MaPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phòng.")]
        public string TenPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        public decimal Gia { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập sức chứa.")]
        public int SucChua { get; set; }

        public bool? TinhTrang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại phòng.")]
        [ForeignKey("LoaiPhong")]
        public string MaLoaiPhong { get; set; }

        [NotMapped]
        public RoomType? LoaiPhong { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public List<RoomImage>? Images { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
