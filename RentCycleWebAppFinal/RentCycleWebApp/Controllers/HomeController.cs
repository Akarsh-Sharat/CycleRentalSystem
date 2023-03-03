using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCycleWebApp.Data;
using RentCycleWebApp.Models;
using System.Diagnostics;

namespace RentCycleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RentCycleWebAppDBContext _context;
        public HomeController(ILogger<HomeController> logger, RentCycleWebAppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
           // return RedirectToAction("Identity/Account/Login");
          return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}