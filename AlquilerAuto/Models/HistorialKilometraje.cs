using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_historial_kilometraje")]
    public class HistorialKilometraje
    {
        [Key]
        public int idHistorial { get; set; }

        [Required]
        public int idAuto { get; set; }

        public int? idReserva { get; set; }

        [Required]
        [Display(Name = "Kilometraje Anterior")]
        public int kilometrajeAnterior { get; set; }

        [Required]
        [Display(Name = "Kilometraje Nuevo")]
        public int kilometrajeNuevo { get; set; }

        [Display(Name = "Diferencia")]
        public int diferencia { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Tipo de Registro")]
        public string tipoRegistro { get; set; } = "Reserva";

        [StringLength(300)]
        [Display(Name = "Observaciones")]
        public string? observaciones { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime? fechaRegistro { get; set; }

        [StringLength(100)]
        [Display(Name = "Usuario")]
        public string? usuarioRegistro { get; set; }

        // Propiedades de navegación (no mapeadas)
        [NotMapped]
        public string? placaAuto { get; set; }

        [NotMapped]
        public string? modeloAuto { get; set; }
    }
}
