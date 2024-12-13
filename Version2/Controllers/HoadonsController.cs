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
    public class HoadonsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public HoadonsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Hoadons
        // Hiển thị danh sách hóa đơn với phân trang và tìm kiếm
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            const int pageSize = 10;

            if (_context.Hoadons == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hoadons' is null.");
            }

            // Truy vấn dữ liệu và nạp navigation properties
            IQueryable<Hoadon> hoadonQuery = _context.Hoadons.Include(h => h.IdphieuDatHangNavigation);

            // Tìm kiếm theo trạng thái
            if (!string.IsNullOrEmpty(searchTerm))
            {
                hoadonQuery = hoadonQuery.Where(h => h.TrangThai.Contains(searchTerm));
            }

            // Tổng số mục và tính số trang
            int totalItems = await hoadonQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy dữ liệu với phân trang
            var hoadons = await hoadonQuery
                .OrderByDescending(h => h.IdhoaDon)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Gửi dữ liệu phân trang và tìm kiếm sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(hoadons);
        }

        // GET: Hoadons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons
                .Include(h => h.IdphieuDatHangNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // GET: Hoadons/Create
        public IActionResult Create()
        {
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang");
            return View();
        }

        // POST: Hoadons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdhoaDon,IdphieuDatHang,NgayLap,TongTien,TrangThai")] Hoadon hoadon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoadon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", hoadon.IdphieuDatHang);
            return View(hoadon);
        }

        // GET: Hoadons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons.FindAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", hoadon.IdphieuDatHang);
            return View(hoadon);
        }

        // POST: Hoadons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdhoaDon,IdphieuDatHang,NgayLap,TongTien,TrangThai")] Hoadon hoadon)
        {
            if (id != hoadon.IdhoaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoadon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoadonExists(hoadon.IdhoaDon))
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
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", hoadon.IdphieuDatHang);
            return View(hoadon);
        }

        // GET: Hoadons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await _context.Hoadons
                .Include(h => h.IdphieuDatHangNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            return View(hoadon);
        }

        // POST: Hoadons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hoadons == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Hoadons'  is null.");
            }
            var hoadon = await _context.Hoadons.FindAsync(id);
            if (hoadon != null)
            {
                _context.Hoadons.Remove(hoadon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoadonExists(int id)
        {
            return (_context.Hoadons?.Any(e => e.IdhoaDon == id)).GetValueOrDefault();
        }
    }
}
