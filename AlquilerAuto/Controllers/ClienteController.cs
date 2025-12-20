using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        public ClienteController( IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public IActionResult Index()
        {
            var clientes = _clienteService.Listar();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View(new Cliente());
        }

        [HttpPost] public IActionResult Create(Cliente reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            string mensaje = _clienteService.AgregarCliente(reg);

            if (mensaje.Contains("exitosamente")) 
            {
                TempData["mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", mensaje);
            return View(reg);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _clienteService.Buscar(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        [HttpPost] public IActionResult Edit(Cliente reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            string mensaje = _clienteService.ActualizarCliente(reg);

            if (mensaje.Contains("exitosamente"))
            {
                TempData["mensaje"] = mensaje;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", mensaje);
            return View(reg);
        }
        public IActionResult Delete(int id)
        {
            var cliente = _clienteService.Buscar(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        [HttpPost] public IActionResult DeleteConfirmed(int id)
        {
            string mensaje = _clienteService.EliminarCliente(id);

            TempData["mensaje"] = mensaje;  // Le pasas el mensaje tal cual a la vista

            return RedirectToAction("Index");
        }
    }
}
