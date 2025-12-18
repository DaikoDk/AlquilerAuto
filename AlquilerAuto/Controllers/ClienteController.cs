using Microsoft.AspNetCore.Mvc;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;

namespace AlquilerAuto.Controllers
{
    public class ClienteController : Controller
    {
        ICliente _cliente;
        public ClienteController(ICliente cliente)
        {
            _cliente = cliente;
        }

        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => _cliente.Listado()));
        }

       
        public async Task<IActionResult> Create()
        {

            return View(await Task.Run(() => new Cliente()));
        }
        [HttpPost] public async Task<IActionResult> Create(Cliente reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            ModelState.AddModelError("", _cliente.agregar(reg));

            return View(await Task.Run(() => reg));
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await Task.Run(() => _cliente.buscar(id)));
        }
        [HttpPost] public async Task<IActionResult> Edit(Cliente reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);
            ModelState.AddModelError("", _cliente.actualizar(reg));

            return View(await Task.Run(() => reg));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await Task.Run(() => _cliente.buscar(id));

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }
        [HttpPost] public IActionResult Delete(Cliente reg)
        {
            string resultado = _cliente.eliminar(reg);

            if (string.IsNullOrEmpty(resultado))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            return View(reg);
        }

    }
}
