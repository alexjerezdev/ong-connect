using Microsoft.EntityFrameworkCore;
using ONG_connect.Data;
using ONG_connect.Interfaces;
using ONG_connect.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar el DbContext con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();