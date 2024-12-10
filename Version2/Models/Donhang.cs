using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Donhang
    {
        public int IddonHang { get; set; }
        public int IdphieuDatHang { get; set; }
        public int IdnguoiGiaoHang { get; set; }
        public DateTime? NgayGiaoHang { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }

        public virtual Nguoigiaohang IdnguoiGiaoHangNavigation { get; set; } = null!;
        public virtual Phieudathang IdphieuDatHangNavigation { get; set; } = null!;
    }
}
