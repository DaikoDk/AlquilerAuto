using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class AutoDAO : IAuto
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");
        public string actualizar(Auto reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_auto_actualizar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);
                        cmd.Parameters.AddWithValue("@placa", reg.placa);
                        cmd.Parameters.AddWithValue("@marca", reg.marca);
                        cmd.Parameters.AddWithValue("@modelo", reg.modelo);
                        cmd.Parameters.AddWithValue("@anio", reg.anio);
                        cmd.Parameters.AddWithValue("@precioPorDia", reg.precioPorDia);
                        cmd.Parameters.AddWithValue("@estado", reg.estado);

                        cn.Open();
                        mensaje = cmd.ExecuteScalar().ToString()??"";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string agregar(Auto reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_auto_agregar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@placa", reg.placa);
                        cmd.Parameters.AddWithValue("@marca", reg.marca);
                        cmd.Parameters.AddWithValue("@modelo", reg.modelo);
                        cmd.Parameters.AddWithValue("@anio", reg.anio);
                        cmd.Parameters.AddWithValue("@precioPorDia", reg.precioPorDia);
                        cmd.Parameters.AddWithValue("@estado", reg.estado);


                        cn.Open();
                        mensaje = cmd.ExecuteScalar().ToString() ?? "";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public Auto buscar(int codigo)
        {
            return Listado().FirstOrDefault(x=> x.idAuto == codigo) ?? new Auto();
        }

        public string eliminar(Auto reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_auto_eliminar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);

                        cn.Open();
                        mensaje = cmd.ExecuteScalar().ToString() ?? "";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public IEnumerable<Auto> Listado()
        {
            List<Auto> temporal = new List<Auto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_auto", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            temporal.Add(new Auto()
                            {
                                idAuto = Convert.ToInt32(dr["idAuto"]),
                                placa = Convert.ToString(dr["placa"]),
                                marca = Convert.ToString(dr["marca"]),
                                modelo = Convert.ToString(dr["modelo"]),
                                anio = Convert.ToInt32(dr["anio"]),
                                precioPorDia = Convert.ToDecimal(dr["precioPorDia"]),
                                estado = Convert.ToString(dr["estado"])
                            });
                        }
                    }
                }
            }
            return temporal;
        }
    }
}
