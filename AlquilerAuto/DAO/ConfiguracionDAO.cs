using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class ConfiguracionDAO : IConfiguracion
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public string ObtenerValor(string clave)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_configuracion_obtener", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@clave", clave);

                cn.Open();
                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? "";
            }
        }

        public int ObtenerValorEntero(string clave)
        {
            var valor = ObtenerValor(clave);
            return int.TryParse(valor, out int resultado) ? resultado : 0;
        }

        public decimal ObtenerValorDecimal(string clave)
        {
            var valor = ObtenerValor(clave);
            return decimal.TryParse(valor, out decimal resultado) ? resultado : 0;
        }

        public bool ObtenerValorBooleano(string clave)
        {
            var valor = ObtenerValor(clave);
            return valor == "1" || valor.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public string Actualizar(string clave, string valor)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_configuracion_actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.Parameters.AddWithValue("@valor", valor);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public IEnumerable<Configuracion> ListarTodas()
        {
            List<Configuracion> lista = new List<Configuracion>();
            
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tb_configuracion ORDER BY clave", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Configuracion
                        {
                            idConfiguracion = Convert.ToInt32(dr["idConfiguracion"]),
                            clave = Convert.ToString(dr["clave"]),
                            valor = Convert.ToString(dr["valor"]),
                            descripcion = dr["descripcion"] != DBNull.Value ? Convert.ToString(dr["descripcion"]) : null,
                            tipo = dr["tipo"] != DBNull.Value ? Convert.ToString(dr["tipo"]) : null,
                            fechaActualizacion = dr["fechaActualizacion"] != DBNull.Value ? Convert.ToDateTime(dr["fechaActualizacion"]) : null
                        });
                    }
                }
            }
            
            return lista;
        }
    }
}
