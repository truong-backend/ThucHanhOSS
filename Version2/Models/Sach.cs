using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Sach
    {
        public Sach()
        {
            Chitietphieudathangs = new HashSet<Chitietphieudathang>();
            IdtacGia = new HashSet<Tacgium>();
        }

        public int Idsach { get; set; }
        public string? HinhAnh { get; set; }
        public string TenSach { get; set; } = null!;
        public int IdnhaXuatBan { get; set; }
        public int IddanhMuc { get; set; }
        public int? NamXuatBan { get; set; }
        public int? SoTrang { get; set; }
        public decimal Gia { get; set; }
        public int? SoLuongTon { get; set; }
        public string? MoTa { get; set; }
        public string? Isbn { get; set; }

        public virtual Danhmuc IddanhMucNavigation { get; set; } = null!;
        public virtual Nhaxuatban IdnhaXuatBanNavigation { get; set; } = null!;
        public virtual ICollection<Chitietphieudathang> Chitietphieudathangs { get; set; }

        public virtual ICollection<Tacgium> IdtacGia { get; set; }
    }
}
