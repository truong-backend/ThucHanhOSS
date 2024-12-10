using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Version2.Models
{
    public partial class Danhmuc
    {
        public Danhmuc()
        {
            Saches = new HashSet<Sach>();
        }

        [Key]
        public int IddanhMuc { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên danh mục phải có độ dài từ 3 đến 100 ký tự.")]
        public string TenDanhMuc { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? MoTa { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
