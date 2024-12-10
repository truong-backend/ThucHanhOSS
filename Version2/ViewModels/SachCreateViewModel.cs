
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using Version2.Models;

namespace Version2.ViewModels
{
    public class SachCreateViewModel
    {
        public Sach? Sach { get; set; }
        public List<int>? SelectedAuthors { get; set; }
        public SelectList? DanhMucs { get; set; }
        public SelectList? Nhaxuatbans { get; set; }
        public SelectList? Tacgiums { get; set; }
    }


}
