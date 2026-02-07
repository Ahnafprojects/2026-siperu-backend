using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiperuBackend.Data;
using SiperuBackend.Models;

namespace SiperuBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/bookings (Dengan Fitur Filter & Search)
    // Contoh Request: GET api/bookings?status=Pending&search=Ahnaf
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        // Siapkan Query (Belum dieksekusi ke DB)
        var query = _context.Bookings.Include(b => b.Room).AsQueryable();

        // 1. Filter berdasarkan Status (misal: cuma mau lihat yang 'Pending')
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(b => b.Status.ToLower() == status.ToLower());
        }

        // 2. Filter Search (Cari nama mahasiswa ATAU keperluan)
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b =>
                b.StudentName.Contains(search) ||
                b.Purpose.Contains(search));
        }

        // Eksekusi dan urutkan dari yang terbaru (Descending)
        return await query.OrderByDescending(b => b.StartTime).ToListAsync();
    }

    // 2. GET: api/bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null) return NotFound();

        return booking;
    }

    // 3. POST: api/bookings (Ajukan Peminjaman)
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(Booking booking)
    {
        // Validasi: EndTime gak boleh sebelum StartTime
        if (booking.EndTime <= booking.StartTime)
        {
            return BadRequest("Waktu selesai harus lebih besar dari waktu mulai.");
        }

        // Validasi: Ruangannya ada gak?
        var room = await _context.Rooms.FindAsync(booking.RoomId);
        if (room == null)
        {
            return BadRequest("Ruangan tidak ditemukan.");
        }

        // Set status default
        booking.Status = "Pending";

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
    }

    // 4. PUT: api/bookings/5/status (Fitur Approval Dosen/Admin)
    // Kita bikin endpoint khusus untuk ubah status biar aman
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        // Validasi status cuma boleh 3 jenis
        if (newStatus != "Approved" && newStatus != "Rejected" && newStatus != "Pending")
        {
            return BadRequest("Status tidak valid. Gunakan: Pending, Approved, atau Rejected.");
        }

        booking.Status = newStatus;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // 5. DELETE: api/bookings/5 (Batalkan Peminjaman)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}