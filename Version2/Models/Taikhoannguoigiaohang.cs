using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Taikhoannguoigiaohang
    {
        public int IdtaiKhoan { get; set; }
        public int IdnguoiGiaoHang { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public DateTime? NgayTaoTaiKhoan { get; set; }

        public virtual Nguoigiaohang IdnguoiGiaoHangNavigation { get; set; } = null!;
    }
}
