using AlquilerAuto.Models;
using AlquilerAuto.Servicio;
using AlquilerAuto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class ReparacionController : Controller
    {
        private readonly IReparacionService _reparacionService;
        private readonly IReservaService _reservaService;

        public ReparacionController(IReparacionService reparacionService, IReservaService reservaService)
        {
            _reparacionService = reparacionService;
            _reservaService = reservaService;
        }

        // ===== THIN CONTROLLER: Solo orquesta y retorna vistas =====

        // GET: Reparacion/Index?idReserva=X
        public IActionResult Index(int? idReserva)
        {
            if (!idReserva.HasValue)
                return View(new List<Reparacion>());

            ViewBag.IdReserva = idReserva.Value;
            ViewBag.DatosReserva = _reservaService.BuscarDetalleVM(idReserva.Value);
            
            return View(_reparacionService.ListarPorReserva(idReserva.Value));
        }

        // GET: Reparacion/Create?idReserva=X
        public IActionResult Create(int idReserva)
        {
            // El servicio prepara los datos
            var dto = _reparacionService.PrepararFormularioCreacion(idReserva);
            
            if (dto == null)
                return NotFound("La reserva no existe.");

            // Verificar si viene desde Finalizar
            if (TempData["FinalizarReservaVM"] != null)
            {
                dto.VolverAFinalizar = true;
                ViewBag.MensajeRetorno = "Después de registrar la reparación, volverá a finalizar la reserva.";
            }

            // Cargar dropdown de catálogo
            CargarCatalogoYResponsables();
            
            return View(dto);
        }

        // POST: Reparacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReparacionCreateDTO dto)
        {
            // IMPORTANTE: Si el IdAuto viene como 0, recuperarlo de la reserva
            if (dto.IdAuto == 0)
            {
                var reserva = _reservaService.BuscarReserva(dto.IdReserva);
                if (reserva != null && reserva.idAuto > 0)
                {
                    dto.IdAuto = reserva.idAuto;
                }
            }

            if (!ModelState.IsValid)
            {
                CargarCatalogoYResponsables();
                return View(dto);
            }

            // El servicio maneja TODA la lógica
            string usuario = User.Identity?.Name ?? "Sistema";
            string resultado = _reparacionService.CrearReparacion(dto, usuario);

            if (resultado == "Reparación registrada exitosamente.")
            {
                TempData["mensaje"] = resultado;
                
                // Si debe volver a Finalizar
                if (dto.VolverAFinalizar && TempData["FinalizarReservaVM"] != null)
                {
                    TempData["mensaje"] = "Reparación registrada. Complete la finalización de la reserva.";
                    return RedirectToAction("Finalizar", "Reserva", new { id = dto.IdReserva });
                }
                
                return RedirectToAction("Index", new { idReserva = dto.IdReserva });
            }

            // Si hay error, agregar al ModelState
            ModelState.AddModelError("", resultado);
            CargarCatalogoYResponsables();
            return View(dto);
        }

        // GET: Reparacion/CambiarEstado/5?estado=Completada
        public IActionResult CambiarEstado(int id, string estado, int idReserva)
        {
            // El servicio valida y ejecuta
            DateTime? fecha = estado == "En proceso" ? DateTime.Now : null;
            DateTime? fechaFin = estado == "Completada" ? DateTime.Now : null;

            string resultado = _reparacionService.ActualizarEstado(id, estado, fecha, fechaFin);

            TempData[resultado.Contains("exitosamente") ? "mensaje" : "error"] = resultado;

            return RedirectToAction("Index", new { idReserva });
        }

        // GET: Reparacion/Edit/5
        public IActionResult Edit(int id)
        {
            // Buscar la reparación en todas las reservas
            var todasLasReparaciones = _reparacionService.ListarPorReserva(0) ?? new List<Reparacion>();
            var reparacion = todasLasReparaciones.FirstOrDefault(r => r.idReparacion == id);
            
            if (reparacion == null)
                return NotFound("La reparación no existe.");

            return View(reparacion);
        }

        // POST: Reparacion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Reparacion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Actualizar solo costo y estado
            string resultado = _reparacionService.ActualizarReparacion(model);

            if (resultado.Contains("exitosamente"))
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index", new { idReserva = model.idReserva });
            }

            ModelState.AddModelError("", resultado);
            return View(model);
        }

        // ===== MÉTODOS AUXILIARES PRIVADOS =====
        private void CargarCatalogoYResponsables()
        {
            // Catálogo de reparaciones comunes
            ViewBag.Catalogo = _reparacionService.ListarCatalogo()
                .Select(c => new SelectListItem
                {
                    Value = c.idCatalogoReparacion.ToString(),
                    Text = $"{c.descripcion} - S/ {c.costoEstimado:N2}"
                }).ToList();

            // Responsables
            ViewBag.Responsables = new List<SelectListItem>
            {
                new SelectListItem { Value = "Cliente", Text = "Cliente (Se cobra al cliente)" },
                new SelectListItem { Value = "Empresa", Text = "Empresa (Asume la empresa)" },
                new SelectListItem { Value = "Tercero", Text = "Tercero (Seguro u otro)" }
            };
        }
    }
}
