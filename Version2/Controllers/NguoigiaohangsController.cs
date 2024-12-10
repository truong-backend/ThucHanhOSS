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
    public class NguoigiaohangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public NguoigiaohangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Nguoigiaohangs
        public async Task<IActionResult> Index()
        {
              return _context.Nguoigiaohangs != null ? 
                          View(await _context.Nguoigiaohangs.ToListAsync()) :
                          Problem("Entity set 'HeThongBanSachContext.Nguoigiaohangs'  is null.");
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
