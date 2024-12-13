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
    public class DonhangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public DonhangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Donhangs
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
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

        [HttpGet]
        public async Task<IActionResult> UpdateTrangThai()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrangThai(int id)
        {
            // Find the Donhang by ID
            var donhang = await _context.Donhangs.FindAsync(id);

            if (donhang != null)
            {
                // Check if the current TrangThai is "Đang Giao" and update it
                if (donhang.TrangThai == "Đang Giao")
                {
                    donhang.TrangThai = "Hoàn Thành";
                    _context.Update(donhang); // Mark the entity as modified
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
            }

            // Redirect to the same page to see the updated list
            return RedirectToAction("Index");
        }
        // GET: Donhangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdnguoiGiaoHangNavigation)
                .Include(d => d.IdphieuDatHangNavigation)
                .FirstOrDefaultAsync(m => m.IddonHang == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
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
        // GET: Donhangs/Create
        public IActionResult Create()
        {
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang");
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang");
            return View();
        }

        // POST: Donhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddonHang,IdphieuDatHang,IdnguoiGiaoHang,NgayGiaoHang,TrangThai,GhiChu")] Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", donhang.IdnguoiGiaoHang);
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", donhang.IdphieuDatHang);
            return View(donhang);
        }

        // GET: Donhangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang == null)
            {
                return NotFound();
            }
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", donhang.IdnguoiGiaoHang);
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", donhang.IdphieuDatHang);
            return View(donhang);
        }

        // POST: Donhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IddonHang,IdphieuDatHang,IdnguoiGiaoHang,NgayGiaoHang,TrangThai,GhiChu")] Donhang donhang)
        {
            if (id != donhang.IddonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonhangExists(donhang.IddonHang))
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
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", donhang.IdnguoiGiaoHang);
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", donhang.IdphieuDatHang);
            return View(donhang);
        }

        // GET: Donhangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.IdnguoiGiaoHangNavigation)
                .Include(d => d.IdphieuDatHangNavigation)
                .FirstOrDefaultAsync(m => m.IddonHang == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: Donhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donhangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Donhangs'  is null.");
            }
            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang != null)
            {
                _context.Donhangs.Remove(donhang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonhangExists(int id)
        {
            return (_context.Donhangs?.Any(e => e.IddonHang == id)).GetValueOrDefault();
        }
    }
}
