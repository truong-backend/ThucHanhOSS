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
    public class TaikhoansController : Controller
    {
        private HeThongBanSachContext db = new HeThongBanSachContext();
        // GET: TacGias
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            var sach = db.Taikhoans.Include(h => h.IdkhachHangNavigation).Include(h => h.IdkhachHangNavigation);
            ViewBag.tk = sach.ToList();

            const int pageSize = 10;

            if (db.Taikhoans == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Taikhoan> tacGiasQuery = db.Taikhoans;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TenDangNhap.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
                .OrderByDescending(t => t.IdtaiKhoan)
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
        public async Task<IActionResult> Delete(int id)
        {
            var ls = db.Taikhoans.FirstOrDefault(ls => ls.IdtaiKhoan == id);

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
            Taikhoan ls = db.Taikhoans.Find(id);
            db.Taikhoans.Remove(ls);
            db.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
