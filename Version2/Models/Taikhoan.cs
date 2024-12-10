using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Taikhoan
    {
        public int IdtaiKhoan { get; set; }
        public int IdkhachHang { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public DateTime? NgayTaoTaiKhoan { get; set; }

        public virtual Khachhang IdkhachHangNavigation { get; set; } = null!;
    }
}
