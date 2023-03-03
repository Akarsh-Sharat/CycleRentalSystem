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
    public class UserAccountBalancesController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public UserAccountBalancesController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: UserAccountBalances
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.UserAccountBalances.Include(u => u.UserAccount);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: UserAccountBalances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserAccountBalances == null)
            {
                return NotFound();
            }

            var userAccountBalance = await _context.UserAccountBalances
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccountBalance == null)
            {
                return NotFound();
            }

            return View(userAccountBalance);
        }

        // GET: UserAccountBalances/Create
        public IActionResult Create()
        {
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber");
            return View();
        }

        // POST: UserAccountBalances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserAccountId,BalanceAmount")] UserAccountBalance userAccountBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAccountBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userAccountBalance.UserAccountId);
            return View(userAccountBalance);
        }

        // GET: UserAccountBalances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserAccountBalances == null)
            {
                return NotFound();
            }

            var userAccountBalance = await _context.UserAccountBalances.FindAsync(id);
            if (userAccountBalance == null)
            {
                return NotFound();
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userAccountBalance.UserAccountId);
            return View(userAccountBalance);
        }

        // POST: UserAccountBalances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserAccountId,BalanceAmount")] UserAccountBalance userAccountBalance)
        {
            if (id != userAccountBalance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccountBalance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountBalanceExists(userAccountBalance.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userAccountBalance.UserAccountId);
            return View(userAccountBalance);
        }

        // GET: UserAccountBalances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserAccountBalances == null)
            {
                return NotFound();
            }

            var userAccountBalance = await _context.UserAccountBalances
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAccountBalance == null)
            {
                return NotFound();
            }

            return View(userAccountBalance);
        }

        // POST: UserAccountBalances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserAccountBalances == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.UserAccountBalances'  is null.");
            }
            var userAccountBalance = await _context.UserAccountBalances.FindAsync(id);
            if (userAccountBalance != null)
            {
                _context.UserAccountBalances.Remove(userAccountBalance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountBalanceExists(int id)
        {
          return _context.UserAccountBalances.Any(e => e.Id == id);
        }
    }
}
