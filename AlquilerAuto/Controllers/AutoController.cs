using Microsoft.AspNetCore.Mvc;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Threading.Tasks;

namespace AlquilerAuto.Controllers
{
    public class AutoController : Controller
    {
        IAuto _auto;
        public AutoController(IAuto auto) 
        {
            _auto = auto;
        }

        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => _auto.Listado()));
        }
        public async Task<IActionResult> Create() 
        {
            return View(await Task.Run(() => new Auto()));
        }
        [HttpPost] public async Task<IActionResult> Create(Auto reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            string resultado = _auto.agregar(reg);

            if (resultado == "OK")
                return RedirectToAction("Index"); // Redirige si todo salió bien

            // Si hay error, lo mostramos
            ModelState.AddModelError("", resultado);
            return View(reg);
        }
        public async Task<IActionResult> Edit(int id) 
        {
            return View(await Task.Run(() => _auto.buscar(id)));
        }
        [HttpPost] public async Task<IActionResult> Edit(Auto reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);
            ModelState.AddModelError("", _auto.actualizar(reg));

            return View(await Task.Run(()=> reg));
        }
        public async Task<IActionResult> Delete(int id) 
        {
            var auto = await Task.Run(()=> _auto.buscar(id));
            if (auto == null)
                return NotFound();

            return View(auto);
        }
        [HttpPost] public IActionResult Delete(Auto reg) 
        {
            string resultado = _auto.eliminar(reg);

            if(string.IsNullOrEmpty(resultado)) {
                return RedirectToAction("Index");               
            }
            ModelState.AddModelError("", resultado);

            return View(reg);
        }
    }
}
