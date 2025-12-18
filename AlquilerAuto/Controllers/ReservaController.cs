using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlquilerAuto.Controllers
{
    public class ReservaController : Controller
    {
        IReserva _reserva;
        ICliente _cliente;
        IAuto _auto;
        public ReservaController(IReserva reserva, ICliente cliente, IAuto auto)
        {
            _reserva = reserva;
            _cliente = cliente;
            _auto = auto;
        }

        public IActionResult Index()
        {
            return View( _reserva.Listado());
        }

        public IActionResult Create()
        {
            ViewBag.Clientes = _cliente.Listado().Select(c => new SelectListItem
            {
                Value = c.idCliente.ToString(),
                Text = c.nombreApe
            });

            ViewBag.Autos = _auto.Listado()
                .Where(a => a.estado == "Disponible")
                .Select(a => new SelectListItem
                {
                    Value = a.idAuto.ToString(),
                    Text = $"{a.placa} - {a.marca} {a.modelo}"
                });

            return View(new ReservaVM());
        }

        [HttpPost]
        public IActionResult Create(ReservaVM vm)
        {
            if (!ModelState.IsValid)
            {
                // Recargar combos si hay error
                ViewBag.Clientes = _cliente.Listado().Select(c => new SelectListItem
                {
                    Value = c.idCliente.ToString(),
                    Text = c.nombreApe
                });

                ViewBag.Autos = _auto.Listado()
                    .Where(a => a.estado == "Disponible")
                    .Select(a => new SelectListItem
                    {
                        Value = a.idAuto.ToString(),
                        Text = $"{a.placa} - {a.marca} {a.modelo}"
                    });

                return View(vm);
            }

            Reserva reserva = new Reserva
            {
                idCliente = vm.idCliente.Value,
                idAuto = vm.idAuto.Value,
                fechaInicio = vm.fechaInicio.Value,
                fechaFin = vm.fechaFin.Value
            };

            string resultado = _reserva.agregar(reserva);

            if (resultado == "OK")
                return RedirectToAction("Index");

            // Recargar combos en caso de error del SP
            ViewBag.Clientes = _cliente.Listado().Select(c => new SelectListItem
            {
                Value = c.idCliente.ToString(),
                Text = c.nombreApe
            });

            ViewBag.Autos = _auto.Listado()
                .Where(a => a.estado == "Disponible")
                .Select(a => new SelectListItem
                {
                    Value = a.idAuto.ToString(),
                    Text = $"{a.placa} - {a.marca} {a.modelo}"
                });

            ModelState.AddModelError("", resultado);
            return View(vm);
        }


        public IActionResult Delete(int id)
        {
            var detalle = _reserva.BuscarDetalle(id); // Llama al nuevo método
            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        [HttpPost]
        public IActionResult DeleteConfirmad(int id)
        {
            string resultado = _reserva.Cancelar(id);

            if (resultado == "OK")
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado);
            var detalle = _reserva.BuscarDetalle(id);
            return View("Delete", detalle);
        }
    }
}
