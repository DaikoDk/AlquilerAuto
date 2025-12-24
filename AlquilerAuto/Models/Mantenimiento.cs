using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_mantenimiento")]
    public class Mantenimiento
    {
        [Key]
        public int idMantenimiento { get; set; }

        [Required]
        [Display(Name = "Auto")]
        public int idAuto { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo de Mantenimiento")]
        public string tipo { get; set; } = "Preventivo";

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string? descripcion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        [Display(Name = "Costo")]
        public decimal costo { get; set; } = 0;

        [Display(Name = "Kilometraje Programado")]
        public int? kilometrajeProgramado { get; set; }

        [Display(Name = "Fecha Programada")]
        [DataType(DataType.Date)]
        public DateTime? fechaProgramada { get; set; }

        [Display(Name = "Fecha Realizada")]
        public DateTime? fechaRealizada { get; set; }

        [StringLength(30)]
        [Display(Name = "Estado")]
        public string estado { get; set; } = "Programado"; // Programado, En proceso, Completado, Cancelado

        [Display(Name = "Próximo Mantenimiento (Km)")]
        public int? proximoMantenimientoKm { get; set; }

        [StringLength(100)]
        [Display(Name = "Taller")]
        public string? taller { get; set; }

        [StringLength(100)]
        [Display(Name = "Responsable")]
        public string? responsable { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime? fechaRegistro { get; set; }

        // Propiedades de navegación (no mapeadas)
        [NotMapped]
        public string? placaAuto { get; set; }

        [NotMapped]
        public string? modeloAuto { get; set; }

        [NotMapped]
        public int? kilometrajeActualAuto { get; set; }
    }
}
