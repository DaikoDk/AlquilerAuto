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
                string query = "SELECT Nombre, Correo, Rol FROM Usuario WHERE Correo = @c AND Clave = @p";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@c", correo);
                cmd.Parameters.AddWithValue("@p", clave);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader()) 
                {
                    if (dr.Read()) 
                    {
                        usuario = new Usuario();
                        usuario.Nombre = dr["Nombre"].ToString();
                        usuario.Correo = dr["Correo"].ToString();
                        usuario.Rol = dr["Rol"].ToString();
                        
                    }
                }
            }
            return usuario;
        }
    }
}
