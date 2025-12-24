using AlquilerAuto.DAO;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Servicio;
using AlquilerAuto.Servicio.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//Moneda PE-Peru
var cultureInfo = new CultureInfo("es-PE");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar sesión para TempData
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ICliente, ClienteDAO>();
builder.Services.AddScoped<IAuto, AutoDAO>();
builder.Services.AddScoped<IReserva, ReservaDAO>();
builder.Services.AddScoped<IUsuario, UsuarioDAO>();

// Nuevos servicios de DAOs
builder.Services.AddScoped<IMarca, MarcaDAO>();
builder.Services.AddScoped<IModelo, ModeloDAO>();
builder.Services.AddScoped<IReparacion, ReparacionDAO>();
builder.Services.AddScoped<IConfiguracion, ConfiguracionDAO>();
builder.Services.AddScoped<IMantenimiento, MantenimientoDAO>();
builder.Services.AddScoped<IDashboard, DashboardDAO>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAutoService, AutoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IReparacionService, ReparacionService>();
builder.Services.AddScoped<IConfiguracionService, ConfiguracionService>();
builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();

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

// Habilitar sesión antes de la autenticación
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
