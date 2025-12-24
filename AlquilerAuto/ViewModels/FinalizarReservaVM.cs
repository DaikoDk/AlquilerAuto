using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.ViewModels
{
    public class FinalizarReservaVM
    {
        public int IdReserva { get; set; }
        
        [Required(ErrorMessage = "El kilometraje final es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El kilometraje debe ser mayor a 0")]
        [Display(Name = "Kilometraje Final")]
        public int KilometrajeFin { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar el estado de entrega")]
        [Display(Name = "Estado de Entrega")]
        public string EstadoEntrega { get; set; } = "";
        
        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
        
        // Datos informativos (solo lectura)
        public int? KilometrajeInicio { get; set; }
        public string? NombreCliente { get; set; }
        public string? DniCliente { get; set; }
        public string? PlacaAuto { get; set; }
        public string? ModeloAuto { get; set; }
        public string? ColorAuto { get; set; }
        public int? KilometrajeActualAuto { get; set; }
        public DateTime FechaInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public TimeSpan HoraFin { get; set; }
        public decimal Subtotal { get; set; }
        public string? Estado { get; set; }
        
        // Costo de reparaciones (agregado)
        public decimal CostoReparaciones { get; set; } = 0;
        
        // Flag para indicar si necesita registrar reparación
        public bool RequiereReparacion { get; set; } = false;
    }
}
