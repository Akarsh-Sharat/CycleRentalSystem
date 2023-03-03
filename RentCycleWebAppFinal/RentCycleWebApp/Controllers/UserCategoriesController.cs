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
    public class UserCategoriesController : Controller
    {
        private readonly RentCycleWebAppDBContext _context;

        public UserCategoriesController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: UserCategories
        public async Task<IActionResult> Index()
        {
              return View(await _context.UserCategories.ToListAsync());
        }

        // GET: UserCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserCategories == null)
            {
                return NotFound();
            }

            var userCategory = await _context.UserCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCategory == null)
            {
                return NotFound();
            }

            return View(userCategory);
        }

        // GET: UserCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserCategory1")] UserCategory userCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCategory);
        }

        // GET: UserCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserCategories == null)
            {
                return NotFound();
            }

            var userCategory = await _context.UserCategories.FindAsync(id);
            if (userCategory == null)
            {
                return NotFound();
            }
            return View(userCategory);
        }

        // POST: UserCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserCategory1")] UserCategory userCategory)
        {
            if (id != userCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCategoryExists(userCategory.Id))
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
            return View(userCategory);
        }

        // GET: UserCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserCategories == null)
            {
                return NotFound();
            }

            var userCategory = await _context.UserCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCategory == null)
            {
                return NotFound();
            }

            return View(userCategory);
        }

        // POST: UserCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserCategories == null)
            {
                return Problem("Entity set 'RentCycleWebAppDBContext.UserCategories'  is null.");
            }
            var userCategory = await _context.UserCategories.FindAsync(id);
            if (userCategory != null)
            {
                _context.UserCategories.Remove(userCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCategoryExists(int id)
        {
          return _context.UserCategories.Any(e => e.Id == id);
        }
    }
}
