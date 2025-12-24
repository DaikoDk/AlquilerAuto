using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.ViewModels
{
    public class ReparacionCreateDTO
    {
        [Required(ErrorMessage = "La reserva es obligatoria")]
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "El auto es obligatorio")]
        public int IdAuto { get; set; }

        [Display(Name = "Tipo de Reparación (Catálogo)")]
        public int? IdCatalogoReparacion { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción del Daño")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        [Display(Name = "Costo de Reparación")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "Debe especificar quién asume el costo")]
        [Display(Name = "Responsable del Costo")]
        public string Responsable { get; set; } = "Cliente";

        [Required(ErrorMessage = "Debe seleccionar el estado de la reparación")]
        [Display(Name = "Estado de la Reparación")]
        public string EstadoReparacion { get; set; } = "Pendiente";

        // Propiedades para la vista (no se envían al servidor)
        public string? NombreCliente { get; set; }
        public string? PlacaAuto { get; set; }
        public string? ModeloAuto { get; set; }
        public string? EstadoReserva { get; set; }
        
        // Flag para saber si viene desde Finalizar
        public bool VolverAFinalizar { get; set; } = false;
    }
}
