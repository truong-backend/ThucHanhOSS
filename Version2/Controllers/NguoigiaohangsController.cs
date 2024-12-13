using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Version2.Models;

namespace Version2.Controllers
{
    public class NguoigiaohangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public NguoigiaohangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Nguoigiaohangs
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            ViewBag.ls = _context.Nguoigiaohangs;

            const int pageSize = 10;

            if (_context.Nguoigiaohangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Nguoigiaohang> tacGiasQuery = _context.Nguoigiaohangs;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TenNguoiGiaoHang.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
                .OrderByDescending(t => t.IdnguoiGiaoHang)
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
        public async Task<IActionResult> UI_NguoiGiaoHang(string searchTerm, int page = 1)
        {
            ViewBag.ls = _context.Donhangs.ToList();

            const int pageSize = 10;

            if (_context.Donhangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Donhang> tacGiasQuery = _context.Donhangs;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TrangThai.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
             .Include(t => t.IdnguoiGiaoHangNavigation)
            .OrderByDescending(t => t.IddonHang)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Kiểm tra nếu không có tác giả nào trong danh sách
            if (!tacGias.Any())
            {
                ViewBag.Message = "Không có danh mục cần tìm hoặc danh sách rỗng.";
            }



            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm; // Lưu giá trị tìm kiếm trong ViewBag

            return View(tacGias);
        }
        // GET: Nguoigiaohangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nguoigiaohangs == null)
            {
                return NotFound();
            }

            var nguoigiaohang = await _context.Nguoigiaohangs
                .FirstOrDefaultAsync(m => m.IdnguoiGiaoHang == id);
            if (nguoigiaohang == null)
            {
                return NotFound();
            }

            return View(nguoigiaohang);
        }

        // GET: Nguoigiaohangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nguoigiaohangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdnguoiGiaoHang,TenNguoiGiaoHang,SoDienThoai,DiaChi,Email,NgayThamGia")] Nguoigiaohang nguoigiaohang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguoigiaohang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nguoigiaohang);
        }

        // GET: Nguoigiaohangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nguoigiaohangs == null)
            {
                return NotFound();
            }

            var nguoigiaohang = await _context.Nguoigiaohangs.FindAsync(id);
            if (nguoigiaohang == null)
            {
                return NotFound();
            }
            return View(nguoigiaohang);
        }

        // POST: Nguoigiaohangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdnguoiGiaoHang,TenNguoiGiaoHang,SoDienThoai,DiaChi,Email,NgayThamGia")] Nguoigiaohang nguoigiaohang)
        {
            if (id != nguoigiaohang.IdnguoiGiaoHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoigiaohang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoigiaohangExists(nguoigiaohang.IdnguoiGiaoHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nguoigiaohang);
        }

        // GET: Nguoigiaohangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nguoigiaohangs == null)
            {
                return NotFound();
            }

            var nguoigiaohang = await _context.Nguoigiaohangs
                .FirstOrDefaultAsync(m => m.IdnguoiGiaoHang == id);
            if (nguoigiaohang == null)
            {
                return NotFound();
            }

            return View(nguoigiaohang);
        }

        // POST: Nguoigiaohangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nguoigiaohangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Nguoigiaohangs'  is null.");
            }
            var nguoigiaohang = await _context.Nguoigiaohangs.FindAsync(id);
            if (nguoigiaohang != null)
            {
                _context.Nguoigiaohangs.Remove(nguoigiaohang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoigiaohangExists(int id)
        {
            return (_context.Nguoigiaohangs?.Any(e => e.IdnguoiGiaoHang == id)).GetValueOrDefault();
        }
    }
}
