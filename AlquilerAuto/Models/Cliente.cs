using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }

        [Display(Name ="Nombre Apellido")]
        [Required(ErrorMessage ="El Nombre y Apellido es obligatorio")]
        public string? nombreApe { get; set; }

        [Display(Name = "DNI")]
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength =8 ,ErrorMessage = "El DNI debe tener 8 dígitos")]
        [RegularExpression(@"^[0-8]+$", ErrorMessage = "Solo números")]
        public string? dni { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El número de telefono es obligatorio")]
        [StringLength(9, MinimumLength =9 ,ErrorMessage = "El Telefono debe tener 9 dígitos")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Solo números")]
        public string? telefono { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string? email { get; set; }
    }
}
