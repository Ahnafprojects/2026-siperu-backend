# Siperu Backend

Backend API untuk sistem peminjaman/booking ruangan (SIPERU). Menyediakan endpoint CRUD untuk data ruangan dan booking, termasuk validasi bentrok jadwal, serta dokumentasi API melalui Swagger.

**Teknologi**
- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core 10
- SQLite
- Swagger / OpenAPI

**Instalasi**
1. Pastikan .NET SDK 10 terpasang.
2. Restore dependency:

```bash
dotnet restore
```

Jika ingin menjalankan migrasi database secara manual:

```bash
dotnet tool install --global dotnet-ef
# atau jika sudah terpasang:
dotnet ef database update
```

**Env**
Salin `.env.example` menjadi `.env` lalu sesuaikan nilai yang dibutuhkan. Jangan commit `.env` ke repository.

Variabel yang dipakai:
- `ASPNETCORE_ENVIRONMENT` (contoh: `Development`)
- `ASPNETCORE_URLS` (contoh: `https://localhost:5001;http://localhost:5000`)
- `ConnectionStrings__DefaultConnection` (contoh: `Data Source=siperu.db`)

**Panduan Menjalankan**
1. Jalankan aplikasi:

```bash
dotnet run
```

2. Akses Swagger UI di:

- `https://localhost:5001/swagger` atau
- `http://localhost:5000/swagger`

3. Endpoint utama:
- `GET /api/rooms`
- `GET /api/bookings`

4. Contoh request siap pakai:
- Lihat file `SiperuBackend.http` untuk contoh request Rooms dan Bookings.

Catatan:
- SQLite database default akan menggunakan file `siperu.db`.
- Seed data untuk Room dan Booking akan masuk saat migrasi dijalankan.
