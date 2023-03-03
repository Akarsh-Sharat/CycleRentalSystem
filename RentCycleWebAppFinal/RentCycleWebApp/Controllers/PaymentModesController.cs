using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCycleWebApp.Data;
using RentCycleWebApp.Models;

namespace RentCycleWebApp.Controllers
{
    public class PaymentModesController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public PaymentModesController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: PaymentModes
        public async Task<IActionResult> Index()
        {
              return View(await _context.PaymentModes.ToListAsync());
        }

        // GET: PaymentModes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaymentModes == null)
            {
                return NotFound();
            }

            var paymentMode = await _context.PaymentModes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMode == null)
            {
                return NotFound();
            }

            return View(paymentMode);
        }

        // GET: PaymentModes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentModes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentMode1")] PaymentMode paymentMode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentMode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentMode);
        }

        // GET: PaymentModes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PaymentModes == null)
            {
                return NotFound();
            }

            var paymentMode = await _context.PaymentModes.FindAsync(id);
            if (paymentMode == null)
            {
                return NotFound();
            }
            return View(paymentMode);
        }

        // POST: PaymentModes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PaymentMode1")] PaymentMode paymentMode)
        {
            if (id != paymentMode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentMode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentModeExists(paymentMode.Id))
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
            return View(paymentMode);
        }

        // GET: PaymentModes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PaymentModes == null)
            {
                return NotFound();
            }

            var paymentMode = await _context.PaymentModes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMode == null)
            {
                return NotFound();
            }

            return View(paymentMode);
        }

        // POST: PaymentModes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PaymentModes == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.PaymentModes'  is null.");
            }
            var paymentMode = await _context.PaymentModes.FindAsync(id);
            if (paymentMode != null)
            {
                _context.PaymentModes.Remove(paymentMode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentModeExists(int id)
        {
          return _context.PaymentModes.Any(e => e.Id == id);
        }
    }
}
