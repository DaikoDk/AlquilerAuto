using AlquilerAuto.Models;
using AlquilerAuto.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAuto.Controllers
{
    [Authorize(Roles = "Administrador")] // Solo administradores pueden modificar configuraciones
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfiguracionController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        // ===== THIN CONTROLLER: Solo orquesta =====

        // GET: Configuracion/Index
        public IActionResult Index()
        {
            var configuraciones = _configuracionService.ListarTodas();
            return View(configuraciones);
        }

        // GET: Configuracion/Edit/MORA_POR_HORA
        public IActionResult Edit(string clave)
        {
            if (string.IsNullOrEmpty(clave))
                return NotFound();

            var configuraciones = _configuracionService.ListarTodas();
            var config = configuraciones.FirstOrDefault(c => c.clave == clave);

            if (config == null)
                return NotFound();

            return View(config);
        }

        // POST: Configuracion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Configuracion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validar que clave y valor no sean nulos
            if (string.IsNullOrWhiteSpace(model.clave) || string.IsNullOrWhiteSpace(model.valor))
            {
                ModelState.AddModelError("", "La clave y el valor son obligatorios");
                return View(model);
            }

            // El servicio maneja las validaciones
            string resultado = _configuracionService.Actualizar(model.clave, model.valor);

            if (resultado == "Configuración actualizada exitosamente.")
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            return View(model);
        }
    }
}
