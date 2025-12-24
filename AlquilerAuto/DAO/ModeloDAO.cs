using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class ModeloDAO : IModelo
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public IEnumerable<Modelo> Listado()
        {
            return ListadoPorMarca(0);
        }

        public IEnumerable<Modelo> ListadoPorMarca(int idMarca)
        {
            List<Modelo> lista = new List<Modelo>();
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_modelo_listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idMarca", idMarca == 0 ? (object)DBNull.Value : idMarca);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Modelo
                        {
                            idModelo = Convert.ToInt32(dr["idModelo"]),
                            idMarca = Convert.ToInt32(dr["idMarca"]),
                            nombreMarca = Convert.ToString(dr["nombreMarca"]),
                            nombre = Convert.ToString(dr["nombre"]),
                            categoria = Convert.ToString(dr["categoria"]),
                            numeroPasajeros = dr["numeroPasajeros"] != DBNull.Value 
                                ? Convert.ToInt32(dr["numeroPasajeros"]) 
                                : null,
                            activo = Convert.ToBoolean(dr["activo"])
                        });
                    }
                }
            }
            return lista;
        }

        public Modelo buscar(int codigo)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT m.*, ma.nombre AS nombreMarca 
                FROM tb_modelo m
                INNER JOIN tb_marca ma ON m.idMarca = ma.idMarca
                WHERE m.idModelo = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", codigo);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Modelo
                        {
                            idModelo = Convert.ToInt32(dr["idModelo"]),
                            idMarca = Convert.ToInt32(dr["idMarca"]),
                            nombreMarca = Convert.ToString(dr["nombreMarca"]),
                            nombre = Convert.ToString(dr["nombre"]),
                            categoria = Convert.ToString(dr["categoria"]),
                            numeroPasajeros = dr["numeroPasajeros"] != DBNull.Value 
                                ? Convert.ToInt32(dr["numeroPasajeros"]) 
                                : null
                        };
                    }
                }
            }
            return new Modelo();
        }

        public string agregar(Modelo reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_modelo_agregar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idMarca", reg.idMarca);
                    cmd.Parameters.AddWithValue("@nombre", reg.nombre);
                    cmd.Parameters.AddWithValue("@categoria", reg.categoria ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@numeroPasajeros", reg.numeroPasajeros ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string actualizar(Modelo reg)
        {
            throw new NotImplementedException();
        }

        public string eliminar(Modelo reg)
        {
            throw new NotImplementedException();
        }
    }
}
