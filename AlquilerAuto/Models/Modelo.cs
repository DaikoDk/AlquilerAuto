using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_modelo")]
    public class Modelo
    {
        [Key]
        public int idModelo { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Debe seleccionar una marca")]
        public int idMarca { get; set; }

        [Display(Name = "Nombre del Modelo")]
        [Required(ErrorMessage = "El nombre del modelo es obligatorio")]
        [StringLength(50)]
        public string? nombre { get; set; }

        [Display(Name = "Categoría")]
        [StringLength(30)]
        public string? categoria { get; set; } // Sedan, SUV, Hatchback, etc.

        [Display(Name = "Número de Pasajeros")]
        [Range(2, 15, ErrorMessage = "Debe ser entre 2 y 15 pasajeros")]
        public int? numeroPasajeros { get; set; }

        public bool activo { get; set; } = true;
        public DateTime? fechaRegistro { get; set; }

        // Propiedad de navegación
        [NotMapped]
        public string? nombreMarca { get; set; }

        [NotMapped]
        public string nombreCompleto => $"{nombreMarca} - {nombre}";
    }
}
