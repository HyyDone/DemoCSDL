using System.ComponentModel.DataAnnotations;
using AdminDACS.Models;

namespace AdminDACS.Models
{
    public class Customer
    {
        [Key]
        public string MaKH { get; set; }
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
        public ICollection<Booking> Bookings { get; set; }
    }
}
