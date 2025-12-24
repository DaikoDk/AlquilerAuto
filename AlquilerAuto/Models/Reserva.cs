using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_reserva")]
    public class Reserva
    {
        [Key]
        public int idReserva { get; set; }

        [Display(Name = "Cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente")]
        public int idCliente { get; set; }

        [Display(Name = "Auto")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un auto")]
        public int idAuto { get; set; }

        // Fechas y Horas
        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime fechaInicio { get; set; }

        [Display(Name = "Hora Inicio")]
        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan horaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime fechaFin { get; set; }

        [Display(Name = "Hora Fin")]
        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan horaFin { get; set; }

        // Kilometraje
        [Display(Name = "Kilometraje Inicio")]
        public int? kilometrajeInicio { get; set; }

        [Display(Name = "Kilometraje Fin")]
        public int? kilometrajeFin { get; set; }

        [NotMapped]
        [Display(Name = "Km Recorridos")]
        public int? kilometrosRecorridos => kilometrajeFin.HasValue && kilometrajeInicio.HasValue
            ? kilometrajeFin - kilometrajeInicio
            : null;

        // Costos
        [Display(Name = "Subtotal")]
        [Required]
        public decimal subtotal { get; set; }

        [Display(Name = "Mora")]
        public decimal mora { get; set; } = 0;

        [Display(Name = "Costo Reparaciones")]
        public decimal costoReparaciones { get; set; } = 0;

        [Display(Name = "Total")]
        [Required]
        public decimal total { get; set; }

        // Estados
        [Display(Name = "Estado")]
        public string? estado { get; set; }

        [Display(Name = "Estado de Entrega")]
        public string? estadoEntrega { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? observacionesEntrega { get; set; }

        // Control de tiempos reales
        [Display(Name = "Fecha/Hora Inicio Real")]
        public DateTime? fechaHoraInicioReal { get; set; }

        [Display(Name = "Fecha/Hora Fin Real")]
        public DateTime? fechaHoraFinReal { get; set; }

        // Auditoría
        public DateTime? fechaCreacion { get; set; }
        public string? usuarioCreacion { get; set; }
        public DateTime? fechaFinalizacion { get; set; }
        public string? usuarioFinalizacion { get; set; }

        // Propiedades solo para listado (no mapeadas)
        [NotMapped]
        public string? cliente { get; set; }

        [NotMapped]
        public string? placa { get; set; }

        [NotMapped]
        public string? autoModelo { get; set; }
    }
}
