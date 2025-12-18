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
        public DateTime? fechaInicio { get; set; }

        [Required(ErrorMessage = "Debes ingresar la fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime? fechaFin { get; set; }
    }
}
/*Solo sirve en el caso de Crear o actualizar*/