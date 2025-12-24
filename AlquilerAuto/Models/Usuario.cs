using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlquilerAuto.Models
{
    [Table("tb_usuario")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatorio")]
        public string? Clave { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string? Rol { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public bool Activo { get; set; } = true;
    }
}
