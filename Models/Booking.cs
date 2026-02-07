using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Perlu ini untuk ForeignKey

namespace SiperuBackend.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string StudentName { get; set; } = string.Empty; // Nama Peminjam

    [Required]
    public string Purpose { get; set; } = string.Empty; // Keperluan (misal: Rapat Hima)

    [Required]
    public DateTime StartTime { get; set; } // Mulai pinjam

    [Required]
    public DateTime EndTime { get; set; } // Selesai pinjam

    // --- RELASI KE TABEL ROOM ---
    [Required]
    public int RoomId { get; set; } // Foreign Key (Menyimpan ID Ruangan)

    // Navigation Property (Biar bisa akses detail ruangannya, misal: booking.Room.Name)
    [ForeignKey("RoomId")]
    public Room? Room { get; set; }

    // --- STATUS PEMINJAMAN ---
    // Kita pakai string dulu biar simpel (Pending, Approved, Rejected)
    public string Status { get; set; } = "Pending"; 
}