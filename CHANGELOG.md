# Changelog

Semua perubahan penting akan dicatat di file ini.

## [v1.0.0] - 2026-02-07
### Added
- API Rooms (CRUD) dengan endpoint `/api/rooms`.
- API Bookings (CRUD + update status) dengan validasi bentrok jadwal pada status Approved.
- Database SQLite dengan Entity Framework Core dan migrasi awal.
- Seed data untuk Rooms dan Bookings.
- Dokumentasi API via Swagger (OpenAPI).
- Dokumentasi proyek: `README.md` dan `.env.example`.
- Contoh request koleksi API di `SiperuBackend.http`.

## [v1.0.1] - 2026-02-07
### Added
- Workflow CI (GitHub Actions) untuk build.
- Badge CI pada `README.md`.

## [v1.0.2] - 2026-02-10
### Fixed
- Prevent bookings with past start time via API validation.
- Fix booking collision logic to prevent overlapping Approved bookings.
- Remove merge conflict markers and corrupted text in `BookingsController`.
- Resolve multiple merge conflicts with `develop` branch.
- Remove binary database files accidentally committed to repository.

### Changed
- Recreate database schema to fully sync with current EF Core models.
- Add new migration to reflect updated model structure.

### Chore
- Remove `bin/` and `obj/` folders from Git tracking.
- Add/update `.gitignore` to prevent binary and build artifacts from being committed.
- Cleanup repository to reduce bloat and avoid future merge conflicts.
