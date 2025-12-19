using Microsoft.AspNetCore.Mvc;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Service;
using System.Threading.Tasks;

namespace AlquilerAuto.Controllers
{
    public class AutoController : Controller
    {
       private readonly IAutoService _autoService;
        public AutoController(IAutoService autoService) 
        {
            _autoService = autoService;
        }

        public IActionResult Index()
        {
            var autos = _autoService.Listar();
            return View(autos);
        }
        public IActionResult Create() 
        {
            return View(new Auto());
        }
        [HttpPost] public IActionResult Create(Auto reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            string resultado = _autoService.Agregar(reg);

            if (resultado == "OK")
                return RedirectToAction("Index"); // Redirige si todo salió bien

            // Si hay error, lo mostramos
            ModelState.AddModelError("", resultado);
            return View(reg);
        }
        public IActionResult Edit(int id) 
        {
            var auto = _autoService.Buscar(id);
            if (auto == null || auto.idAuto == 0)
                return NotFound();

            return View(auto);
        }
        [HttpPost] public IActionResult Edit(Auto reg) 
        {
            if (!ModelState.IsValid)
                return View(reg);

            string resultado = _autoService.Actualizar(reg);

            if (resultado.Contains("exitosamente"))
                return RedirectToAction("Index");

            ModelState.AddModelError("", resultado);
            return View(reg);
        }
        public IActionResult Delete(int id) 
        {
            var auto =  _autoService.Buscar(id);
            if (auto == null)
                return NotFound();

            return View(auto);
        }
        [HttpPost] public IActionResult DeleteInactive(int id) 
        {
            string resultado = _autoService.Eliminar(id);

            if (resultado == "Auto inactivo correctamente.")
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);

            // Recarga el auto solo si existe, pero si el Service dice "no se encontró..." (~objeto null),
            // de todas formas, muestra la vista solo si hay datos.
            var auto = _autoService.Buscar(id);
            if (auto == null)
            {
                // Aquí solo rediriges, pero el mensaje SIEMPRE viene del service (por ModelState)
                return RedirectToAction("Index");
            }
            return View("Delete", auto);
        }
    }
}
