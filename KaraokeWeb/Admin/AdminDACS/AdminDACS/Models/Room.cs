using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AdminDACS.Models
{
    public class Room
    {
        [Key]
        public string MaPhong { get; set; }

        [Required]
        public string TenPhong { get; set; }
        [Required]
        public decimal Gia { get; set; }
        public int SucChua { get; set; }
        public bool? TinhTrang { get; set; }

        [Required]
        [ForeignKey("LoaiPhong")]
        public string MaLoaiPhong { get; set; }  // đổi từ int -> string

        public RoomType LoaiPhong { get; set; }

        [Required]
        public string? ImageUrl { get; set; }
        public List<RoomImage> Images { get; set; }
    }
}
