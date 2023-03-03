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
    public class UserRideCostsController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public UserRideCostsController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: UserRideCosts
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.UserRideCosts.Include(u => u.DeviceRate).Include(u => u.UserRideInfo);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: UserRideCosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRideCosts == null)
            {
                return NotFound();
            }

            var userRideCost = await _context.UserRideCosts
                .Include(u => u.DeviceRate)
                .Include(u => u.UserRideInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRideCost == null)
            {
                return NotFound();
            }

            return View(userRideCost);
        }

        // GET: UserRideCosts/Create
        public IActionResult Create()
        {
            ViewData["DeviceRateId"] = new SelectList(_context.DeviceRates, "Id", "Id");
            ViewData["UserRideInfoId"] = new SelectList(_context.UserRideInfos, "Id", "DeviceId");
            return View();
        }

        // POST: UserRideCosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserRideInfoId,DeviceRateId,Cost")] UserRideCost userRideCost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRideCost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeviceRateId"] = new SelectList(_context.DeviceRates, "Id", "Id", userRideCost.DeviceRateId);
            ViewData["UserRideInfoId"] = new SelectList(_context.UserRideInfos, "Id", "DeviceId", userRideCost.UserRideInfoId);
            return View(userRideCost);
        }

        // GET: UserRideCosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRideCosts == null)
            {
                return NotFound();
            }

            var userRideCost = await _context.UserRideCosts.FindAsync(id);
            if (userRideCost == null)
            {
                return NotFound();
            }
            ViewData["DeviceRateId"] = new SelectList(_context.DeviceRates, "Id", "Id", userRideCost.DeviceRateId);
            ViewData["UserRideInfoId"] = new SelectList(_context.UserRideInfos, "Id", "DeviceId", userRideCost.UserRideInfoId);
            return View(userRideCost);
        }

        // POST: UserRideCosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserRideInfoId,DeviceRateId,Cost")] UserRideCost userRideCost)
        {
            if (id != userRideCost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRideCost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRideCostExists(userRideCost.Id))
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
            ViewData["DeviceRateId"] = new SelectList(_context.DeviceRates, "Id", "Id", userRideCost.DeviceRateId);
            ViewData["UserRideInfoId"] = new SelectList(_context.UserRideInfos, "Id", "DeviceId", userRideCost.UserRideInfoId);
            return View(userRideCost);
        }

        // GET: UserRideCosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRideCosts == null)
            {
                return NotFound();
            }

            var userRideCost = await _context.UserRideCosts
                .Include(u => u.DeviceRate)
                .Include(u => u.UserRideInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRideCost == null)
            {
                return NotFound();
            }

            return View(userRideCost);
        }

        // POST: UserRideCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRideCosts == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.UserRideCosts'  is null.");
            }
            var userRideCost = await _context.UserRideCosts.FindAsync(id);
            if (userRideCost != null)
            {
                _context.UserRideCosts.Remove(userRideCost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRideCostExists(int id)
        {
          return _context.UserRideCosts.Any(e => e.Id == id);
        }
    }
}
