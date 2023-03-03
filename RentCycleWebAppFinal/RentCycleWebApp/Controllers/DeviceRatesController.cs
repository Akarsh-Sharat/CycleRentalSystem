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
    public class DeviceRatesController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public DeviceRatesController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: DeviceRates
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.DeviceRates.Include(d => d.DeviceModel).Include(d => d.UserCategory);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: DeviceRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DeviceRates == null)
            {
                return NotFound();
            }

            var deviceRate = await _context.DeviceRates
                .Include(d => d.DeviceModel)
                .Include(d => d.UserCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceRate == null)
            {
                return NotFound();
            }

            return View(deviceRate);
        }

        // GET: DeviceRates/Create
        public IActionResult Create()
        {
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1");
            ViewData["UserCategoryId"] = new SelectList(_context.UserCategories, "Id", "UserCategory1");
            return View();
        }

        // POST: DeviceRates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeviceModelId,UserCategoryId,Rate,StartTime,EndTime,EffectiveStartDate,EffectiveEndDate")] DeviceRate deviceRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceRate.DeviceModelId);
            ViewData["UserCategoryId"] = new SelectList(_context.UserCategories, "Id", "UserCategory1", deviceRate.UserCategoryId);
            return View(deviceRate);
        }

        // GET: DeviceRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DeviceRates == null)
            {
                return NotFound();
            }

            var deviceRate = await _context.DeviceRates.FindAsync(id);
            if (deviceRate == null)
            {
                return NotFound();
            }
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceRate.DeviceModelId);
            ViewData["UserCategoryId"] = new SelectList(_context.UserCategories, "Id", "UserCategory1", deviceRate.UserCategoryId);
            return View(deviceRate);
        }

        // POST: DeviceRates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceModelId,UserCategoryId,Rate,StartTime,EndTime,EffectiveStartDate,EffectiveEndDate")] DeviceRate deviceRate)
        {
            if (id != deviceRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceRateExists(deviceRate.Id))
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
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceRate.DeviceModelId);
            ViewData["UserCategoryId"] = new SelectList(_context.UserCategories, "Id", "UserCategory1", deviceRate.UserCategoryId);
            return View(deviceRate);
        }

        // GET: DeviceRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DeviceRates == null)
            {
                return NotFound();
            }

            var deviceRate = await _context.DeviceRates
                .Include(d => d.DeviceModel)
                .Include(d => d.UserCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceRate == null)
            {
                return NotFound();
            }

            return View(deviceRate);
        }

        // POST: DeviceRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DeviceRates == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.DeviceRates'  is null.");
            }
            var deviceRate = await _context.DeviceRates.FindAsync(id);
            if (deviceRate != null)
            {
                _context.DeviceRates.Remove(deviceRate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceRateExists(int id)
        {
          return _context.DeviceRates.Any(e => e.Id == id);
        }
    }
}
