using System.ComponentModel.DataAnnotations;

namespace AlquilerAuto.Models
{
    public class Reserva
    {
        public int idReserva { get; set; }
        [Display(Name ="CLiente")]
        [Range(1,int.MaxValue,ErrorMessage ="Seleccione un cliente")]
        public int idCliente { get; set; }
        [Display(Name = "Auto")]
        public int idAuto { get; set; }

        [Display(Name = "Fecha Inicio"), Required(ErrorMessage = "La fecha es obligatorio")]
        public DateTime fechaInicio { get; set; }

        [Display(Name = "Fecha Fin"), Required(ErrorMessage = "La fecha es obligatorio")]
        public DateTime fechaFin { get; set; }

        [Display(Name = "Total")]
        public decimal total { get; set; }

        public string? estado { get; set; }

        //Solo para listado
        public string ? cliente { get; set; }
        public string ? placa { get; set; }
    }
}
