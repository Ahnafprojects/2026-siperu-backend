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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data untuk Room
        modelBuilder.Entity<Room>().HasData(
            new Room 
            { 
                Id = 1,
                Name = "C-101", 
                Capacity = 30, 
                Description = "Ruang Kelas Lantai 1", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 2,
                Name = "C-102", 
                Capacity = 40, 
                Description = "Ruang Kelas Lantai 1", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 3,
                Name = "Lab Komputer 1", 
                Capacity = 25, 
                Description = "Lab dengan 25 PC", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 4,
                Name = "Aula", 
                Capacity = 200, 
                Description = "Aula untuk acara besar", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 5,
                Name = "Ruang Rapat", 
                Capacity = 15, 
                Description = "Ruang untuk rapat kecil", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 6,
                Name = "Lab Komputer 2", 
                Capacity = 30, 
                Description = "Lab Multimedia", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Room 
            { 
                Id = 7,
                Name = "Ruang Seminar", 
                Capacity = 100, 
                Description = "Untuk seminar dan presentasi", 
                IsAvailable = true,
                CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed data untuk Booking
        modelBuilder.Entity<Booking>().HasData(
            new Booking 
            { 
                Id = 1,
                StudentName = "Ahmad Fauzi", 
                Purpose = "Rapat Himpunan", 
                StartTime = new DateTime(2026, 2, 9, 10, 0, 0), 
                EndTime = new DateTime(2026, 2, 9, 12, 0, 0), 
                RoomId = 1,
                Status = "Approved"
            },
            new Booking 
            { 
                Id = 2,
                StudentName = "Siti Nurhaliza", 
                Purpose = "Workshop Programming", 
                StartTime = new DateTime(2026, 2, 10, 13, 0, 0), 
                EndTime = new DateTime(2026, 2, 10, 16, 0, 0), 
                RoomId = 3,
                Status = "Pending"
            },
            new Booking 
            { 
                Id = 3,
                StudentName = "Budi Santoso", 
                Purpose = "Seminar Mahasiswa", 
                StartTime = new DateTime(2026, 2, 11, 9, 0, 0), 
                EndTime = new DateTime(2026, 2, 11, 11, 0, 0), 
                RoomId = 4,
                Status = "Approved"
            }
        );
    }
}