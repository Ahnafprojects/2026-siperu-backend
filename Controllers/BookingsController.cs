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

    // 1. GET: api/bookings (Lihat semua peminjaman + Data Ruangannya)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        // .Include(b => b.Room) itu fungsinya mirip JOIN di SQL.
        // Biar pas ambil data booking, data ruangannya kebawa juga.
        return await _context.Bookings.Include(b => b.Room).ToListAsync();
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