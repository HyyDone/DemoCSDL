using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AdminDACS.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public string MaPhong { get; set; } = string.Empty;

        public decimal? TotalAmount { get; set; }
        public decimal? TotalFoodAmount { get; set; }
        public decimal? TotalRoomAmount { get; set; }
        public bool IsPaid { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
        [ForeignKey("MaPhong")]
        public Room? Room  { get; set; }
    }
}
