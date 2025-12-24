using AlquilerAuto.Models;
using Microsoft.Data.SqlClient;
using AlquilerAuto.Repositorio;

namespace AlquilerAuto.DAO
{
    
    public class UsuarioDAO : IUsuario
    {
        string cadena = (new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build().GetConnectionString("cn") ?? "");

        public Usuario ObtenerUsuario(string correo, string clave) 
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string query = "SELECT idUsuario, nombre, correo, rol, activo FROM tb_usuario WHERE correo = @c AND clave = @p AND activo = 1";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@c", correo);
                cmd.Parameters.AddWithValue("@p", clave);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader()) 
                {
                    if (dr.Read()) 
                    {
                        usuario = new Usuario();
                        usuario.IdUsuario = Convert.ToInt32(dr["idUsuario"]);
                        usuario.Nombre = dr["nombre"].ToString();
                        usuario.Correo = dr["correo"].ToString();
                        usuario.Rol = dr["rol"].ToString();
                        usuario.Activo = Convert.ToBoolean(dr["activo"]);
                    }
                }
            }
            return usuario;
        }
    }
}
