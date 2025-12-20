using AlquilerAuto.Models;
namespace AlquilerAuto.Repositorio
{
    public interface IUsuario
    {
        Usuario ObtenerUsuario(string correo, string clave);
    }
}
