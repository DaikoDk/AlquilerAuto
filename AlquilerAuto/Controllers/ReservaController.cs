using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Service;
using AlquilerAuto.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlquilerAuto.Controllers
{
    public class ReservaController : Controller
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly IAutoService _autoService;
        public ReservaController(IReservaService reservaService, IClienteService clienteService, IAutoService autoService)
        {
            _reservaService = reservaService;
            _clienteService = clienteService;
            _autoService = autoService;
        }

        public IActionResult Index()
        {
            return View( _reservaService.Listar());
        }

        public IActionResult Create()
        {
            CargarListasSelect();
            return View(new ReservaVM());
        }

        [HttpPost]
        public IActionResult Create(ReservaVM vm)
        {
            if (!ModelState.IsValid)
            {
                CargarListasSelect();
                return View(vm);
            }
            string resultado = _reservaService.AgregarReserva(vm);

            if (resultado != "Reserva registrada correctamente.")
            {
                ModelState.AddModelError("", resultado);
                CargarListasSelect();
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Cancelar(int id)
        {
            var detalle = _reservaService.BuscarDetalleVM(id);
            if (detalle == null)
                return NotFound();
            return View(detalle);
        }

        [HttpPost]
        public IActionResult CancelarConfirmado(int id)
        {
            string resultado = _reservaService.CancelarReserva(id);

            if (resultado == "OK")
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado); // Mensaje de error directo
            var detalle = _reservaService.BuscarDetalleVM(id);
            return View("Cancelar", detalle);
        }

        /*Metodo auxiliar para cargar los conmbos de la VM*/
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
                    Text = a.modelo
                }).ToList();
        }

    }
}
