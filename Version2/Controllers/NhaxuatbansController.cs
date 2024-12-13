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
    public class NhaxuatbansController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public NhaxuatbansController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Nhaxuatbans
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            ViewBag.ls = _context.Nhaxuatbans;

            const int pageSize = 10;

            if (_context.Nhaxuatbans == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.TacGia' is null.");
            }

            // Tìm kiếm theo tên hoặc quốc tịch
            IQueryable<Nhaxuatban> tacGiasQuery = _context.Nhaxuatbans;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tacGiasQuery = tacGiasQuery.Where(t => t.TenNhaXuatBan.Contains(searchTerm));
            }

            // Tổng số mục và trang
            int totalItems = await tacGiasQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy tác giả với phân trang
            var tacGias = await tacGiasQuery
            .OrderByDescending(t => t.IdnhaXuatBan)
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

        // GET: Nhaxuatbans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nhaxuatbans == null)
            {
                return NotFound();
            }

            var nhaxuatban = await _context.Nhaxuatbans
                .FirstOrDefaultAsync(m => m.IdnhaXuatBan == id);
            if (nhaxuatban == null)
            {
                return NotFound();
            }

            return View(nhaxuatban);
        }

        // GET: Nhaxuatbans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nhaxuatbans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdnhaXuatBan,TenNhaXuatBan,DiaChi,SoDienThoai,Email,Website")] Nhaxuatban nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaxuatban);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaxuatban);
        }

        // GET: Nhaxuatbans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nhaxuatbans == null)
            {
                return NotFound();
            }

            var nhaxuatban = await _context.Nhaxuatbans.FindAsync(id);
            if (nhaxuatban == null)
            {
                return NotFound();
            }
            return View(nhaxuatban);
        }

        // POST: Nhaxuatbans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdnhaXuatBan,TenNhaXuatBan,DiaChi,SoDienThoai,Email,Website")] Nhaxuatban nhaxuatban)
        {
            if (id != nhaxuatban.IdnhaXuatBan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaxuatban);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaxuatbanExists(nhaxuatban.IdnhaXuatBan))
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
            return View(nhaxuatban);
        }

        // GET: Nhaxuatbans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nhaxuatbans == null)
            {
                return NotFound();
            }

            var nhaxuatban = await _context.Nhaxuatbans
                .FirstOrDefaultAsync(m => m.IdnhaXuatBan == id);
            if (nhaxuatban == null)
            {
                return NotFound();
            }

            return View(nhaxuatban);
        }

        // POST: Nhaxuatbans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nhaxuatbans == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Nhaxuatbans'  is null.");
            }
            var nhaxuatban = await _context.Nhaxuatbans.FindAsync(id);
            if (nhaxuatban != null)
            {
                _context.Nhaxuatbans.Remove(nhaxuatban);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaxuatbanExists(int id)
        {
            return (_context.Nhaxuatbans?.Any(e => e.IdnhaXuatBan == id)).GetValueOrDefault();
        }
    }
}
