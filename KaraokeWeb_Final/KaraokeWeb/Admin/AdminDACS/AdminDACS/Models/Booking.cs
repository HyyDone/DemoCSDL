using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDACS.Models
{
    public class Booking
    {
        [Key]
        public string MaBooking { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string MaKH { get; set; }
        public Customer Customer { get; set; }

        [Required]
        [ForeignKey("Room")]
        public string MaPhong { get; set; }
        public Room Room { get; set; }

        [Required]
        public DateTime BookingTime { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        public DateTime EndTime { get; set; }
    }
}
