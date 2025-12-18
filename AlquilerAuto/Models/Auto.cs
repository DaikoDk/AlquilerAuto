using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.Models
{
    public class Auto
    {
        public int idAuto { get; set; }

        [StringLength(7, MinimumLength = 7, ErrorMessage = "La placa debe tener 7 carácteres")]
        [Display(Name = "Placa"), Required(ErrorMessage = "La placa es obligatorio")]
        public string? placa { get; set; }

        [Display(Name = "Marca"), Required(ErrorMessage = "La marca es obligatorio")]
        public string ? marca { get; set; }

        [Display(Name = "Modelo"), Required(ErrorMessage = "El modelo es obligatorio")]
        public string? modelo { get; set; }

        [Display(Name = "Año")] 
        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1000,9999,ErrorMessage ="El año debe tener 4 dígitos")]
        public int anio { get; set; }

        [Display(Name = "Precio por Día"), Required(ErrorMessage = "El Precio es obligatorio")]
        public decimal precioPorDia { get; set; }

        [Display(Name = "Estado"), Required(ErrorMessage = "El estado es obligatorio")]
        public string? estado { get; set; }
    }
}
