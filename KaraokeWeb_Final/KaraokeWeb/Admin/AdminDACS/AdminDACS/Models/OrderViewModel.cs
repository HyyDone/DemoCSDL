using AdminDACS.Models;

namespace AdminDACS.Models
{
    public class OrderViewModel
    {
        public string MaPhong { get; set; }
        public List<Menu> DanhSachMon { get; set; } = new();
        public List<Menu> DoAnList { get; set; } = new();
        public List<Menu> DoUongList { get; set; } = new();
        public List<OrderDetail> OrderDetails { get; set; } = new();
        public Dictionary<string, int> Quantities { get; set; } = new();
        public decimal TongTien { get; set; }
    }

}
