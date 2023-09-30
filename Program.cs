using Entidades.Models;
using Entidades.Repositories;
using Entidades.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


//builder.Services.AddTransient<IFicheroMuestraService, FicheroMuestraService>();
builder.Services.AddTransient<IMuestraRepository, MuestrasRepository>();
//builder.Services.AddTransient<IMuestraService, MuestraService>();
builder.Services.AddTransient<ICampoRepository, CampoRepository>();
builder.Services.AddTransient<CampoService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();