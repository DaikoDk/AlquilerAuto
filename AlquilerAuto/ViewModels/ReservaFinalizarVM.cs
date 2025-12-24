using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.ViewModels
{
    public class ReservaFinalizarVM
    {
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "Debe ingresar el kilometraje final")]
        [Range(1, int.MaxValue, ErrorMessage = "El kilometraje debe ser mayor a 0")]
        [Display(Name = "Kilometraje Final")]
        public int KilometrajeFin { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado de entrega")]
        [Display(Name = "Estado de Entrega")]
        public string? EstadoEntrega { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        // Información de la reserva (solo lectura)
        public DateTime FechaInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int? KilometrajeInicio { get; set; }
        public string? Cliente { get; set; }
        public string? Auto { get; set; }
        public decimal Subtotal { get; set; }

        // Cálculos
        public decimal MoraCalculada { get; set; }
        public int MinutosRetraso { get; set; }
        public bool TieneRetraso { get; set; }
    }
}
