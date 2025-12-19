using AlquilerAuto.DAO;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Service;
using AlquilerAuto.Service.ServiceImpl;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Modena PE-Peru
var cultureInfo = new CultureInfo("es-PE");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICliente, ClienteDAO>();
builder.Services.AddScoped<IAuto, AutoDAO>();
builder.Services.AddScoped<IReserva, ReservaDAO>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IAutoService, AutoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
