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
    public class ApiDeviceModelController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiDeviceModelController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiDeviceModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceModel>>> GetDeviceModels()
        {
            return await _context.DeviceModels.ToListAsync();
        }

        // GET: api/ApiDeviceModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceModel>> GetDeviceModel(int id)
        {
            var deviceModel = await _context.DeviceModels.FindAsync(id);

            if (deviceModel == null)
            {
                return NotFound();
            }

            return deviceModel;
        }

        // PUT: api/ApiDeviceModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceModel(int id, DeviceModel deviceModel)
        {
            if (id != deviceModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceModelExists(id))
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

        // POST: api/ApiDeviceModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceModel>> PostDeviceModel(DeviceModel deviceModel)
        {
            _context.DeviceModels.Add(deviceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeviceModel", new { id = deviceModel.Id }, deviceModel);
        }

        // DELETE: api/ApiDeviceModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceModel(int id)
        {
            var deviceModel = await _context.DeviceModels.FindAsync(id);
            if (deviceModel == null)
            {
                return NotFound();
            }

            _context.DeviceModels.Remove(deviceModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceModelExists(int id)
        {
            return _context.DeviceModels.Any(e => e.Id == id);
        }
    }
}
