using Microsoft.EntityFrameworkCore;
using BakuRentWeb.Data;
using BakuRentWeb.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();

    // Seed demo data only if the table is empty
    if (!db.Listings.Any())
    {
        db.Listings.AddRange(
            new Listing
            {
                Title = "Nərimanov metrosuna yaxın 2 otaqlı mənzil",
                Description = "Təmirli, əşyalı, kombi var. Metroya 5 dəqiqəlik piyada yol.",
                District = "Nərimanov",
                Rooms = 2,
                Price = 750,
                Phone = "+994501234567",
                Status = "Active",
                CreatedAt = DateTime.Now.AddDays(-3)
            },
            new Listing
            {
                Title = "Yasamalda 3 otaqlı geniş mənzil",
                Description = "Yeni tikili, lift var, tam təmirli. Ailəyə verilir.",
                District = "Yasamal",
                Rooms = 3,
                Price = 1100,
                Phone = "+994555556677",
                Status = "Active",
                CreatedAt = DateTime.Now.AddDays(-2)
            },
            new Listing
            {
                Title = "Səbail mərkəzdə studiya",
                Description = "Tələbələr və təkcə yaşayanlar üçün, internet daxildir.",
                District = "Səbail",
                Rooms = 1,
                Price = 500,
                Phone = "+994703334455",
                Status = "Rented",
                CreatedAt = DateTime.Now.AddDays(-1)
            });

        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
