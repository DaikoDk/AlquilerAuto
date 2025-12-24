using AlquilerAuto.Models;

namespace AlquilerAuto.Repositorio
{
    public interface IReparacion
    {
        string agregar(Reparacion reg);
        string actualizar(Reparacion reg);
        IEnumerable<Reparacion> ListadoPorReserva(int idReserva);
        IEnumerable<CatalogoReparacion> ListadoCatalogo();
        string ActualizarEstado(int idReparacion, string estado, DateTime? fechaInicio, DateTime? fechaFin);
    }
}
