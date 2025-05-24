using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class RoomViewModel
    {
        [Required]
        public string MaPhong { get; set; }

        [Required]
        public string TenPhong { get; set; }

        [Required]
        public decimal Gia { get; set; }

        public int SucChua { get; set; }

        [Required]
        public string MaLoaiPhong { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        public List<SelectListItem> LoaiPhongList { get; set; }
    }
}