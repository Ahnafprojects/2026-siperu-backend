using Microsoft.EntityFrameworkCore;
using SiperuBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Untuk Controllers
builder.Services.AddEndpointsApiExplorer(); // Untuk Swagger
builder.Services.AddSwaggerGen(); // Untuk Swagger UI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin() // Allow any origin for development
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

app.UseHttpsRedirection();

// Aktifkan Policy CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers(); // Routing untuk Controllers

app.Run();

