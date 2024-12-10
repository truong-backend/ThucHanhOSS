using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Version2.Models;

namespace Version2.Controllers
{
    public class TacGiumsController : Controller
    {
        private HeThongBanSachContext db = new HeThongBanSachContext();
        // GET: TacGias
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            ViewBag.ls = db.Tacgia;

            const int pageSize = 10;

            if (db.Tacgia == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Tacgium> tacGiasQuery = db.Tacgia;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TenTacGia.Contains(searchTerm) || t.QuocTich.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
                .OrderByDescending(t => t.IdtacGia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Kiểm tra nếu không có tác giả nào trong danh sách
            if (!tacGias.Any())
            {
                ViewBag.Message = "Không có tác giả cần tìm hoặc danh sách rỗng.";
            }



            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm; // Lưu giá trị tìm kiếm trong ViewBag

            return View(tacGias);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tacgium ls)
        {


            // Kiểm tra xem tên tác giả đã tồn tại hay chưa
            if (db.Tacgia.Any(t => t.TenTacGia == ls.TenTacGia))
            {
                ModelState.AddModelError("TenTacGia", "Tên loại đã tồn tại trong hệ thống.");
            }

            // Nếu không có lỗi, thực hiện tạo mới
            if (ModelState.IsValid)
            {
                db.Add(ls);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, trả lại view để hiển thị thông báo
            return View(ls);

        }

        public async Task<IActionResult> Edit(int id)
        {
            Tacgium x = db.Tacgia.Find(id);
            return View(x);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Tacgium ls)
        {
            if (ModelState.IsValid)
            {
                Tacgium x = db.Tacgia.Find(ls.IdtacGia);
                if (x != null)
                {
                    x.TenTacGia = ls.TenTacGia;
                    x.NgaySinh = ls.NgaySinh;
                    x.QuocTich = ls.QuocTich;
                    db.SaveChanges();
;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit");
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            var ls = db.Tacgia.FirstOrDefault(ls => ls.IdtacGia == id);

            // Nếu không tìm thấy, trả về NotFound (hoặc xử lý theo cách khác)
            if (ls == null)
            {
                return NotFound();
            }

            // Truyền đối tượng LoaiSach vào View
            return View(ls);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteI(int id)
        {
            Tacgium ls = db.Tacgia.Find(id);
            db.Tacgia.Remove(ls);
            db.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
