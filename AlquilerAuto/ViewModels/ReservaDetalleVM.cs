namespace AlquilerAuto.ViewModels
{
    public class ReservaDetalleVM
    {
        public int? IdReserva { get; set; }
        public string? Estado { get; set; }
        public string? EstadoEntrega { get; set; }

        // Fechas y Horas
        public DateTime FechaInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public TimeSpan HoraFin { get; set; }

        // Fechas/Horas reales
        public DateTime? FechaHoraInicioReal { get; set; }
        public DateTime? FechaHoraFinReal { get; set; }

        // Kilometraje
        public int? KilometrajeInicio { get; set; }
        public int? KilometrajeFin { get; set; }
        public int? KilometrosRecorridos { get; set; }

        // Costos
        public decimal Subtotal { get; set; }
        public decimal Mora { get; set; }
        public decimal CostoReparaciones { get; set; }
        public decimal Total { get; set; }

        public string? ObservacionesEntrega { get; set; }

        // Datos del cliente
        public string? nombreCliente { get; set; }
        public string? dniCliente { get; set; }

        // Datos del auto
        public string? placaAuto { get; set; }
        public string? modeloAuto { get; set; }
        public string? colorAuto { get; set; }
        public int? kilometrajeActualAuto { get; set; }
    }
}
