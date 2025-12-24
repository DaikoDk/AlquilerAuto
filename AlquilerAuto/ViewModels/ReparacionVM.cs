using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.ViewModels
{
    public class ReparacionVM
    {
        public int IdReserva { get; set; }
        public int IdAuto { get; set; }

        [Display(Name = "Tipo de Reparación")]
        public int? IdCatalogoReparacion { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción del Daño")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Display(Name = "Costo")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal Costo { get; set; }

        [Display(Name = "Responsable")]
        public string? Responsable { get; set; } = "Cliente";

        // Información contextual
        public string? ClienteNombre { get; set; }
        public string? AutoPlaca { get; set; }
        public string? AutoModelo { get; set; }

        // Lista de reparaciones comunes (para dropdown)
        public List<CatalogoReparacionDto>? ReparacionesComunes { get; set; }
    }

    public class CatalogoReparacionDto
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public decimal CostoEstimado { get; set; }
    }
}
