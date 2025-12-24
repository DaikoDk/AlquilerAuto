using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;

namespace AlquilerAuto.Servicio.Service
{
    public class MantenimientoService : IMantenimientoService
    {
        private readonly IMantenimiento _mantenimientoDAO;
        private readonly IConfiguracionService _configuracionService;

        public MantenimientoService(IMantenimiento mantenimientoDAO, IConfiguracionService configuracionService)
        {
            _mantenimientoDAO = mantenimientoDAO;
            _configuracionService = configuracionService;
        }

        public IEnumerable<Mantenimiento> Listar()
        {
            return _mantenimientoDAO.Listado();
        }

        public IEnumerable<Mantenimiento> ListarPorAuto(int idAuto)
        {
            return _mantenimientoDAO.ListadoPorAuto(idAuto);
        }

        public IEnumerable<Mantenimiento> ListarPendientes()
        {
            return _mantenimientoDAO.Listado()
                .Where(m => m.estado == "Programado" || m.estado == "En proceso");
        }

        public IEnumerable<Mantenimiento> ListarVencidos()
        {
            var hoy = DateTime.Now.Date;
            return _mantenimientoDAO.Listado()
                .Where(m => m.estado == "Programado" && 
                           m.fechaProgramada.HasValue && 
                           m.fechaProgramada.Value.Date < hoy);
        }

        public Mantenimiento Buscar(int id)
        {
            var mantenimiento = _mantenimientoDAO.Buscar(id);
            return mantenimiento ?? new Mantenimiento();
        }

        public string Agregar(Mantenimiento mantenimiento)
        {
            // ===== VALIDACIONES DE NEGOCIO =====

            if (string.IsNullOrWhiteSpace(mantenimiento.tipo))
                return "El tipo de mantenimiento es obligatorio.";

            if (mantenimiento.costo < 0)
                return "El costo no puede ser negativo.";

            var tiposValidos = new[] { "Preventivo", "Correctivo", "Revision tecnica", "Cambio aceite", "Cambio llantas", "Otro" };
            if (!tiposValidos.Contains(mantenimiento.tipo))
                return "Tipo de mantenimiento no válido.";

            // Establecer estado por defecto
            if (string.IsNullOrWhiteSpace(mantenimiento.estado))
                mantenimiento.estado = "Programado";

            // Establecer fecha de registro
            mantenimiento.fechaRegistro = DateTime.Now;

            string resultado = _mantenimientoDAO.Agregar(mantenimiento);

            if (resultado == "OK")
                return "Mantenimiento registrado exitosamente.";

            return resultado;
        }

        public string Actualizar(Mantenimiento mantenimiento)
        {
            // ===== VALIDACIONES DE NEGOCIO =====

            if (string.IsNullOrWhiteSpace(mantenimiento.tipo))
                return "El tipo de mantenimiento es obligatorio.";

            if (mantenimiento.costo < 0)
                return "El costo no puede ser negativo.";

            string resultado = _mantenimientoDAO.Actualizar(mantenimiento);

            if (resultado == "OK")
                return "Mantenimiento actualizado exitosamente.";

            return resultado;
        }

        public string Completar(int idMantenimiento)
        {
            var mantenimiento = _mantenimientoDAO.Buscar(idMantenimiento);
            
            if (mantenimiento == null || mantenimiento.idMantenimiento == 0)
                return "El mantenimiento no existe.";

            if (mantenimiento.estado == "Completado")
                return "El mantenimiento ya está completado.";

            string resultado = _mantenimientoDAO.ActualizarEstado(idMantenimiento, "Completado", DateTime.Now);

            if (resultado == "OK")
                return "Mantenimiento completado exitosamente.";

            return resultado;
        }

        public string Cancelar(int idMantenimiento)
        {
            var mantenimiento = _mantenimientoDAO.Buscar(idMantenimiento);
            
            if (mantenimiento == null || mantenimiento.idMantenimiento == 0)
                return "El mantenimiento no existe.";

            if (mantenimiento.estado == "Completado")
                return "No se puede cancelar un mantenimiento completado.";

            string resultado = _mantenimientoDAO.ActualizarEstado(idMantenimiento, "Cancelado", null);

            if (resultado == "OK")
                return "Mantenimiento cancelado exitosamente.";

            return resultado;
        }

        public string Eliminar(int id)
        {
            var mantenimiento = _mantenimientoDAO.Buscar(id);
            
            if (mantenimiento == null || mantenimiento.idMantenimiento == 0)
                return "El mantenimiento no existe.";

            if (mantenimiento.estado == "En proceso")
                return "No se puede eliminar un mantenimiento en proceso.";

            string resultado = _mantenimientoDAO.Eliminar(id);

            if (resultado == "OK")
                return "Mantenimiento eliminado exitosamente.";

            return resultado;
        }
    }
}
