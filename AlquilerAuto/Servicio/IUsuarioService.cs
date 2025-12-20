using AlquilerAuto.Models;

namespace AlquilerAuto.Servicio
{
    public interface IUsuarioService
    {
        Usuario ValidarLogin(string correo, string clave);       
     
    }
}
