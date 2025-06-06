using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDACS.Models
{
    public class TempOrderItem
    {
        [Key]
        public int TempOrderItemId { get; set; }

        [Required]
        public string MaPhong { get; set; }

        [Required]
        [MaxLength(10)]
        public string MaMon { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        [ForeignKey("MaMon")]
        public Menu? Menu { get; set; }

        [ForeignKey("MaPhong")]
        public Room? Room { get; set; }
    }
}
