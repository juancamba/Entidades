using Entidades;
using Entidades.Models;
using Entidades.Repositories;
using Entidades.Serivces.Ficheros;
using Entidades.Services;
using Entidades.Services.Ficheros;
using Microsoft.EntityFrameworkCore;
using Entidades;
using System;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IFicheroMuestraService, FicheroMuestraService>();
builder.Services.AddScoped<IFicheroEntidadService, FicheroEntidadService>();
builder.Services.AddScoped<IMuestraRepository, MuestraRepository>();
builder.Services.AddScoped<IEntidadesRepository, EntidadesRepository>();
builder.Services.AddScoped<ICampoRepository, CampoRepository>();
builder.Services.AddScoped<ITipoMuestraRepository, TipoMuestraRepository>();
builder.Services.AddScoped<IValoresReferenciaRepository, ValoresReferenciaRepository>();
builder.Services.AddScoped<CampoService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
Preparedb.Population(app);
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
