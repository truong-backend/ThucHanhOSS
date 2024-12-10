using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Nguoigiaohang
    {
        public Nguoigiaohang()
        {
            Donhangs = new HashSet<Donhang>();
            Taikhoannguoigiaohangs = new HashSet<Taikhoannguoigiaohang>();
        }

        public int IdnguoiGiaoHang { get; set; }
        public string TenNguoiGiaoHang { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Email { get; set; }
        public DateTime? NgayThamGia { get; set; }

        public virtual ICollection<Donhang> Donhangs { get; set; }
        public virtual ICollection<Taikhoannguoigiaohang> Taikhoannguoigiaohangs { get; set; }
    }
}
