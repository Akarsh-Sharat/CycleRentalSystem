using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ApiUserPaymentController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiUserPaymentController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiUserPayment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPayment>>> GetUserPayments()
        {
            return await _context.UserPayments.ToListAsync();
        }

        // GET: api/ApiUserPayment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserPaymentSummaryForMobile>>> GetUserPayment(int id)
        {
            //String MobileNumber = "";
            int userAccountID = 0;
            var userAccount = _context.UserAccounts.Where(x=>x.MobileNumber.StartsWith(id.ToString())).FirstOrDefault();
            if (userAccount != null) {
                userAccountID = userAccount.Id;
                //MobileNumber = userAccount.MobileNumber;
            }

            var userPayments = await _context.UserPayments.Where(x => x.UserAccountId == userAccountID).ToListAsync();
            List<UserPaymentForMobile> UserRideInfoForMobiles = new List<UserPaymentForMobile>();

            decimal totalAmount = 0;
            foreach (var item in userPayments)
            {
                var paymentMode = _context.PaymentModes.Where(x => x.Id == item.PaymentModeId).FirstOrDefault();
                String paymentModeName = "GPAY";
                if (paymentMode != null)
                {
                    paymentModeName = paymentMode.PaymentMode1;
                    //MobileNumber = userAccount.MobileNumber;
                }

                UserPaymentForMobile userPaymentForMobile = new UserPaymentForMobile();
                userPaymentForMobile.Id = item.Id;
                userPaymentForMobile.PaymentModeName = paymentModeName;
                totalAmount = totalAmount + item.Amount;
                userPaymentForMobile.Amount = item.Amount.ToString("0.00");

                userPaymentForMobile.PaymentDate = ((DateTime)item.PaymentDate).Date.ToShortDateString();

                if (((DateTime)item.PaymentDate).TimeOfDay.ToString().Contains("00:00:00") == false)
                {
                    userPaymentForMobile.PaymentTime = ((DateTime)item.PaymentDate).TimeOfDay.ToString();
                }
                else {
                    userPaymentForMobile.PaymentTime = "N/A";
                }
                userPaymentForMobile.TransactionId = item.TransactionId;
                userPaymentForMobile.TransactionStatus = item.TransactionStatus;
                UserRideInfoForMobiles.Add(userPaymentForMobile);
            }

            UserPaymentSummaryForMobile userPaymentSummaryForMobile = new UserPaymentSummaryForMobile();
            userPaymentSummaryForMobile.UserPaymentForMobile = UserRideInfoForMobiles;
            userPaymentSummaryForMobile.TotalPaidAmount = totalAmount.ToString("0.00");

            var userRideInfo = await _context.UserRideInfos.Where(x => x.UserAccountId == userAccountID).ToListAsync();
            decimal RideTotalCost = 0;
            foreach (var item in userRideInfo)
            {
                var userRideCost = item.UserRideCosts.Where(x => x.UserRideInfoId == item.Id).FirstOrDefault();
                if (userRideCost != null)
                {
                    RideTotalCost = RideTotalCost + userRideCost.Cost;
                }

            }
            userPaymentSummaryForMobile.TotalRideAmount = RideTotalCost.ToString("0.00");
            userPaymentSummaryForMobile.TotalBalanceAmount = (totalAmount- RideTotalCost).ToString("0.00");



            List<UserPaymentSummaryForMobile> userPaymentSummaryForMobiles = new List<UserPaymentSummaryForMobile>();
            userPaymentSummaryForMobiles.Add(userPaymentSummaryForMobile);

            return userPaymentSummaryForMobiles;
        }

        // PUT: api/ApiUserPayment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPayment(int id, UserPayment userPayment)
        {
            if (id != userPayment.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPaymentExists(id))
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

        // POST: api/ApiUserPayment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPayment>> PostUserPayment(UserPayment userPayment)
        {
            _context.UserPayments.Add(userPayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPayment", new { id = userPayment.Id }, userPayment);
        }

        // DELETE: api/ApiUserPayment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPayment(int id)
        {
            var userPayment = await _context.UserPayments.FindAsync(id);
            if (userPayment == null)
            {
                return NotFound();
            }

            _context.UserPayments.Remove(userPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPaymentExists(int id)
        {
            return _context.UserPayments.Any(e => e.Id == id);
        }
    }
}
