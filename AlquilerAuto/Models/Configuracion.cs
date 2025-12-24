using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_configuracion")]
    public class Configuracion
    {
        [Key]
        public int idConfiguracion { get; set; }

        [Required]
        [StringLength(50)]
        public string? clave { get; set; }

        [Required]
        [StringLength(200)]
        public string? valor { get; set; }

        [StringLength(300)]
        public string? descripcion { get; set; }

        [StringLength(20)]
        public string? tipo { get; set; } // NUMERO, TEXTO, BOOLEAN, DECIMAL

        public DateTime? fechaActualizacion { get; set; }
    }
}
