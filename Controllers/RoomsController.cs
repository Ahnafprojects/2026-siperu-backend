using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiperuBackend.Data;
using SiperuBackend.Models;

namespace SiperuBackend.Controllers;

[Route("api/[controller]")] // URL-nya nanti: /api/rooms
[ApiController] // Ini otomatis mengurus validasi (jika model required kosong, lgsg error 400)
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    // Dependency Injection: Kita "suntik" database ke sini
    public RoomsController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/rooms (Ambil semua data)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        // ToListAsync() biar database gak nge-freeze pas ngambil banyak data
        return await _context.Rooms.ToListAsync();
    }

    // 2. GET: api/rooms/5 (Ambil detail 1 ruangan)
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound(); // Return 404 kalau gak ketemu
        }

        return room;
    }

    // 3. POST: api/rooms (Tambah ruangan baru)
    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(Room room)
    {
        // Tambah ke antrian database
        _context.Rooms.Add(room);
        
        // Simpan perubahan (Commit)
        await _context.SaveChangesAsync();

        // Return 201 Created dan kasih tahu URL detail barangnya
        return CreatedAtAction("GetRoom", new { id = room.Id }, room);
    }

    // 4. PUT: api/rooms/5 (Edit ruangan)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, Room room)
    {
        // Cek apakah ID di URL sama dengan ID di body data
        if (id != room.Id)
        {
            return BadRequest();
        }

        // Tandai data ini sbg "Modified" (diedit)
        _context.Entry(room).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoomExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // Sukses update biasanya return 204 (No Content)
    }

    // 5. DELETE: api/rooms/5 (Hapus ruangan)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        // Hard Delete (Hapus Permanen)
        _context.Rooms.Remove(room);
        
        // Kalau mau Soft Delete (sesuai opsional tugas):
        // room.IsAvailable = false;
        // _context.Entry(room).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper function untuk ngecek data ada atau nggak
    private bool RoomExists(int id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}