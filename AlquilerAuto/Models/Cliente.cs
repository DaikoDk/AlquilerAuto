using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_cliente")]
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }

        [Display(Name ="Nombre Apellido")]
        [Required(ErrorMessage ="El Nombre y Apellido es obligatorio")]
        public string? nombreApe { get; set; }

        [Display(Name = "DNI")]
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Solo números")]
        public string? dni { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El número de telefono es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El Telefono debe tener 9 dígitos")]
        [RegularExpression(@"^9[0-9]+$", ErrorMessage = "Debe iniciar con 9 y solo números")]
        public string? telefono { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string? email { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200)]
        public string? direccion { get; set; }

        // Nuevos campos de control
        [Display(Name = "Número de Reservas")]
        public int numeroReservas { get; set; } = 0;

        [Display(Name = "Número de Incidentes")]
        public int numeroIncidentes { get; set; } = 0;

        [Display(Name = "Bloqueado")]
        public bool bloqueado { get; set; } = false;

        public DateTime? fechaRegistro { get; set; }
        public bool activo { get; set; } = true;
    }
}
