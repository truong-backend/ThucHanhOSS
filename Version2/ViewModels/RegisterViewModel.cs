using System;
using System.ComponentModel.DataAnnotations;

namespace Version2.Models
{
    public class RegisterViewModel
    {
        // Thông tin tài khoản
        [Required]
        public string TenDangNhap { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        // Thông tin khách hàng
        [Required]
        public string TenKhachHang { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string? DiaChi { get; set; }

        public string? SoDienThoai { get; set; }

        public DateTime? NgayDangKy { get; set; }
    }
}
