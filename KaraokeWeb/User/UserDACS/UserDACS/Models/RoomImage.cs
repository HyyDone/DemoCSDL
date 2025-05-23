using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace UserDACS.Models
{
    public class RoomImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        [ForeignKey("Room")]
        public string MaPhong { get; set; } = string.Empty;
        public Room? Room  { get; set; }
    }
}
