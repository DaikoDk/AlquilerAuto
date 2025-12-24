using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_reparacion")]
    public class Reparacion
    {
        [Key]
        public int idReparacion { get; set; }

        [Display(Name = "Reserva")]
        [Required]
        public int idReserva { get; set; }

        [Display(Name = "Auto")]
        [Required]
        public int idAuto { get; set; }

        [Display(Name = "Tipo de Reparación")]
        public int? idCatalogoReparacion { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500)]
        public string? descripcion { get; set; }

        [Display(Name = "Costo")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal costo { get; set; }

        [Display(Name = "Estado")]
        public string? estado { get; set; } // Pendiente, En proceso, Completada, Cancelada

        [Display(Name = "Responsable")]
        public string? responsable { get; set; } // Cliente, Empresa, Tercero

        // Fechas
        public DateTime? fechaReporte { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }

        public string? usuarioReporte { get; set; }

        // Propiedades de navegación (no mapeadas)
        [NotMapped]
        public string? nombreCliente { get; set; }

        [NotMapped]
        public string? placaAuto { get; set; }

        [NotMapped]
        public string? descripcionCatalogo { get; set; }
    }
}
