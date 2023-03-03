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
    public class ApiUserAccountBalanceController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiUserAccountBalanceController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiUserAccountBalance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccountBalance>>> GetUserAccountBalances()
        {
            return await _context.UserAccountBalances.ToListAsync();
        }

        // GET: api/ApiUserAccountBalance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccountBalance>> GetUserAccountBalance(int id)
        {
            var userAccountBalance = await _context.UserAccountBalances.FindAsync(id);

            if (userAccountBalance == null)
            {
                return NotFound();
            }

            return userAccountBalance;
        }

        // PUT: api/ApiUserAccountBalance/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAccountBalance(int id, UserAccountBalance userAccountBalance)
        {
            if (id != userAccountBalance.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAccountBalance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountBalanceExists(id))
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

        // POST: api/ApiUserAccountBalance
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAccountBalance>> PostUserAccountBalance(UserAccountBalance userAccountBalance)
        {
            _context.UserAccountBalances.Add(userAccountBalance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAccountBalance", new { id = userAccountBalance.Id }, userAccountBalance);
        }

        // DELETE: api/ApiUserAccountBalance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAccountBalance(int id)
        {
            var userAccountBalance = await _context.UserAccountBalances.FindAsync(id);
            if (userAccountBalance == null)
            {
                return NotFound();
            }

            _context.UserAccountBalances.Remove(userAccountBalance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAccountBalanceExists(int id)
        {
            return _context.UserAccountBalances.Any(e => e.Id == id);
        }
    }
}
