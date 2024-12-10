using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Nhaxuatban
    {
        public Nhaxuatban()
        {
            Saches = new HashSet<Sach>();
        }

        public int IdnhaXuatBan { get; set; }
        public string TenNhaXuatBan { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
