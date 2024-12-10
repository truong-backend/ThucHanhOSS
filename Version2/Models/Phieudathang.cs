using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Phieudathang
    {
        public Phieudathang()
        {
            Chitietphieudathangs = new HashSet<Chitietphieudathang>();
            Donhangs = new HashSet<Donhang>();
            Hoadons = new HashSet<Hoadon>();
        }

        public int IdphieuDatHang { get; set; }
        public int IdkhachHang { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public string? TrangThaiThanhToan { get; set; }

        public virtual Khachhang IdkhachHangNavigation { get; set; } = null!;
        public virtual ICollection<Chitietphieudathang> Chitietphieudathangs { get; set; }
        public virtual ICollection<Donhang> Donhangs { get; set; }
        public virtual ICollection<Hoadon> Hoadons { get; set; }
    }
}
