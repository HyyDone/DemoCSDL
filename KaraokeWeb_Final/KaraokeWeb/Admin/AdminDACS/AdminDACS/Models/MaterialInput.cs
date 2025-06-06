using System.ComponentModel.DataAnnotations;

namespace AdminDACS.Models
{
    public class MaterialInput
    {
        [Key]
        public int InputId { get; set; }

        [Required]
        public string MaNL { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
