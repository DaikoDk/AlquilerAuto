using AlquilerAuto.Models;

namespace AlquilerAuto.Servicio
{
    public interface IMantenimientoService
    {
        IEnumerable<Mantenimiento> Listar();
        IEnumerable<Mantenimiento> ListarPorAuto(int idAuto);
        IEnumerable<Mantenimiento> ListarPendientes();
        IEnumerable<Mantenimiento> ListarVencidos();
        Mantenimiento Buscar(int id);
        string Agregar(Mantenimiento mantenimiento);
        string Actualizar(Mantenimiento mantenimiento);
        string Completar(int idMantenimiento);
        string Cancelar(int idMantenimiento);
        string Eliminar(int id);
    }
}
