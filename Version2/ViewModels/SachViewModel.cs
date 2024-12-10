
using System;
using System.ComponentModel.DataAnnotations;
using Version2.Models;

namespace Version2.ViewModels
{
    public class SachViewModel
    {
        public List<Sach> Saches { get; set; }
        public List<Danhmuc> LoaiSaches { get; set; }
        public List<Tacgium> TacGias { get; set; }
    }
}
