using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using AlquilerAuto.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AlquilerAuto.Controllers
{
    [Authorize]
    public class AutoController : Controller
    {
        private readonly IAutoService _autoService;
        private readonly IMarca _marcaDAO;
        private readonly IModelo _modeloDAO;

        public AutoController(IAutoService autoService, IMarca marcaDAO, IModelo modeloDAO)
        {
            _autoService = autoService;
            _marcaDAO = marcaDAO;
            _modeloDAO = modeloDAO;
        }

        public IActionResult Index()
        {
            var autos = _autoService.Listar();
            return View(autos);
        }

        public IActionResult Create()
        {
            CargarMarcas();
            return View(new Auto());
        }

        [HttpPost]
        public IActionResult Create(Auto reg)
        {
            if (!ModelState.IsValid)
            {
                CargarMarcas();
                return View(reg);
            }

            string resultado = _autoService.Agregar(reg);

            if (resultado.Contains("exitosamente"))
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            CargarMarcas();
            return View(reg);
        }

        public IActionResult Edit(int id)
        {
            var auto = _autoService.Buscar(id);
            if (auto == null || auto.idAuto == 0)
                return NotFound();

            CargarMarcas();
            CargarModelos(auto.idMarca);
            return View(auto);
        }

        [HttpPost]
        public IActionResult Edit(Auto reg)
        {
            if (!ModelState.IsValid)
            {
                CargarMarcas();
                CargarModelos(reg.idMarca);
                return View(reg);
            }

            string resultado = _autoService.Actualizar(reg);

            if (resultado.Contains("exitosamente"))
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultado);
            CargarMarcas();
            CargarModelos(reg.idMarca);
            return View(reg);
        }

        public IActionResult Delete(int id)
        {
            var auto = _autoService.Buscar(id);
            if (auto == null || auto.idAuto == 0)
                return NotFound();

            return View(auto);
        }

        [HttpPost]
        public IActionResult DeleteInactive(int id)
        {
            string resultado = _autoService.Eliminar(id);

            if (resultado.Contains("correctamente"))
            {
                TempData["mensaje"] = resultado;
                return RedirectToAction("Index");
            }

            TempData["error"] = resultado;
            return RedirectToAction("Index");
        }

        // Método AJAX para obtener modelos por marca
        [HttpGet]
        public JsonResult ObtenerModelosPorMarca(int idMarca)
        {
            var modelos = _modeloDAO.ListadoPorMarca(idMarca)
                .Select(m => new SelectListItem
                {
                    Value = m.idModelo.ToString(),
                    Text = m.nombre
                })
                .ToList();

            return Json(modelos);
        }

        // GET: Auto/Historial/5
        public IActionResult Historial(int id)
        {
            var auto = _autoService.Buscar(id);
            if (auto == null || auto.idAuto == 0)
                return NotFound();

            ViewBag.Auto = auto;
            
            // Obtener historial desde la BD
            var historial = ObtenerHistorialKilometraje(id);
            
            return View(historial);
        }

        private List<HistorialKilometraje> ObtenerHistorialKilometraje(int idAuto)
        {
            var lista = new List<HistorialKilometraje>();
            
            string? cadena = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build().GetConnectionString("cn");

            if (string.IsNullOrEmpty(cadena))
                return lista;

            using (var cn = new Microsoft.Data.SqlClient.SqlConnection(cadena))
            using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
                SELECT h.*, a.placa AS placaAuto,
                       ma.nombre + ' ' + mo.nombre AS modeloAuto
                FROM tb_historial_kilometraje h
                INNER JOIN tb_auto a ON h.idAuto = a.idAuto
                INNER JOIN tb_marca ma ON a.idMarca = ma.idMarca
                INNER JOIN tb_modelo mo ON a.idModelo = mo.idModelo
                WHERE h.idAuto = @idAuto
                ORDER BY h.fechaRegistro DESC", cn))
            {
                cmd.Parameters.AddWithValue("@idAuto", idAuto);
                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new HistorialKilometraje
                        {
                            idHistorial = Convert.ToInt32(dr["idHistorial"]),
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            idReserva = dr["idReserva"] != DBNull.Value ? Convert.ToInt32(dr["idReserva"]) : null,
                            kilometrajeAnterior = Convert.ToInt32(dr["kilometrajeAnterior"]),
                            kilometrajeNuevo = Convert.ToInt32(dr["kilometrajeNuevo"]),
                            diferencia = Convert.ToInt32(dr["diferencia"]),
                            tipoRegistro = Convert.ToString(dr["tipoRegistro"]) ?? "Reserva",
                            observaciones = dr["observaciones"] != DBNull.Value ? Convert.ToString(dr["observaciones"]) : null,
                            fechaRegistro = dr["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(dr["fechaRegistro"]) : null,
                            usuarioRegistro = dr["usuarioRegistro"] != DBNull.Value ? Convert.ToString(dr["usuarioRegistro"]) : null,
                            placaAuto = Convert.ToString(dr["placaAuto"]),
                            modeloAuto = Convert.ToString(dr["modeloAuto"])
                        });
                    }
                }
            }

            return lista;
        }

        // Métodos auxiliares privados
        private void CargarMarcas()
        {
            ViewBag.Marcas = _marcaDAO.Listado()
                .Select(m => new SelectListItem
                {
                    Value = m.idMarca.ToString(),
                    Text = m.nombre
                })
                .ToList();
        }

        private void CargarModelos(int idMarca)
        {
            ViewBag.Modelos = _modeloDAO.ListadoPorMarca(idMarca)
                .Select(m => new SelectListItem
                {
                    Value = m.idModelo.ToString(),
                    Text = m.nombre
                })
                .ToList();
        }
    }
}
