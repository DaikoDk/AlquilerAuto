using AlquilerAuto.Models;
using AlquilerAuto.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        private readonly IMantenimientoService _mantenimientoService;
        private readonly IAutoService _autoService;

        public MantenimientoController(IMantenimientoService mantenimientoService, IAutoService autoService)
        {
            _mantenimientoService = mantenimientoService;
            _autoService = autoService;
        }

        // GET: Mantenimiento/Index
        public IActionResult Index()
        {
            var mantenimientos = _mantenimientoService.Listar();
            return View(mantenimientos);
        }

        // GET: Mantenimiento/Create
        public IActionResult Create()
        {
            CargarAutos();
            return View(new Mantenimiento());
        }

        // POST: Mantenimiento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Mantenimiento model)
        {
            if (!ModelState.IsValid)
            {
                CargarAutos();
                return View(model);
            }

            string resultado = _mantenimientoService.Agregar(model);

            if (resultado == "Mantenimiento registrado exitosamente.")
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            CargarAutos();
            return View(model);
        }

        // GET: Mantenimiento/Edit/5
        public IActionResult Edit(int id)
        {
            var mantenimiento = _mantenimientoService.Buscar(id);
            
            if (mantenimiento == null || mantenimiento.idMantenimiento == 0)
                return NotFound();

            CargarAutos();
            return View(mantenimiento);
        }

        // POST: Mantenimiento/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Mantenimiento model)
        {
            if (!ModelState.IsValid)
            {
                CargarAutos();
                return View(model);
            }

            string resultado = _mantenimientoService.Actualizar(model);

            if (resultado == "Mantenimiento actualizado exitosamente.")
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            CargarAutos();
            return View(model);
        }

        // GET: Mantenimiento/Completar/5
        public IActionResult Completar(int id)
        {
            string resultado = _mantenimientoService.Completar(id);

            TempData[resultado.Contains("exitosamente") ? "mensaje" : "error"] = resultado;

            return RedirectToAction("Index");
        }

        // GET: Mantenimiento/Cancelar/5
        public IActionResult Cancelar(int id)
        {
            string resultado = _mantenimientoService.Cancelar(id);

            TempData[resultado.Contains("exitosamente") ? "mensaje" : "error"] = resultado;

            return RedirectToAction("Index");
        }

        // GET: Mantenimiento/Delete/5
        public IActionResult Delete(int id)
        {
            var mantenimiento = _mantenimientoService.Buscar(id);
            
            if (mantenimiento == null || mantenimiento.idMantenimiento == 0)
                return NotFound();

            return View(mantenimiento);
        }

        // POST: Mantenimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            string resultado = _mantenimientoService.Eliminar(id);

            TempData[resultado.Contains("exitosamente") ? "mensaje" : "error"] = resultado;

            return RedirectToAction("Index");
        }

        // Método auxiliar
        private void CargarAutos()
        {
            ViewBag.Autos = _autoService.Listar()
                .Select(a => new SelectListItem
                {
                    Value = a.idAuto.ToString(),
                    Text = $"{a.nombreMarca} {a.nombreModelo} - {a.placa}"
                }).ToList();
        }
    }
}
