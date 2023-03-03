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
    public class UserPaymentsController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public UserPaymentsController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: UserPayments
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.UserPayments.Include(u => u.PaymentMode).Include(u => u.UserAccount);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: UserPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserPayments == null)
            {
                return NotFound();
            }

            var userPayment = await _context.UserPayments
                .Include(u => u.PaymentMode)
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPayment == null)
            {
                return NotFound();
            }

            return View(userPayment);
        }

        // GET: UserPayments/Create
        public IActionResult Create()
        {
            ViewData["PaymentModeId"] = new SelectList(_context.PaymentModes, "Id", "PaymentMode1");
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber");
            return View();
        }

        // POST: UserPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserAccountId,Amount,PaymentModeId,PaymentDate")] UserPayment userPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentModeId"] = new SelectList(_context.PaymentModes, "Id", "PaymentMode1", userPayment.PaymentModeId);
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userPayment.UserAccountId);
            return View(userPayment);
        }

        // GET: UserPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserPayments == null)
            {
                return NotFound();
            }

            var userPayment = await _context.UserPayments.FindAsync(id);
            if (userPayment == null)
            {
                return NotFound();
            }
            ViewData["PaymentModeId"] = new SelectList(_context.PaymentModes, "Id", "PaymentMode1", userPayment.PaymentModeId);
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userPayment.UserAccountId);
            return View(userPayment);
        }

        // POST: UserPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserAccountId,Amount,PaymentModeId,PaymentDate")] UserPayment userPayment)
        {
            if (id != userPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPaymentExists(userPayment.Id))
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
            ViewData["PaymentModeId"] = new SelectList(_context.PaymentModes, "Id", "PaymentMode1", userPayment.PaymentModeId);
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userPayment.UserAccountId);
            return View(userPayment);
        }

        // GET: UserPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserPayments == null)
            {
                return NotFound();
            }

            var userPayment = await _context.UserPayments
                .Include(u => u.PaymentMode)
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPayment == null)
            {
                return NotFound();
            }

            return View(userPayment);
        }

        // POST: UserPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserPayments == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.UserPayments'  is null.");
            }
            var userPayment = await _context.UserPayments.FindAsync(id);
            if (userPayment != null)
            {
                _context.UserPayments.Remove(userPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPaymentExists(int id)
        {
          return _context.UserPayments.Any(e => e.Id == id);
        }
    }
}
