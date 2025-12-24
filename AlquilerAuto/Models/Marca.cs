using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_marca")]
    public class Marca
    {
        [Key]
        public int idMarca { get; set; }

        [Display(Name = "Nombre de la Marca")]
        [Required(ErrorMessage = "El nombre de la marca es obligatorio")]
        [StringLength(50)]
        public string? nombre { get; set; }

        [Display(Name = "País de Origen")]
        [StringLength(50)]
        public string? paisOrigen { get; set; }

        public bool activo { get; set; } = true;
        public DateTime? fechaRegistro { get; set; }
    }
}
