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
    public class ApiDeviceShadowController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiDeviceShadowController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiDeviceShadow
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceShadow>>> GetDeviceShadows()
        {
            return await _context.DeviceShadows.ToListAsync();
        }

        // GET: api/ApiDeviceShadow/5
        [HttpGet("{id}")]
        public ActionResult<DeviceShadowForMobile> GetDeviceShadow(int id)
        {
            String sId = id.ToString();
            String MobileNumber = sId.Substring(0,4);
            String DeviceId = sId.Substring(4, sId.Length-4);
            String userCategoryId = "1";

            DeviceShadowForMobile deviceShadowForMobile = new DeviceShadowForMobile();

            

            var deviceShadow = _context.DeviceShadows.Where(x=>x.DeviceId.EndsWith(DeviceId)).FirstOrDefault();
            var userAccount = _context.UserAccounts.Where(x => x.MobileNumber.StartsWith(MobileNumber)).FirstOrDefault();
          
            if(userAccount != null)
            {
                if (userAccount.UsercategoryId != null)
                {
                    userCategoryId = userAccount.UsercategoryId+"";
                }
            }

            

            if (deviceShadow == null)
            {
                return new DeviceShadowForMobile();
            }
            else {

                int hr = DateTime.Now.Hour;
                var deviceRate = _context.DeviceRates.Where(x => x.UserCategoryId.ToString() == userCategoryId && x.DeviceModelId == deviceShadow.DeviceModelId && x.StartTime >=hr && x.EndTime<=hr && x.EffectiveStartDate<=DateTime.Now && x.EffectiveEndDate>=DateTime.Now).FirstOrDefault();

                var deviceModel = _context.DeviceModels.Where(x => x.Id== deviceShadow.DeviceModelId).FirstOrDefault();

                if(deviceModel != null)
                {
                    deviceShadowForMobile.DeviceModelName = deviceModel.DeviceModel1;
                }
                else
                {
                    deviceShadowForMobile.DeviceModelName = "N/A";
                }
                deviceShadowForMobile.DeviceId = deviceShadow.DeviceId;
                deviceShadowForMobile.Status = deviceShadow.Status;
                deviceShadowForMobile.Location = deviceShadow.Location;
                deviceShadowForMobile.Info=deviceShadow.Info;
                deviceShadowForMobile.UserRating = deviceShadow.UserRating.ToString();
                if (deviceRate != null)
                {
                    deviceShadowForMobile.Rate = deviceRate.Rate.ToString() + " /mnt";
                }
                else {
                    deviceShadowForMobile.Rate = "5 / mnt";
                }


            }

            return deviceShadowForMobile;
        }

        // PUT: api/ApiDeviceShadow/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceShadow(string id, DeviceShadow deviceShadow)
        {
            if (id != deviceShadow.DeviceId)
            {
                return BadRequest();
            }

            _context.Entry(deviceShadow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceShadowExists(id))
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

        // POST: api/ApiDeviceShadow
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceShadow>> PostDeviceShadow(DeviceShadow deviceShadow)
        {
            _context.DeviceShadows.Add(deviceShadow);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceShadowExists(deviceShadow.DeviceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDeviceShadow", new { id = deviceShadow.DeviceId }, deviceShadow);
        }

        // DELETE: api/ApiDeviceShadow/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceShadow(string id)
        {
            var deviceShadow = await _context.DeviceShadows.FindAsync(id);
            if (deviceShadow == null)
            {
                return NotFound();
            }

            _context.DeviceShadows.Remove(deviceShadow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceShadowExists(string id)
        {
            return _context.DeviceShadows.Any(e => e.DeviceId == id);
        }
    }
}
