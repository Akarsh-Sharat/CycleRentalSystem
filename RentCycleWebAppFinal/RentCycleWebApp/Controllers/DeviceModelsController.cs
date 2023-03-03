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
    public class DeviceModelsController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public DeviceModelsController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: DeviceModels
        public async Task<IActionResult> Index()
        {
              return View(await _context.DeviceModels.ToListAsync());
        }

        // GET: DeviceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DeviceModels == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            return View(deviceModel);
        }

        // GET: DeviceModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeviceModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeviceModel1")] DeviceModel deviceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceModel);
        }

        // GET: DeviceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DeviceModels == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }
            return View(deviceModel);
        }

        // POST: DeviceModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceModel1")] DeviceModel deviceModel)
        {
            if (id != deviceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceModelExists(deviceModel.Id))
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
            return View(deviceModel);
        }

        // GET: DeviceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DeviceModels == null)
            {
                return NotFound();
            }

            var deviceModel = await _context.DeviceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            return View(deviceModel);
        }

        // POST: DeviceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DeviceModels == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.DeviceModels'  is null.");
            }
            var deviceModel = await _context.DeviceModels.FindAsync(id);
            if (deviceModel != null)
            {
                _context.DeviceModels.Remove(deviceModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceModelExists(int id)
        {
          return _context.DeviceModels.Any(e => e.Id == id);
        }
    }
}
