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

    // GET methods (Tetap sama, tidak berubah)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var query = _context.Bookings.Include(b => b.Room).AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(b => b.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b =>
                b.StudentName.Contains(search) ||
                b.Purpose.Contains(search));
        }

        return await query.OrderByDescending(b => b.StartTime).ToListAsync();
    }

    [HttpGet("{id}")] // cihuy
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null) return NotFound();

        return booking;
    }

    // --- REVISI POST (CREATE) ---
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(Booking booking)
    {
        // --- 1. VALIDASI MASA LALU ---
        if (booking.StartTime < DateTime.Now)
        {
            return BadRequest("Gagal: Tidak bisa meminjam ruangan di waktu yang sudah lewat.");
        }

        // --- 2. VALIDASI DURASI ---
        if (booking.EndTime <= booking.StartTime)
        {
            return BadRequest("Waktu selesai harus lebih besar dari waktu mulai.");
        }

        // --- 3. VALIDASI BENTROK / DOUBLE BOOKING ---
        // Cek apakah ada booking lain di Ruangan yg sama DAN Statusnya Approved
        var collision = _context.Bookings.Any(b => 
            b.RoomId == booking.RoomId &&
            b.Status == "Approved" && // Cuma cek yang sudah disetujui
            (
                (booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)
            )
        );

        if (collision)
        {
            return BadRequest("Gagal: Ruangan sudah terpakai di jam tersebut.");
        }

        // --- 4. SET DEFAULT STATUS ---
        booking.Status = "Pending"; // Default status harus Pending
        
        // --- 5. SIMPAN KE DATABASE ---
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
    }

    // --- REVISI PUT (UPDATE STATUS) ---
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

<<<<<<< HEAD
        if (newStatus != "Approved" && newStatus != "Rejected" && newStatus != "Pending" && newStatus != "Cancelled")
=======
        if (newStatus != "Approved" &&
    newStatus != "Rejected" &&
    newStatus != "Pending" &&
    newStatus != "Cancelled")
>>>>>>> origin/develop
        {
            return BadRequest("Status tidak valid. Opsi: Approved, Rejected, Pending, Cancelled");
        }

        // LOGIKA BARU: Jika Admin mau meng-Approve, cek dulu bentrok gak?
        igit checkout fix/booking-collision-logic
git pull origin developf (newStatus == "Approved")
        {
            // Cek bentrok dengan booking LAIN (selain diri sendiri)
            if (await IsRoomBooked(booking.RoomId, booking.StartTime, booking.EndTime, booking.Id))
            {
                return BadRequest("Gagal Approve: Ruangan sudah dipakai booking lain di jam ini.");
            }
        }

        booking.Status = newStatus;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // --- FUNGSI POLISI LALU LINTAS (HELPER) ---
    private async Task<bool> IsRoomBooked(int roomId, DateTime start, DateTime end, int? excludeBookingId = null)
    {
        return await _context.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status == "Approved" && // Cuma peduli sama yang udah Approved
            b.Id != excludeBookingId && // Jangan cek bentrok sama diri sendiri
            (start < b.EndTime && end > b.StartTime) // Rumus Matematika Bentrok
        );
    }
}