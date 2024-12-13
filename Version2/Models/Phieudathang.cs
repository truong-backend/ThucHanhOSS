using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Mã phiếu đặt hàng không được bỏ trống.")]
        public int IdphieuDatHang { get; set; }
        [Required]
        public int IdkhachHang { get; set; }
        [Required(ErrorMessage = "Ngày lập hóa đơn không được bỏ trống.")]
        public DateTime? NgayLapHoaDon { get; set; }
        [Required(ErrorMessage = "Trạng thái thanh toán không được bỏ trống.")]
        public string? TrangThaiThanhToan { get; set; }

        public virtual Khachhang IdkhachHangNavigation { get; set; }
        public virtual ICollection<Chitietphieudathang> Chitietphieudathangs { get; set; }
        public virtual ICollection<Donhang> Donhangs { get; set; }
        public virtual ICollection<Hoadon> Hoadons { get; set; }
    }
}
