using System.ComponentModel.DataAnnotations;

namespace UserDACS.Models
{
    public class RoomType
    {
        [Key]
        [StringLength(20)]
        public string MaLoaiPhong { get; set; }
        [Required]
        [StringLength(100)]
        public string LoaiPhong { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
