using Microsoft.EntityFrameworkCore;
using BakuRentWeb.Models;

namespace BakuRentWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
    }
}
