using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_auto")]
    public class Auto
    {
        [Key]
        public int idAuto { get; set; }

        [StringLength(10, MinimumLength = 7, ErrorMessage = "La placa debe tener entre 7 y 10 caracteres")]
        [Display(Name = "Placa"), Required(ErrorMessage = "La placa es obligatorio")]
        public string? placa { get; set; }

        // Relaciones con Marca y Modelo
        [Display(Name = "Marca"), Required(ErrorMessage = "La marca es obligatoria")]
        public int idMarca { get; set; }

        [Display(Name = "Modelo"), Required(ErrorMessage = "El modelo es obligatorio")]
        public int idModelo { get; set; }

        [Display(Name = "Año")] 
        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1990, 2026, ErrorMessage = "El año debe estar entre 1990 y 2026")]
        public int anio { get; set; }

        [Display(Name = "Color")]
        [StringLength(30)]
        public string? color { get; set; }

        [Display(Name = "Número de Motor")]
        [StringLength(50)]
        public string? numeroMotor { get; set; }

        [Display(Name = "Número de Chasis")]
        [StringLength(50)]
        public string? numeroChasis { get; set; }

        // Kilometraje
        [Display(Name = "Kilometraje Actual")]
        [Range(0, int.MaxValue, ErrorMessage = "El kilometraje debe ser mayor o igual a 0")]
        public int kilometrajeActual { get; set; } = 0;

        [Display(Name = "Última Revisión (Km)")]
        public int? ultimaRevisionKm { get; set; }

        [Display(Name = "Próxima Revisión (Km)")]
        public int? proximaRevisionKm { get; set; }

        // Precios
        [Display(Name = "Precio por Día")]
        [Required(ErrorMessage = "El precio por día es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal precioPorDia { get; set; }

        [Display(Name = "Precio por Hora")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal? precioPorHora { get; set; }

        [Display(Name = "Estado"), Required(ErrorMessage = "El estado es obligatorio")]
        public string? estado { get; set; }

        // Campos de auditoría
        public DateTime? fechaRegistro { get; set; }
        public DateTime? fechaUltimaActualizacion { get; set; }
        public bool activo { get; set; } = true;

        // Propiedades de navegación (solo para lectura)
        [NotMapped]
        public string? nombreMarca { get; set; }

        [NotMapped]
        public string? nombreModelo { get; set; }

        [NotMapped]
        public string nombreCompleto => $"{nombreMarca} {nombreModelo}";
    }
}
