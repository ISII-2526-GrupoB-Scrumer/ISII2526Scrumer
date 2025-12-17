using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<Car> Car { get; set; } 
    public DbSet<Maintenance> Maintenance { get; set; }

    public DbSet<Rental> Rental { get; set; }
    public DbSet<RentalItem> RentalItem { get; set; }

    public DbSet<Review> Review { get; set; }
    public DbSet<ReviewItem> ReviewItem { get; set; }

    public DbSet<PurchaseItem> PurchaseItem { get; set; }

    public DbSet<Purchase> Purchase { get; set; }

    public DbSet<Booking> Booking { get; set; }
    public DbSet<BookingItem> BookingItem { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }



}
