using System.Diagnostics;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboard _dashboardDAO;
        private readonly IMantenimiento _mantenimientoDAO;

        public HomeController(ILogger<HomeController> logger, IDashboard dashboardDAO, IMantenimiento mantenimientoDAO)
        {
            _logger = logger;
            _dashboardDAO = dashboardDAO;
            _mantenimientoDAO = mantenimientoDAO;
        }

        public IActionResult Index()
        {
            var dashboard = new DashboardVM
            {
                // Estadísticas generales
                TotalAutos = _dashboardDAO.ContarAutos(),
                AutosDisponibles = _dashboardDAO.ContarAutosDisponibles(),
                AutosAlquilados = _dashboardDAO.ContarAutosAlquilados(),
                TotalClientes = _dashboardDAO.ContarClientes(),
                ReservasActivas = _dashboardDAO.ContarReservasActivas(),
                IngresosMes = _dashboardDAO.ObtenerIngresosMes(),

                // Listas
                ReservasProximas = _dashboardDAO.ObtenerReservasActivas().Take(5),
                AutosRequierenMantenimiento = _dashboardDAO.ObtenerAutosMantenimiento()
                    .Where(a => a.estadoMantenimiento != "OK").Take(5),
                MantenimientosVencidos = _mantenimientoDAO.Listado()
                    .Where(m => m.estado == "Programado" && 
                               m.fechaProgramada.HasValue && 
                               m.fechaProgramada.Value.Date < DateTime.Now.Date)
                    .Take(5)
                    .ToList()
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
