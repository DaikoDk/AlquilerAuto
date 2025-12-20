using AlquilerAuto.Models;
using AlquilerAuto.DAO;
using AlquilerAuto.Repositorio;

namespace AlquilerAuto.Servicio.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuario _usuarioDAO;

        public UsuarioService(IUsuario usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        public Usuario ValidarLogin(string correo, string clave)
        {
           return _usuarioDAO.ObtenerUsuario(correo, clave);
        }


    }
}
