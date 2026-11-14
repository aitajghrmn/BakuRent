using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakuRentWeb.Data;
using BakuRentWeb.Models;

namespace BakuRentWeb.Controllers
{
    public class ListingsController : Controller
    {
        private readonly AppDbContext _context;

        public ListingsController(AppDbContext context)
        {
            _context = context;
        }

        // List with search, filters and sorting
        public async Task<IActionResult> Index(
            string? search, string? district, string? status, string? sort)
        {
            var query = _context.Listings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Title.Contains(search) || x.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(x => x.District == district);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            query = sort switch
            {
                "price_asc" => query.OrderBy(x => x.Price),
                "price_desc" => query.OrderByDescending(x => x.Price),
                "date_asc" => query.OrderBy(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            ViewBag.Search = search;
            ViewBag.District = district;
            ViewBag.StatusFilter = status;
            ViewBag.Sort = sort;
            ViewBag.TotalCount = await _context.Listings.CountAsync();

            var listings = await query.ToListAsync();
            return View(listings);
        }

        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var listing = await _context.Listings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        // Create (GET)
        public IActionResult Create()
        {
            return View(new Listing());
        }

        // Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing)
        {
            if (!ModelState.IsValid)
            {
                return View(listing);
            }

            listing.Id = 0;
            listing.CreatedAt = DateTime.Now;
            listing.Status = "Active";

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var listing = await _context.Listings.FindAsync(id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Listing listing)
        {
            if (id != listing.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(listing);

            var existing = await _context.Listings.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.Title = listing.Title;
            existing.Description = listing.Description;
            existing.District = listing.District;
            existing.Rooms = listing.Rooms;
            existing.Price = listing.Price;
            existing.Phone = listing.Phone;
            existing.Status = listing.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Quick status toggle: Active <-> Rented
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var listing = await _context.Listings.FindAsync(id);

            if (listing == null)
                return NotFound();

            listing.Status = listing.Status == "Active" ? "Rented" : "Active";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Delete (GET, confirmation page)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var listing = await _context.Listings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        // Delete (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _context.Listings.FindAsync(id);

            if (listing != null)
            {
                _context.Listings.Remove(listing);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
