using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class MarcaDAO : IMarca
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public IEnumerable<Marca> Listado()
        {
            List<Marca> lista = new List<Marca>();
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_marca_listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Marca
                        {
                            idMarca = Convert.ToInt32(dr["idMarca"]),
                            nombre = Convert.ToString(dr["nombre"]),
                            paisOrigen = Convert.ToString(dr["paisOrigen"]),
                            activo = Convert.ToBoolean(dr["activo"]),
                            fechaRegistro = dr["fechaRegistro"] != DBNull.Value 
                                ? Convert.ToDateTime(dr["fechaRegistro"]) 
                                : null
                        });
                    }
                }
            }
            return lista;
        }

        public Marca buscar(int codigo)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tb_marca WHERE idMarca = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", codigo);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Marca
                        {
                            idMarca = Convert.ToInt32(dr["idMarca"]),
                            nombre = Convert.ToString(dr["nombre"]),
                            paisOrigen = Convert.ToString(dr["paisOrigen"]),
                            activo = Convert.ToBoolean(dr["activo"])
                        };
                    }
                }
            }
            return new Marca();
        }

        public string agregar(Marca reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_marca_agregar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", reg.nombre);
                    cmd.Parameters.AddWithValue("@paisOrigen", reg.paisOrigen ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string actualizar(Marca reg)
        {
            throw new NotImplementedException();
        }

        public string eliminar(Marca reg)
        {
            throw new NotImplementedException();
        }
    }
}
