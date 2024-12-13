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
            const int pageSize = 10;

            if (db.Taikhoans == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Hoadons' is null.");
            }

            // Truy vấn dữ liệu và nạp navigation properties
            IQueryable<Taikhoan> hoadonQuery = db.Taikhoans.Include(h => h.IdkhachHangNavigation);

            // Tìm kiếm theo trạng thái
            if (!string.IsNullOrEmpty(searchTerm))
            {
                hoadonQuery = hoadonQuery.Where(h => h.TenDangNhap.Contains(searchTerm));
            }

            // Tổng số mục và tính số trang
            int totalItems = await hoadonQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy dữ liệu với phân trang
            var hoadons = await hoadonQuery
                .OrderByDescending(h => h.IdtaiKhoan)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi dữ liệu phân trang và tìm kiếm sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(hoadons);
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
