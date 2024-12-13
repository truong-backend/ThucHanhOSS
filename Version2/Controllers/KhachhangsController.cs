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
    public class KhachhangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public KhachhangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Khachhangs
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            ViewBag.ls = _context.Khachhangs;

            const int pageSize = 10;

            if (_context.Danhmucs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Khachhang> tacGiasQuery = _context.Khachhangs;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TenKhachHang.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
            .OrderByDescending(t => t.IdkhachHang)
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

        // GET: Khachhangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Khachhangs == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .FirstOrDefaultAsync(m => m.IdkhachHang == id);
            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // GET: Khachhangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Khachhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdkhachHang,TenKhachHang,NgaySinh,DiaChi,SoDienThoai,NgayDangKy")] Khachhang khachhang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachhang);
        }

        // GET: Khachhangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Khachhangs == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang == null)
            {
                return NotFound();
            }
            return View(khachhang);
        }

        // POST: Khachhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdkhachHang,TenKhachHang,NgaySinh,DiaChi,SoDienThoai,NgayDangKy")] Khachhang khachhang)
        {
            if (id != khachhang.IdkhachHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachhang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachhangExists(khachhang.IdkhachHang))
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
            return View(khachhang);
        }

        // GET: Khachhangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Khachhangs == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs
                .FirstOrDefaultAsync(m => m.IdkhachHang == id);
            if (khachhang == null)
            {
                return NotFound();
            }

            return View(khachhang);
        }

        // POST: Khachhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Khachhangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Khachhangs'  is null.");
            }
            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang != null)
            {
                _context.Khachhangs.Remove(khachhang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachhangExists(int id)
        {
            return (_context.Khachhangs?.Any(e => e.IdkhachHang == id)).GetValueOrDefault();
        }
    }
}
