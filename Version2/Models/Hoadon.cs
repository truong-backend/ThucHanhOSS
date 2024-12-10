using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Hoadon
    {
        public int IdhoaDon { get; set; }
        public int IdphieuDatHang { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = null!;

        public virtual Phieudathang IdphieuDatHangNavigation { get; set; } = null!;
    }
}
