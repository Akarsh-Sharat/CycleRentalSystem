using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCycleWebApp.Data;
using RentCycleWebApp.Models;

namespace RentCycleWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUserCategoryController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiUserCategoryController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiUserCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCategory>>> GetUserCategories()
        {
            return await _context.UserCategories.ToListAsync();
        }

        // GET: api/ApiUserCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCategory>> GetUserCategory(int id)
        {
            var userCategory = await _context.UserCategories.FindAsync(id);

            if (userCategory == null)
            {
                return NotFound();
            }

            return userCategory;
        }

        // PUT: api/ApiUserCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCategory(int id, UserCategory userCategory)
        {
            if (id != userCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(userCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiUserCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCategory>> PostUserCategory(UserCategory userCategory)
        {
            _context.UserCategories.Add(userCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserCategory", new { id = userCategory.Id }, userCategory);
        }

        // DELETE: api/ApiUserCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCategory(int id)
        {
            var userCategory = await _context.UserCategories.FindAsync(id);
            if (userCategory == null)
            {
                return NotFound();
            }

            _context.UserCategories.Remove(userCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCategoryExists(int id)
        {
            return _context.UserCategories.Any(e => e.Id == id);
        }
    }
}
