using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiperuBackend.Models;

public class Room
{
    [Key]
    public int Id { get; set; }

    [Required] // Validasi: Wajib isi
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty; // Nama Ruangan (misal: C-101)

    [Required]
    public int Capacity { get; set; } // Kapasitas (misal: 30 orang)

    public string? Description { get; set; } // Deskripsi (Boleh kosong/nullable)

    public bool IsAvailable { get; set; } = true; // Status aktif/tidak

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}