using AlquilerAuto.DAO;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;

namespace AlquilerAuto.Servicio.Service
{
    public class AutoService : IAutoService
    {
        private readonly IAuto _autoDAO;
        private readonly IReserva _reservaDAO;

        public AutoService(IAuto autoDAO, IReserva reservaDAO)
        {
            _autoDAO = autoDAO;
            _reservaDAO = reservaDAO;
        }


        public string Actualizar(Auto reg)
        {
            if (ExistePlaca(reg.placa, reg.idAuto))
            {
                return "La placa ya existe.";
            }

            string result = _autoDAO.actualizar(reg);
            if (result != "OK")
            {
                return "No se pudo actualizar el auto: " + result;
            }

            return "Auto actualizado exitosamente.";
        }

        public string Agregar(Auto reg)
        {
            if (ExistePlaca(reg.placa))
                return "La placa ya existe";

            string result = _autoDAO.agregar(reg);

            if (result != "OK")
                return "No se pudo registrar el auto: " + result;

            return "Auto registrado exitosamente.";
        }

        public Auto Buscar(int id)
        {
            var auto = _autoDAO.buscar(id);
            return auto;
        }

        public string Eliminar(int id)
        {
            var auto = _autoDAO.buscar(id);
            if (auto == null)
                return "No se encontró el auto.";

            // Validacion de negocio en service
            if (auto.estado == "Disponible" || auto.estado == "En mantenimiento" || auto.estado == "Alquilado")
                return "No se puede eliminar el auto porque está en estado no permitido.";

            string result = _autoDAO.eliminar(auto);
            if (result != "OK")
            {
                return "No se pudo eliminar el auto: " + result;
            }

            return "Auto eliminado correctamente.";
        }

        public bool ExistePlaca(string placa, int? idAuto = null)
        {
            // Lógica para validación de placas
            var autos = _autoDAO.Listado();
            if (idAuto.HasValue)
                return autos.Any(a => a.placa == placa && a.idAuto != idAuto.Value);
            else
                return autos.Any(a => a.placa == placa);
        }

        public IEnumerable<Auto> Listar()
        {
            return _autoDAO.Listado();
        }

        public IEnumerable<Auto> ListarDisponible()
        {
            return _autoDAO.Listado().Where(a => a.estado == "Disponible");
        }
    }
}
