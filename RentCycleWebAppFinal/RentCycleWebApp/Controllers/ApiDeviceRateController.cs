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
    public class ApiDeviceRateController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiDeviceRateController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiDeviceRate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceRate>>> GetDeviceRates()
        {
            return await _context.DeviceRates.ToListAsync();
        }

        // GET: api/ApiDeviceRate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceRate>> GetDeviceRate(int id)
        {
            var deviceRate = await _context.DeviceRates.FindAsync(id);

            if (deviceRate == null)
            {
                return NotFound();
            }

            return deviceRate;
        }

        // PUT: api/ApiDeviceRate/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceRate(int id, DeviceRate deviceRate)
        {
            if (id != deviceRate.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceRateExists(id))
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

        // POST: api/ApiDeviceRate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceRate>> PostDeviceRate(DeviceRate deviceRate)
        {
            _context.DeviceRates.Add(deviceRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeviceRate", new { id = deviceRate.Id }, deviceRate);
        }

        // DELETE: api/ApiDeviceRate/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceRate(int id)
        {
            var deviceRate = await _context.DeviceRates.FindAsync(id);
            if (deviceRate == null)
            {
                return NotFound();
            }

            _context.DeviceRates.Remove(deviceRate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceRateExists(int id)
        {
            return _context.DeviceRates.Any(e => e.Id == id);
        }
    }
}
