using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class SachModel
    {

        public int Idsach { get; set; }
        public IFormFile? HinhAnh { get; set; }
        public string TenSach { get; set; } = null!;
        public int IdnhaXuatBan { get; set; }
        public int IddanhMuc { get; set; }
        public int? NamXuatBan { get; set; }
        public int? SoTrang { get; set; }
        public decimal Gia { get; set; }
        public int? SoLuongTon { get; set; }
        public string? MoTa { get; set; }
        public string? Isbn { get; set; }
    }
}
