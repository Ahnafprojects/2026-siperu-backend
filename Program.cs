using Microsoft.EntityFrameworkCore;
using SiperuBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Untuk Controllers
builder.Services.AddEndpointsApiExplorer(); // Untuk Swagger
builder.Services.AddSwaggerGen(); // Untuk Swagger UI

// Add Database Service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.MapControllers(); // Routing untuk Controllers

app.Run();

