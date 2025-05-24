using System.ComponentModel.DataAnnotations;
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

        public decimal TotalAmount { get; set; }

        public bool IsPaid { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
