# SDD Backend - Siperu

## Ringkasan
Backend menyediakan API untuk manajemen `Room` (ruangan) dan `Booking` (peminjaman ruangan). Teknologi: ASP.NET Core Web API + Entity Framework Core.

## Base URL
- Default routing: `/{host}/api/{controller}`
- Endpoint yang tersedia:
  - `api/rooms`
  - `api/bookings`

## Autentikasi
- Tidak ada autentikasi/otorisasi di kode saat ini.

## Endpoints

### Rooms

#### 1. GET `api/rooms`
Ambil seluruh data ruangan.
- Request body: tidak ada
- Query params: tidak ada
- Response: `200 OK` + array `Room`

**Contoh response**
```json
[
  {
    "id": 1,
    "name": "C-101",
    "capacity": 30,
    "description": "Ruang Kelas Lantai 1",
    "isAvailable": true,
    "createdAt": "2026-02-08T00:00:00Z"
  }
]
```

#### 2. GET `api/rooms/{id}`
Ambil detail 1 ruangan.
- Path params: `id` (int)
- Response:
  - `200 OK` + `Room`
  - `404 Not Found` jika tidak ada

#### 3. POST `api/rooms`
Tambah ruangan baru.
- Request body: `Room` (tanpa `id` bisa, akan dibuat otomatis oleh DB)
- Response:
  - `201 Created` + `Room`
  - `400 Bad Request` jika validasi gagal

**Contoh request**
```json
{
  "name": "Lab Bahasa",
  "capacity": 20,
  "description": "Lab audio",
  "isAvailable": true
}
```

#### 4. PUT `api/rooms/{id}`
Update data ruangan (full replace).
- Path params: `id` (int)
- Request body: `Room` (field `id` harus sama dengan `id` di URL)
- Response:
  - `204 No Content` jika sukses
  - `400 Bad Request` jika `id` tidak cocok
  - `404 Not Found` jika tidak ada

#### 5. DELETE `api/rooms/{id}`
Hapus ruangan (hard delete).
- Path params: `id` (int)
- Response:
  - `204 No Content` jika sukses
  - `404 Not Found` jika tidak ada

---

### Bookings

#### 1. GET `api/bookings`
Ambil seluruh data booking (termasuk `Room`). Mendukung filter.
- Query params:
  - `status` (string, opsional): filter status (case-insensitive)
  - `search` (string, opsional): cari di `studentName` atau `purpose`
- Response:
  - `200 OK` + array `Booking`
- Sorting: `StartTime` desc

**Contoh request**
```
GET /api/bookings?status=approved&search=rapat
```

**Contoh response**
```json
[
  {
    "id": 1,
    "studentName": "Ahmad Fauzi",
    "purpose": "Rapat Himpunan",
    "startTime": "2026-02-09T10:00:00",
    "endTime": "2026-02-09T12:00:00",
    "roomId": 1,
    "status": "Approved",
    "room": {
      "id": 1,
      "name": "C-101",
      "capacity": 30,
      "description": "Ruang Kelas Lantai 1",
      "isAvailable": true,
      "createdAt": "2026-02-08T00:00:00Z"
    }
  }
]
```

#### 2. GET `api/bookings/{id}`
Ambil detail 1 booking (termasuk `Room`).
- Path params: `id` (int)
- Response:
  - `200 OK` + `Booking`
  - `404 Not Found` jika tidak ada

#### 3. POST `api/bookings`
Buat booking baru. Status akan dipaksa menjadi `Pending`.
- Request body: `Booking`
- Validasi:
  - `EndTime` harus > `StartTime`
  - `RoomId` harus ada
  - Tidak boleh bentrok dengan booking lain yang `Approved` pada rentang waktu yang sama
- Response:
  - `201 Created` + `Booking`
  - `400 Bad Request` jika validasi gagal

**Contoh request**
```json
{
  "studentName": "Dina",
  "purpose": "Diskusi proyek",
  "startTime": "2026-02-12T09:00:00",
  "endTime": "2026-02-12T11:00:00",
  "roomId": 2
}
```

#### 4. PUT `api/bookings/{id}/status`
Update status booking.
- Path params: `id` (int)
- Request body: string status **JSON string**, contoh: `"Approved"`
- Status valid: `Pending`, `Approved`, `Rejected`
- Jika `Approved`, akan dicek bentrok dengan booking lain yang `Approved`
- Response:
  - `204 No Content` jika sukses
  - `400 Bad Request` jika status invalid atau bentrok
  - `404 Not Found` jika booking tidak ada

**Contoh request**
```json
"Approved"
```

#### 5. DELETE `api/bookings/{id}`
Hapus booking (hard delete).
- Path params: `id` (int)
- Response:
  - `204 No Content` jika sukses
  - `404 Not Found` jika tidak ada

## Skema Data

### Tabel: Rooms
| Field | Type | Required | Default | Keterangan |
|---|---|---|---|---|
| `Id` | int | Ya (PK) | Auto | Primary key |
| `Name` | string (max 100) | Ya | "" | Nama ruangan |
| `Capacity` | int | Ya | 0 | Kapasitas ruangan |
| `Description` | string | Tidak | null | Deskripsi |
| `IsAvailable` | bool | Tidak | true | Status aktif/tidak |
| `CreatedAt` | datetime | Tidak | `DateTime.UtcNow` | Waktu dibuat |

### Tabel: Bookings
| Field | Type | Required | Default | Keterangan |
|---|---|---|---|---|
| `Id` | int | Ya (PK) | Auto | Primary key |
| `StudentName` | string | Ya | "" | Nama peminjam |
| `Purpose` | string | Ya | "" | Keperluan |
| `StartTime` | datetime | Ya | - | Mulai pinjam |
| `EndTime` | datetime | Ya | - | Selesai pinjam |
| `RoomId` | int | Ya (FK) | - | Relasi ke `Rooms.Id` |
| `Status` | string | Tidak | "Pending" | Status booking |
| `Room` | object | Tidak | null | Navigation property (join ke Room) |

## Catatan Validasi dan Aturan Bisnis
- Booking bentrok jika:
  - `start < existing.EndTime` **dan** `end > existing.StartTime`
  - dan booking existing berstatus `Approved`
- Booking `Approved` tidak boleh overlap dengan booking `Approved` lain.
- Saat create booking, status selalu dipaksa `Pending`.

