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
    public class DeviceShadowsController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public DeviceShadowsController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: DeviceShadows
        public async Task<IActionResult> Index()
        {
            var rentCycleWebAppDBContext = _context.DeviceShadows.Include(d => d.DeviceModel);
            return View(await rentCycleWebAppDBContext.ToListAsync());
        }

        // GET: DeviceShadows/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DeviceShadows == null)
            {
                return NotFound();
            }

            var deviceShadow = await _context.DeviceShadows
                .Include(d => d.DeviceModel)
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (deviceShadow == null)
            {
                return NotFound();
            }

            return View(deviceShadow);
        }

        // GET: DeviceShadows/Create
        public IActionResult Create()
        {
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1");
            return View();
        }

        // POST: DeviceShadows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,Location,Status,DeviceModelId,Info,InOperationFrom,LastServicingDate,UserRating")] DeviceShadow deviceShadow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceShadow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceShadow.DeviceModelId);
            return View(deviceShadow);
        }

        // GET: DeviceShadows/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DeviceShadows == null)
            {
                return NotFound();
            }

            var deviceShadow = await _context.DeviceShadows.FindAsync(id);
            if (deviceShadow == null)
            {
                return NotFound();
            }
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceShadow.DeviceModelId);
            return View(deviceShadow);
        }

        // POST: DeviceShadows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DeviceId,Location,Status,DeviceModelId,Info,InOperationFrom,LastServicingDate,UserRating")] DeviceShadow deviceShadow)
        {
            if (id != deviceShadow.DeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceShadow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceShadowExists(deviceShadow.DeviceId))
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
            ViewData["DeviceModelId"] = new SelectList(_context.DeviceModels, "Id", "DeviceModel1", deviceShadow.DeviceModelId);
            return View(deviceShadow);
        }

        // GET: DeviceShadows/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DeviceShadows == null)
            {
                return NotFound();
            }

            var deviceShadow = await _context.DeviceShadows
                .Include(d => d.DeviceModel)
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (deviceShadow == null)
            {
                return NotFound();
            }

            return View(deviceShadow);
        }

        // POST: DeviceShadows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DeviceShadows == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.DeviceShadows'  is null.");
            }
            var deviceShadow = await _context.DeviceShadows.FindAsync(id);
            if (deviceShadow != null)
            {
                _context.DeviceShadows.Remove(deviceShadow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceShadowExists(string id)
        {
          return _context.DeviceShadows.Any(e => e.DeviceId == id);
        }
    }
}
