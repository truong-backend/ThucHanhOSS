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
    public class TaikhoannguoigiaohangsController : Controller
    {
        private readonly HeThongBanSachContext _context;

        public TaikhoannguoigiaohangsController(HeThongBanSachContext context)
        {
            _context = context;
        }

        // GET: Taikhoannguoigiaohangs
        public async Task<IActionResult> Index()
        {
            var heThongBanSachContext = _context.Taikhoannguoigiaohangs.Include(t => t.IdnguoiGiaoHangNavigation);
            return View(await heThongBanSachContext.ToListAsync());
        }

        // GET: Taikhoannguoigiaohangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Taikhoannguoigiaohangs == null)
            {
                return NotFound();
            }

            var taikhoannguoigiaohang = await _context.Taikhoannguoigiaohangs
                .Include(t => t.IdnguoiGiaoHangNavigation)
                .FirstOrDefaultAsync(m => m.IdtaiKhoan == id);
            if (taikhoannguoigiaohang == null)
            {
                return NotFound();
            }

            return View(taikhoannguoigiaohang);
        }

        // GET: Taikhoannguoigiaohangs/Create
        public IActionResult Create()
        {
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang");
            return View();
        }

        // POST: Taikhoannguoigiaohangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdtaiKhoan,IdnguoiGiaoHang,TenDangNhap,MatKhau,NgayTaoTaiKhoan")] Taikhoannguoigiaohang taikhoannguoigiaohang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taikhoannguoigiaohang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", taikhoannguoigiaohang.IdnguoiGiaoHang);
            return View(taikhoannguoigiaohang);
        }

        // GET: Taikhoannguoigiaohangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Taikhoannguoigiaohangs == null)
            {
                return NotFound();
            }

            var taikhoannguoigiaohang = await _context.Taikhoannguoigiaohangs.FindAsync(id);
            if (taikhoannguoigiaohang == null)
            {
                return NotFound();
            }
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", taikhoannguoigiaohang.IdnguoiGiaoHang);
            return View(taikhoannguoigiaohang);
        }

        // POST: Taikhoannguoigiaohangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdtaiKhoan,IdnguoiGiaoHang,TenDangNhap,MatKhau,NgayTaoTaiKhoan")] Taikhoannguoigiaohang taikhoannguoigiaohang)
        {
            if (id != taikhoannguoigiaohang.IdtaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taikhoannguoigiaohang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaikhoannguoigiaohangExists(taikhoannguoigiaohang.IdtaiKhoan))
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
            ViewData["IdnguoiGiaoHang"] = new SelectList(_context.Nguoigiaohangs, "IdnguoiGiaoHang", "IdnguoiGiaoHang", taikhoannguoigiaohang.IdnguoiGiaoHang);
            return View(taikhoannguoigiaohang);
        }

        // GET: Taikhoannguoigiaohangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Taikhoannguoigiaohangs == null)
            {
                return NotFound();
            }

            var taikhoannguoigiaohang = await _context.Taikhoannguoigiaohangs
                .Include(t => t.IdnguoiGiaoHangNavigation)
                .FirstOrDefaultAsync(m => m.IdtaiKhoan == id);
            if (taikhoannguoigiaohang == null)
            {
                return NotFound();
            }

            return View(taikhoannguoigiaohang);
        }

        // POST: Taikhoannguoigiaohangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Taikhoannguoigiaohangs == null)
            {
                return Problem("Entity set 'HeThongBanSachContext.Taikhoannguoigiaohangs'  is null.");
            }
            var taikhoannguoigiaohang = await _context.Taikhoannguoigiaohangs.FindAsync(id);
            if (taikhoannguoigiaohang != null)
            {
                _context.Taikhoannguoigiaohangs.Remove(taikhoannguoigiaohang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaikhoannguoigiaohangExists(int id)
        {
          return (_context.Taikhoannguoigiaohangs?.Any(e => e.IdtaiKhoan == id)).GetValueOrDefault();
        }
    }
}
