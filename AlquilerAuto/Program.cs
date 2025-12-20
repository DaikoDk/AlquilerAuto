using AlquilerAuto.DAO;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Servicio;
using AlquilerAuto.Servicio.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddScoped<IUsuario, UsuarioDAO>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAutoService, AutoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        // Aquí indicamos a dónde ir si intentan entrar sin permiso
        option.LoginPath = "/Usuario/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Tiempo de vida de la sesión
    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
