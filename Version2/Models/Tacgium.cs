using System;
using System.Collections.Generic;

namespace Version2.Models
{
    public partial class Tacgium
    {
        public Tacgium()
        {
            Idsaches = new HashSet<Sach>();
        }

        public int IdtacGia { get; set; }
        public string TenTacGia { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? QuocTich { get; set; }

        public virtual ICollection<Sach> Idsaches { get; set; }
    }
}
