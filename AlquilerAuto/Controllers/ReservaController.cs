using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Servicio;
using AlquilerAuto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class ReservaController : Controller
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly IAutoService _autoService;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(
            IReservaService reservaService, 
            IClienteService clienteService, 
            IAutoService autoService,
            ILogger<ReservaController> logger)
        {
            _reservaService = reservaService;
            _clienteService = clienteService;
            _autoService = autoService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_reservaService.Listar());
        }

        public IActionResult Create()
        {
            CargarListasSelect();
            return View(new ReservaVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReservaVM vm)
        {
            _logger.LogInformation("=== INICIO Create POST ===");
            
            // Limpiar errores de campos no requeridos
            ModelState.Remove("subtotal");
            ModelState.Remove("precioPorDia");
            ModelState.Remove("precioPorHora");
            ModelState.Remove("duracionDias");
            ModelState.Remove("duracionHoras");

            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState no es válido. Errores:");
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        _logger.LogWarning($"  {error.Key}: {err.ErrorMessage}");
                    }
                }
                CargarListasSelect();
                return View(vm);
            }

            _logger.LogInformation("Llamando a _reservaService.AgregarReserva...");
            string resultado = _reservaService.AgregarReserva(vm);
            _logger.LogInformation($"Resultado del servicio: '{resultado}'");

            if (resultado != "Reserva registrada correctamente.")
            {
                _logger.LogWarning($"Resultado no coincide. Agregando error al ModelState.");
                ModelState.AddModelError("", resultado);
                CargarListasSelect();
                return View(vm);
            }
            
            // Mensaje simple de éxito
            _logger.LogInformation("Reserva exitosa. Configurando TempData y redirigiendo...");
            TempData["mensaje"] = "Reserva registrada exitosamente. Se iniciará automáticamente en la fecha programada.";
            
            _logger.LogInformation("Ejecutando RedirectToAction('Index')");
            return RedirectToAction("Index");
        }

        public IActionResult Finalizar(int id)
        {
            var detalle = _reservaService.BuscarDetalleVM(id);
            if (detalle == null || detalle.IdReserva == null)
                return NotFound();

            if (detalle.Estado != "Alquilado")
            {
                TempData["error"] = "Solo se pueden finalizar reservas en estado 'Alquilado'.";
                return RedirectToAction("Index");
            }

            var vm = new FinalizarReservaVM
            {
                IdReserva = detalle.IdReserva.Value,
                KilometrajeInicio = detalle.KilometrajeInicio,
                KilometrajeFin = detalle.kilometrajeActualAuto ?? 0,
                NombreCliente = detalle.nombreCliente,
                DniCliente = detalle.dniCliente,
                PlacaAuto = detalle.placaAuto,
                ModeloAuto = detalle.modeloAuto,
                ColorAuto = detalle.colorAuto,
                KilometrajeActualAuto = detalle.kilometrajeActualAuto,
                FechaInicio = detalle.FechaInicio,
                HoraInicio = detalle.HoraInicio,
                FechaFin = detalle.FechaFin,
                HoraFin = detalle.HoraFin,
                Subtotal = detalle.Subtotal,
                Estado = detalle.Estado
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarConfirmado(FinalizarReservaVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Finalizar", vm);
            }

            // Verificar si requiere registrar reparación primero
            if ((vm.EstadoEntrega == "Con daños reparados" || vm.EstadoEntrega == "Pendiente reparacion") 
                && !vm.RequiereReparacion)
            {
                // Marcar que ya pasó por aquí
                vm.RequiereReparacion = true;
                
                // Guardar temporalmente en TempData para recuperar después
                TempData["FinalizarReservaVM"] = System.Text.Json.JsonSerializer.Serialize(vm);
                
                // Redirigir a registrar reparación
                TempData["mensaje"] = "Por favor, registre los daños encontrados antes de finalizar la reserva.";
                return RedirectToAction("Create", "Reparacion", new { idReserva = vm.IdReserva });
            }

            string usuario = User.Identity?.Name ?? "Sistema";
            string resultado = _reservaService.FinalizarReserva(
                vm.IdReserva, 
                vm.KilometrajeFin, 
                vm.EstadoEntrega, 
                vm.Observaciones ?? "", 
                usuario
            );

            if (resultado == "Reserva finalizada correctamente.")
            {
                TempData["mensaje"] = resultado;
                
                // Limpiar TempData
                TempData.Remove("FinalizarReservaVM");
                
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            return View("Finalizar", vm);
        }

        public IActionResult Cancelar(int id)
        {
            var detalle = _reservaService.BuscarDetalleVM(id);
            if (detalle == null)
                return NotFound();
            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelarConfirmado(int id)
        {
            string resultado = _reservaService.CancelarReserva(id);

            if (resultado == "OK")
            {
                TempData["mensaje"] = "Reserva cancelada correctamente.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            var detalle = _reservaService.BuscarDetalleVM(id);
            return View("Cancelar", detalle);
        }

        /*Metodo auxiliar para cargar los combos de la VM*/
        private void CargarListasSelect()
        {
            ViewBag.Clientes = _clienteService.Listar()
                .Select(c => new SelectListItem
                {
                    Value = c.idCliente.ToString(),
                    Text = c.nombreApe
                }).ToList();

            ViewBag.Autos = _autoService.ListarDisponible()
                .Select(a => new SelectListItem
                {
                    Value = a.idAuto.ToString(),
                    Text = $"{a.nombreMarca} {a.nombreModelo} - {a.placa}"
                }).ToList();
        }
    }
}
