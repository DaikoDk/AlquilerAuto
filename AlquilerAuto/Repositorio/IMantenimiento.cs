using AlquilerAuto.Models;

namespace AlquilerAuto.Repositorio
{
    public interface IMantenimiento
    {
        IEnumerable<Mantenimiento> Listado();
        IEnumerable<Mantenimiento> ListadoPorAuto(int idAuto);
        Mantenimiento Buscar(int idMantenimiento);
        string Agregar(Mantenimiento mantenimiento);
        string Actualizar(Mantenimiento mantenimiento);
        string ActualizarEstado(int idMantenimiento, string estado, DateTime? fechaRealizada = null);
        string Eliminar(int idMantenimiento);
    }
}
