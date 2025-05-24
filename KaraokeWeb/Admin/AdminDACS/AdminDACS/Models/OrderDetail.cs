using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDACS.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(10)]
        public string MaMon { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("MaMon")]
        public Menu? Menu { get; set; }
    }
}
