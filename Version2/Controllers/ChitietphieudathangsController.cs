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
    public class ChitietphieudathangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public ChitietphieudathangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Chitietphieudathangs
        public async Task<IActionResult> Index()
        {
            var heThongBanSachContext = _context.Chitietphieudathangs.Include(c => c.IdphieuDatHangNavigation).Include(c => c.IdsachNavigation);
            return View(await heThongBanSachContext.ToListAsync());
        }

        // GET: Chitietphieudathangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chitietphieudathangs == null)
            {
                return NotFound();
            }

            var chitietphieudathang = await _context.Chitietphieudathangs
                .Include(c => c.IdphieuDatHangNavigation)
                .Include(c => c.IdsachNavigation)
                .FirstOrDefaultAsync(m => m.IdphieuDatHang == id);
            if (chitietphieudathang == null)
            {
                return NotFound();
            }

            return View(chitietphieudathang);
        }

        // GET: Chitietphieudathangs/Create
        public IActionResult Create()
        {
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang");
            ViewData["Idsach"] = new SelectList(_context.Saches, "Idsach", "Idsach");
            return View();
        }

        // POST: Chitietphieudathangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdphieuDatHang,Idsach,SoLuong,Gia")] Chitietphieudathang chitietphieudathang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chitietphieudathang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", chitietphieudathang.IdphieuDatHang);
            ViewData["Idsach"] = new SelectList(_context.Saches, "Idsach", "Idsach", chitietphieudathang.Idsach);
            return View(chitietphieudathang);
        }

        // GET: Chitietphieudathangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chitietphieudathangs == null)
            {
                return NotFound();
            }

            var chitietphieudathang = await _context.Chitietphieudathangs.FindAsync(id);
            if (chitietphieudathang == null)
            {
                return NotFound();
            }
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", chitietphieudathang.IdphieuDatHang);
            ViewData["Idsach"] = new SelectList(_context.Saches, "Idsach", "Idsach", chitietphieudathang.Idsach);
            return View(chitietphieudathang);
        }

        // POST: Chitietphieudathangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdphieuDatHang,Idsach,SoLuong,Gia")] Chitietphieudathang chitietphieudathang)
        {
            if (id != chitietphieudathang.IdphieuDatHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chitietphieudathang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChitietphieudathangExists(chitietphieudathang.IdphieuDatHang))
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
            ViewData["IdphieuDatHang"] = new SelectList(_context.Phieudathangs, "IdphieuDatHang", "IdphieuDatHang", chitietphieudathang.IdphieuDatHang);
            ViewData["Idsach"] = new SelectList(_context.Saches, "Idsach", "Idsach", chitietphieudathang.Idsach);
            return View(chitietphieudathang);
        }

        // GET: Chitietphieudathangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chitietphieudathangs == null)
            {
                return NotFound();
            }

            var chitietphieudathang = await _context.Chitietphieudathangs
                .Include(c => c.IdphieuDatHangNavigation)
                .Include(c => c.IdsachNavigation)
                .FirstOrDefaultAsync(m => m.IdphieuDatHang == id);
            if (chitietphieudathang == null)
            {
                return NotFound();
            }

            return View(chitietphieudathang);
        }

        // POST: Chitietphieudathangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chitietphieudathangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Chitietphieudathangs'  is null.");
            }
            var chitietphieudathang = await _context.Chitietphieudathangs.FindAsync(id);
            if (chitietphieudathang != null)
            {
                _context.Chitietphieudathangs.Remove(chitietphieudathang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChitietphieudathangExists(int id)
        {
          return (_context.Chitietphieudathangs?.Any(e => e.IdphieuDatHang == id)).GetValueOrDefault();
        }
    }
}
