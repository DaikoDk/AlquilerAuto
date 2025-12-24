using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_catalogo_reparacion")]
    public class CatalogoReparacion
    {
        [Key]
        public int idCatalogoReparacion { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(200)]
        public string? descripcion { get; set; }

        [Display(Name = "Costo Estimado")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal costoEstimado { get; set; } = 0;

        [Display(Name = "Tiempo Estimado (Horas)")]
        [Range(0, int.MaxValue)]
        public int tiempoEstimadoHoras { get; set; } = 0;

        public bool activo { get; set; } = true;
        public DateTime? fechaRegistro { get; set; }
    }
}
