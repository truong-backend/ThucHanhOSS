using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Chitietphieudathang
    {
        public int IdphieuDatHang { get; set; }
        public int Idsach { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }

        public virtual Phieudathang IdphieuDatHangNavigation { get; set; } = null!;
        public virtual Sach IdsachNavigation { get; set; } = null!;
    }
}
