using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCycleWebApp.Data;
using RentCycleWebApp.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.IO;
using System.Drawing.Text;
using System.ComponentModel.Design;

namespace RentCycleWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUserRideInfoController : ControllerBase
    {
        private readonly RentCycleWebAppDBContext _context;

        public ApiUserRideInfoController(RentCycleWebAppDBContext context)
        {
            _context = context;
        }

        // GET: api/ApiUserRideInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRideInfo>>> GetUserRideInfos()
        {

            return await _context.UserRideInfos.ToListAsync();
        }



        // GET: api/ApiUserRideInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserRideInfoForMobile>>> GetUserRideInfo(int id)
        {
            // here id is actually having first 8 digit of mobile number
            int userAccountID = 0;
            var userAccount = _context.UserAccounts.Where(x => x.MobileNumber.StartsWith(id.ToString())).FirstOrDefault();
            if (userAccount != null)
            {
                userAccountID = userAccount.Id;
            }

            if(userAccountID == 0)
            {
                return NoContent();
            }
            List<UserRideInfoForMobile> UserRideInfoForMobiles = new List<UserRideInfoForMobile>();
            var t = await _context.UserRideInfos.Where(x=>x.UserAccountId== userAccountID).ToListAsync();
            foreach (var item in t)
            {

                UserRideInfoForMobile userRideInfoForMobile = new UserRideInfoForMobile();
                userRideInfoForMobile.Id = item.Id;
                userRideInfoForMobile.DeviceId = item.DeviceId;
                userRideInfoForMobile.UserAccountId = item.UserAccountId;
                if (item.ScanStartTime != null)
                {
                    userRideInfoForMobile.RideDate = ((DateTime)item.ScanStartTime).Date.ToShortDateString();
                    try
                    {
                        userRideInfoForMobile.UnlockTime = ((DateTime)item.ScanStartTime).TimeOfDay.ToString("tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        userRideInfoForMobile.UnlockTime = ((DateTime)item.ScanStartTime).TimeOfDay.ToString();
                    }
                }
                if (item.ScanEndTime != null)
                {
                    userRideInfoForMobile.LockTime = ((DateTime)item.ScanEndTime).TimeOfDay.ToString("tt", CultureInfo.InvariantCulture);
                }
                userRideInfoForMobile.StartPosition = item.StartPosition;
                userRideInfoForMobile.EndPosition = item.EndPosition;
                var UserRideCosts = item.UserRideCosts.Where(x => x.UserRideInfoId == item.Id).FirstOrDefault();

                if (UserRideCosts != null)
                {
                    userRideInfoForMobile.RideRate = UserRideCosts.DeviceRateId.ToString() + " /mnt";
                    userRideInfoForMobile.RideCost = UserRideCosts.Cost.ToString();
                }
                else
                {
                    userRideInfoForMobile.RideRate = "N/A /mnt";
                    userRideInfoForMobile.RideCost = "N/A";
                }
                if (item.ScanStartTime != null)
                {
                    try
                    {
                        userRideInfoForMobile.ScanStartTime = ((DateTime)item.ScanStartTime).TimeOfDay.ToString("tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        userRideInfoForMobile.ScanStartTime = ((DateTime)item.ScanStartTime).TimeOfDay.ToString();
                    }
                   
                }
                if (item.ScanEndTime != null)
                {
                    try
                    {
                        userRideInfoForMobile.ScanEndTime = ((DateTime)item.ScanEndTime).TimeOfDay.ToString("tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        userRideInfoForMobile.ScanEndTime = ((DateTime)item.ScanEndTime).TimeOfDay.ToString();
                    }
                }
                if (item.ScanStartTime != null && item.ScanEndTime != null)
                {
                    TimeSpan ts = (TimeSpan)(item.ScanEndTime - item.ScanStartTime);
                    userRideInfoForMobile.RideDuration = ts.TotalMinutes.ToString() + " min";
                }

                UserRideInfoForMobiles.Add(userRideInfoForMobile);
            }

            return UserRideInfoForMobiles.ToList();
        }

        // PUT: api/ApiUserRideInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRideInfo(int id, UserRideInfo userRideInfo)
        {
            if (id != userRideInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(userRideInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRideInfoExists(id))
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

        // POST: api/ApiUserRideInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUserRideInfo(UserRideInfo userRideInfo)
        {
            int userAccountID = 0;
            int mobileNumber = userRideInfo.UserAccountId;
            var userAccount = _context.UserAccounts.Where(x => x.MobileNumber.StartsWith(mobileNumber.ToString())).FirstOrDefault();
            if (userAccount != null)
            {
                userAccountID = userAccount.Id;
                //MobileNumber = userAccount.MobileNumber;
            }

            userRideInfo.UserAccountId = userAccountID;
            userRideInfo.UnlockTime = DateTime.Now.AddMinutes(330);
            userRideInfo.ScanStartTime = DateTime.Now.AddMinutes(330);
            userRideInfo.LockTime = null;
            userRideInfo.ScanEndTime = null;

            _context.UserRideInfos.Add(userRideInfo);
            await _context.SaveChangesAsync();
            bool a = SendCommand(userRideInfo.DeviceId, userAccountID.ToString());

            return (IActionResult)userRideInfo;
        }

            ///////////////////////////////////////////////////////////////////////////
            ///




            ///////////////////////////////////////////////////////////////////////////

            private bool SendCommand(String deviceID, String userAccountID) {

            #region AWS Setup
            try
            {
                var broker = "a2voxmy6jorank-ats.iot.us-east-1.amazonaws.com"; //<AWS-IoT-Endpoint>           
                var port = 8883;
                var clientId = "RENT500001";
                var certPass = "Elutra123$";

                var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

                var caCertPath = Path.Combine(certificatesPath, "RENT500001.cert.pem");
                var pfxPath = Path.Combine(certificatesPath, "rootCA.pfx");


                var caCert = X509Certificate.CreateFromCertFile(caCertPath);

                X509Certificate2 clientCert = new X509Certificate2(pfxPath, certPass);


                // Create a new MQTT client.
                var client = new MqttClient(broker, port, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);


                client.Connect(clientId);


                string topic = "COMMAND-RENT500001";
                JsonObject jo = new JsonObject();
                jo["command"] = "STARTRIDE";
                jo["deviceID"] = deviceID;
                jo["time"] = DateTime.Now.AddMinutes(330).ToString();
                jo["accountid"] = userAccountID;

                string startCommand = jo.ToString();//"STARTRIDE";//
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                client.Publish(topic, Encoding.UTF8.GetBytes(startCommand));
            }
            catch (Exception ex)
            {

            }
            finally
            {
                
            }
            return true;

        }


       

        // DELETE: api/ApiUserRideInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRideInfo(int id)
        {
            var userRideInfo = await _context.UserRideInfos.FindAsync(id);
            if (userRideInfo == null)
            {
                return NotFound();
            }

            _context.UserRideInfos.Remove(userRideInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRideInfoExists(int id)
        {
            return _context.UserRideInfos.Any(e => e.Id == id);
        }
    }
}
#endregion