namespace AlquilerAuto.ViewModels
{
    public class ReservaDetalleVM
    {
        public int ? IdReserva { get; set; }
        public string ? Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Datos del cliente
        public string ? nombreCliente { get; set; }
        public string ? dniCliente { get; set; }

        // Datos del auto
        public string ? placaAuto { get; set; }
        public string ? modeloAuto { get; set; }
    }
}
