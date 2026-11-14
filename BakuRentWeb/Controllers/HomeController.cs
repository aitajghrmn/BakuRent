using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakuRentWeb.Data;
using BakuRentWeb.Models;

namespace BakuRentWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Total = await _context.Listings.CountAsync();
            ViewBag.Active = await _context.Listings
                .CountAsync(x => x.Status == "Active");
            ViewBag.Rented = await _context.Listings
                .CountAsync(x => x.Status == "Rented");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
