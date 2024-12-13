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
    public class PhieudathangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public PhieudathangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Phieudathangs
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {

            const int pageSize = 5; // Number of items per page

            if (_context.Phieudathangs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Phieudathangs' is null.");
            }

            // Base query
            IQueryable<Phieudathang> query = _context.Phieudathangs.Include(p => p.IdkhachHangNavigation);

            // Search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.TrangThaiThanhToan.Contains(searchTerm));
            }

            // Total items and pages calculation
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Retrieve paginated data
            var phieudathangs = await query
                .OrderByDescending(p => p.IdphieuDatHang) // Sorting by ID
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //// Pass pagination data to the View
            //ViewBag.CurrentPage = page;
            //ViewBag.TotalPages = totalPages;
            //ViewBag.SearchTerm = searchTerm;

            //return View(phieudathangs);

            var heThongBanSachContext = _context.Phieudathangs
                .Include(p => p.IdkhachHangNavigation)
                .Include(p => p.Hoadons); // Bao gồm dữ liệu từ bảng HoaDon
            return View(await heThongBanSachContext.ToListAsync());

        }

        // GET: Phieudathangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phieudathangs == null)
            {
                return NotFound();
            }

            var phieudathang = await _context.Phieudathangs
                .Include(p => p.IdkhachHangNavigation)
                .FirstOrDefaultAsync(m => m.IdphieuDatHang == id);
            if (phieudathang == null)
            {
                return NotFound();
            }

            return View(phieudathang);
        }

        public IActionResult Create()
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu
            var khachhangList = _context.Khachhangs.Select(kh => new { kh.IdkhachHang, kh.TenKhachHang }).ToList();
            if (khachhangList != null && khachhangList.Any())
            {
                ViewData["IdkhachHang"] = new SelectList(khachhangList, "IdkhachHang", "TenKhachHang");
            }
            else
            {
                ViewData["IdkhachHang"] = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            // Kiểm tra danh sách sách
            ViewBag.Idsach = new SelectList(_context.Saches, "Idsach", "TenSach");

            // Khởi tạo danh sách chi tiết phiếu đặt hàng rỗng
            var chitietphieudathangs = new List<Chitietphieudathang> { new Chitietphieudathang() };

            // Trả về Phieudathang với danh sách Chitietphieudathangs
            return View(new Phieudathang { Chitietphieudathangs = chitietphieudathangs });
        }


        // POST: Phieudathangs/Create
        [HttpPost]
        public IActionResult Create(Phieudathang model)
        {
                try
                {
                    // Tìm khách hàng dựa trên IdkhachHang
                    var khachhang = _context.Khachhangs.FirstOrDefault(k => k.IdkhachHang == model.IdkhachHang);
                    if (khachhang != null)
                    {
                        model.IdkhachHangNavigation = khachhang; // Gán đối tượng Khachhang vào navigation property
                    }
                    else
                    {
                        ModelState.AddModelError("", "Khách hàng không tồn tại.");
                        return View(model);
                    }

                    // Lưu phiếu đặt hàng
                    _context.Phieudathangs.Add(model);
                    _context.SaveChanges();


                Chitietphieudathang chitiet = new Chitietphieudathang();
                chitiet.IdphieuDatHang = model.IdphieuDatHang;
                    _context.Chitietphieudathangs.Add(chitiet);
                
                _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
 
            return View(model);
        }




        // GET: Phieudathangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Phieudathangs == null)
            {
                return NotFound();
            }

            var phieudathang = await _context.Phieudathangs.FindAsync(id);
            if (phieudathang == null)
            {
                return NotFound();
            }
            ViewData["IdkhachHang"] = new SelectList(_context.Khachhangs, "IdkhachHang", "IdkhachHang", phieudathang.IdkhachHang);
            return View(phieudathang);
        }

        // POST: Phieudathangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdphieuDatHang,IdkhachHang,NgayLapHoaDon,TrangThaiThanhToan")] Phieudathang phieudathang)
        {
            if (id != phieudathang.IdphieuDatHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieudathang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieudathangExists(phieudathang.IdphieuDatHang))
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
            ViewData["IdkhachHang"] = new SelectList(_context.Khachhangs, "IdkhachHang", "IdkhachHang", phieudathang.IdkhachHang);
            return View(phieudathang);
        }

        // GET: Phieudathangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Phieudathangs == null)
            {
                return NotFound();
            }

            var phieudathang = await _context.Phieudathangs
                .Include(p => p.IdkhachHangNavigation)
                .FirstOrDefaultAsync(m => m.IdphieuDatHang == id);
            if (phieudathang == null)
            {
                return NotFound();
            }

            return View(phieudathang);
        }

        // POST: Phieudathangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Phieudathangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Phieudathangs'  is null.");
            }
            var phieudathang = await _context.Phieudathangs.FindAsync(id);
            if (phieudathang != null)
            {
                _context.Phieudathangs.Remove(phieudathang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhieudathangExists(int id)
        {
            return (_context.Phieudathangs?.Any(e => e.IdphieuDatHang == id)).GetValueOrDefault();
        }
    }
}
