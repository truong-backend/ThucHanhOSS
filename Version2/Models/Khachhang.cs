using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Khachhang
    {
        public Khachhang()
        {
            Phieudathangs = new HashSet<Phieudathang>();
            Taikhoans = new HashSet<Taikhoan>();
        }

        public int IdkhachHang { get; set; }
        public string TenKhachHang { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public DateTime? NgayDangKy { get; set; }

        public virtual ICollection<Phieudathang> Phieudathangs { get; set; }
        public virtual ICollection<Taikhoan> Taikhoans { get; set; }
    }
}
