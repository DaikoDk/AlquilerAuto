using AlquilerAuto.ViewModels;

namespace AlquilerAuto.Repositorio
{
    public interface IDashboard
    {
        IEnumerable<AutoDisponibleVM> ObtenerAutosDisponibles();
        IEnumerable<ReservaActivaVM> ObtenerReservasActivas();
        IEnumerable<AutoMantenimientoVM> ObtenerAutosMantenimiento();
        
        // Estadísticas
        int ContarAutos();
        int ContarAutosDisponibles();
        int ContarAutosAlquilados();
        int ContarClientes();
        int ContarReservasActivas();
        decimal ObtenerIngresosMes();
    }
}
