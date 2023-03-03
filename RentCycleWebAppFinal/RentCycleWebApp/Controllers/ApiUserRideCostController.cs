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
    public class ApiUserRideCostController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiUserRideCostController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiUserRideCost
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRideCost>>> GetUserRideCosts()
        {
            return await _context.UserRideCosts.ToListAsync();
        }

        // GET: api/ApiUserRideCost/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRideCost>> GetUserRideCost(int id)
        {
            var userRideCost = await _context.UserRideCosts.FindAsync(id);

            if (userRideCost == null)
            {
                return NotFound();
            }

            return userRideCost;
        }

        // PUT: api/ApiUserRideCost/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRideCost(int id, UserRideCost userRideCost)
        {
            if (id != userRideCost.Id)
            {
                return BadRequest();
            }

            _context.Entry(userRideCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRideCostExists(id))
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

        // POST: api/ApiUserRideCost
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRideCost>> PostUserRideCost(UserRideCost userRideCost)
        {
            _context.UserRideCosts.Add(userRideCost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRideCost", new { id = userRideCost.Id }, userRideCost);
        }

        // DELETE: api/ApiUserRideCost/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRideCost(int id)
        {
            var userRideCost = await _context.UserRideCosts.FindAsync(id);
            if (userRideCost == null)
            {
                return NotFound();
            }

            _context.UserRideCosts.Remove(userRideCost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRideCostExists(int id)
        {
            return _context.UserRideCosts.Any(e => e.Id == id);
        }
    }
}
