using AlquilerAuto.Models;

namespace AlquilerAuto.ViewModels
{
    /// <summary>
    /// ViewModel para la vista vw_autos_disponibles
    /// </summary>
    public class AutoDisponibleVM
    {
        public int idAuto { get; set; }
        public string? placa { get; set; }
        public string? marca { get; set; }
        public string? modelo { get; set; }
        public string? categoria { get; set; }
        public int anio { get; set; }
        public string? color { get; set; }
        public int kilometrajeActual { get; set; }
        public decimal precioPorDia { get; set; }
        public decimal? precioPorHora { get; set; }
    }

    /// <summary>
    /// ViewModel para la vista vw_reservas_activas
    /// </summary>
    public class ReservaActivaVM
    {
        public int idReserva { get; set; }
        public string? cliente { get; set; }
        public string? dni { get; set; }
        public string? telefono { get; set; }
        public string? auto { get; set; }
        public string? placa { get; set; }
        public DateTime fechaInicio { get; set; }
        public TimeSpan horaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public TimeSpan horaFin { get; set; }
        public string? estado { get; set; }
        public int diasRestantes { get; set; }
    }

    /// <summary>
    /// ViewModel para la vista vw_autos_mantenimiento
    /// </summary>
    public class AutoMantenimientoVM
    {
        public int idAuto { get; set; }
        public string? placa { get; set; }
        public string? marca { get; set; }
        public string? modelo { get; set; }
        public int kilometrajeActual { get; set; }
        public int? ultimaRevisionKm { get; set; }
        public int? proximaRevisionKm { get; set; }
        public int kmDesdeUltimaRevision { get; set; }
        public string? estadoMantenimiento { get; set; }
    }

    /// <summary>
    /// ViewModel para el Dashboard principal
    /// </summary>
    public class DashboardVM
    {
        // Estadísticas generales
        public int TotalAutos { get; set; }
        public int AutosDisponibles { get; set; }
        public int AutosAlquilados { get; set; }
        public int AutosMantenimiento { get; set; }

        public int TotalClientes { get; set; }
        public int ClientesActivos { get; set; }

        public int ReservasActivas { get; set; }
        public int ReservasHoy { get; set; }
        public int ReservasEsteMes { get; set; }

        public decimal IngresosMes { get; set; }
        public decimal IngresosDia { get; set; }

        // Listas para tablas
        public IEnumerable<ReservaActivaVM> ReservasProximas { get; set; } = new List<ReservaActivaVM>();
        public IEnumerable<AutoMantenimientoVM> AutosRequierenMantenimiento { get; set; } = new List<AutoMantenimientoVM>();
        public IEnumerable<Mantenimiento> MantenimientosVencidos { get; set; } = new List<Mantenimiento>();
    }
}
