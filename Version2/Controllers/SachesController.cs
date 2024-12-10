using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using System.Linq;
using Version2.Models;
namespace Version2.Controllers
{
    public class SachesController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public SachesController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách sách

        public IActionResult Index()
        {
            var sach = _context.Saches.Include(h => h.IddanhMucNavigation).Include(h => h.IdnhaXuatBanNavigation);
            ViewBag.sach = sach.ToList();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Models.SachModel sach)
        {
            Sach n = _context.Saches.Find(sach.Idsach);
            if (ModelState.IsValid)
            {
                Sach h = new Sach();
                h.TenSach = sach.TenSach;
                h.NamXuatBan = sach.NamXuatBan;
                h.SoTrang = sach.SoTrang;
                h.Gia = sach.Gia;
                h.SoLuongTon = sach.SoLuongTon;
                h.MoTa = sach.MoTa;
                h.Isbn = sach.Isbn;

                h.IddanhMucNavigation = _context.Danhmucs.Find(sach.IddanhMuc);
                h.IdnhaXuatBanNavigation = _context.Nhaxuatbans.Find(sach.IdnhaXuatBan);

                if (sach.HinhAnh.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sach.Idsach + "_" + sach.HinhAnh.FileName);
                    using (var s = System.IO.File.Create(path))
                    {
                        sach.HinhAnh.CopyTo(s);
                    }
                    h.HinhAnh = "~/images/" + sach.Idsach + "_" + sach.HinhAnh.FileName;
                }
                else
                {
                    h.HinhAnh = "";
                }

                _context.Saches.Add(h);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else if (n != null)
            {
                //ViewBag.nv = n;
                ModelState.AddModelError("Idsach", "ID sách này đã tồn tại!");
                // return View("loiThemHH", hanghoa);
                return View("formThemHH", sach);
            }
            else
            {
                return View("formThemHH");
            }
        }
        [HttpGet]
        public ActionResult loiThemHH(Models.SachModel sach)
        {

            return View(sach);
        }


        public IActionResult xemNsx(int id)
        {
            Nhaxuatban x = _context.Nhaxuatbans.Find(id);

            return PartialView(x);
        }

        public IActionResult Edit(int id)
        {
            var sach = _context.Saches.Include(h => h.IddanhMucNavigation).Include(h => h.IdnhaXuatBanNavigation).FirstOrDefault(s => s.Idsach == id);
            if (sach == null)
            {
                return NotFound();
            }
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm] Models.SachModel sach)
        {
            if (id != sach.Idsach)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var h = _context.Saches.Find(id);
                if (h != null)
                {
                    h.TenSach = sach.TenSach;
                    h.NamXuatBan = sach.NamXuatBan;
                    h.SoTrang = sach.SoTrang;
                    h.Gia = sach.Gia;
                    h.SoLuongTon = sach.SoLuongTon;
                    h.MoTa = sach.MoTa;
                    h.Isbn = sach.Isbn;
                    h.IddanhMucNavigation = _context.Danhmucs.Find(sach.IddanhMuc);
                    h.IdnhaXuatBanNavigation = _context.Nhaxuatbans.Find(sach.IdnhaXuatBan);

                    if (sach.HinhAnh.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sach.Idsach + "_" + sach.HinhAnh.FileName);
                        using (var s = System.IO.File.Create(path))
                        {
                            sach.HinhAnh.CopyTo(s);
                        }
                        h.HinhAnh = "~/images/" + sach.Idsach + "_" + sach.HinhAnh.FileName;
                    }

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(sach);
        }

        // Chức năng xóa sách
        public IActionResult Delete(int id)
        {
            var sach = _context.Saches.FirstOrDefault(s => s.Idsach == id);
            if (sach == null)
            {
                return NotFound();
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var sach = _context.Saches.Find(id);
            if (sach != null)
            {
                _context.Saches.Remove(sach);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
