using AlquilerAuto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.ViewModels
{
    public class ReservaVM
    {
        [Required(ErrorMessage = "Debes seleccionar un cliente")]
        public int? idCliente { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un auto")]
        public int? idAuto { get; set; }

        [Required(ErrorMessage = "Debes ingresar la fecha de inicio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Inicio")]
        public DateTime? fechaInicio { get; set; }

        [Required(ErrorMessage = "Debes ingresar la hora de inicio")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Inicio")]
        public TimeSpan? horaInicio { get; set; }

        [Required(ErrorMessage = "Debes ingresar la fecha de fin")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin")]
        public DateTime? fechaFin { get; set; }

        [Required(ErrorMessage = "Debes ingresar la hora de fin")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora Fin")]
        public TimeSpan? horaFin { get; set; }

        // Campos calculados/informativos
        [Display(Name = "Subtotal")]
        public decimal? subtotal { get; set; }

        [Display(Name = "Precio por Día")]
        public decimal? precioPorDia { get; set; }

        [Display(Name = "Precio por Hora")]
        public decimal? precioPorHora { get; set; }

        // Para cálculos en tiempo real (JavaScript)
        public int duracionDias { get; set; }
        public double duracionHoras { get; set; }
    }
}
/*Solo sirve en el caso de Crear o actualizar*/