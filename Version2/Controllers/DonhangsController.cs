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
    public class DonhangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public DonhangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Donhangs
        public async Task<IActionResult> Index()
        {
            var heThongBanSachContext = _context.Donhangs.Include(d => d.IdnguoiGiaoHangNavigation).Include(d => d.IdphieuDatHangNavigation);
            return View(await heThongBanSachContext.ToListAsync());
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddonHang,IdphieuDatHang,IdnguoiGiaoHang,NgayGiaoHang,TrangThai,GhiChu")] Donhang donhang)
        {
            
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IddonHang,IdphieuDatHang,IdnguoiGiaoHang,NgayGiaoHang,TrangThai,GhiChu")] Donhang donhang)
        {
            if (id != donhang.IddonHang)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
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
            //}
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
        //[ValidateAntiForgeryToken]
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
