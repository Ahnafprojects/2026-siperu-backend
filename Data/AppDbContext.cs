using Microsoft.EntityFrameworkCore;
using SiperuBackend.Models;

namespace SiperuBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Daftarkan tabel Room di sini
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
}