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
    public class UserRideInfoesController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public UserRideInfoesController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: UserRideInfoes
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.UserRideInfos.Include(u => u.UserAccount);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: UserRideInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRideInfos == null)
            {
                return NotFound();
            }

            var userRideInfo = await _context.UserRideInfos
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRideInfo == null)
            {
                return NotFound();
            }

            return View(userRideInfo);
        }

        // GET: UserRideInfoes/Create
        public IActionResult Create()
        {
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber");
            return View();
        }

        // POST: UserRideInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserAccountId,DeviceId,ScanStartTime,ScanEndTime,UnlockTime,LockTime,StartPosition,EndPosition")] UserRideInfo userRideInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRideInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userRideInfo.UserAccountId);
            return View(userRideInfo);
        }

        // GET: UserRideInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRideInfos == null)
            {
                return NotFound();
            }

            var userRideInfo = await _context.UserRideInfos.FindAsync(id);
            if (userRideInfo == null)
            {
                return NotFound();
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userRideInfo.UserAccountId);
            return View(userRideInfo);
        }

        // POST: UserRideInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserAccountId,DeviceId,ScanStartTime,ScanEndTime,UnlockTime,LockTime,StartPosition,EndPosition")] UserRideInfo userRideInfo)
        {
            if (id != userRideInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRideInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRideInfoExists(userRideInfo.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "MobileNumber", userRideInfo.UserAccountId);
            return View(userRideInfo);
        }

        // GET: UserRideInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRideInfos == null)
            {
                return NotFound();
            }

            var userRideInfo = await _context.UserRideInfos
                .Include(u => u.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRideInfo == null)
            {
                return NotFound();
            }

            return View(userRideInfo);
        }

        // POST: UserRideInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRideInfos == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.UserRideInfos'  is null.");
            }
            var userRideInfo = await _context.UserRideInfos.FindAsync(id);
            if (userRideInfo != null)
            {
                _context.UserRideInfos.Remove(userRideInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRideInfoExists(int id)
        {
          return _context.UserRideInfos.Any(e => e.Id == id);
        }
    }
}
