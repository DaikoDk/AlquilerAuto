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
                using (SqlCommand cmd = new SqlCommand("usp_auto_actualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);
                    cmd.Parameters.AddWithValue("@placa", reg.placa ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idMarca", reg.idMarca);
                    cmd.Parameters.AddWithValue("@idModelo", reg.idModelo);
                    cmd.Parameters.AddWithValue("@anio", reg.anio);
                    cmd.Parameters.AddWithValue("@color", reg.color ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@kilometrajeActual", reg.kilometrajeActual);
                    cmd.Parameters.AddWithValue("@precioPorDia", reg.precioPorDia);
                    cmd.Parameters.AddWithValue("@precioPorHora", reg.precioPorHora ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", reg.estado ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) 
            { 
                mensaje = ex.Message; 
            }
            return mensaje;
        }

        public string agregar(Auto reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_auto_agregar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@placa", reg.placa ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idMarca", reg.idMarca);
                    cmd.Parameters.AddWithValue("@idModelo", reg.idModelo);
                    cmd.Parameters.AddWithValue("@anio", reg.anio);
                    cmd.Parameters.AddWithValue("@color", reg.color ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@numeroMotor", reg.numeroMotor ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@numeroChasis", reg.numeroChasis ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@kilometrajeActual", reg.kilometrajeActual);
                    cmd.Parameters.AddWithValue("@precioPorDia", reg.precioPorDia);
                    cmd.Parameters.AddWithValue("@precioPorHora", reg.precioPorHora ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) 
            { 
                mensaje = ex.Message; 
            }
            return mensaje;
        }

        public Auto buscar(int codigo)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_auto_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAuto", codigo);

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Auto
                        {
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            placa = Convert.ToString(dr["placa"]),
                            idMarca = Convert.ToInt32(dr["idMarca"]),
                            idModelo = Convert.ToInt32(dr["idModelo"]),
                            nombreMarca = dr["nombreMarca"] != DBNull.Value ? Convert.ToString(dr["nombreMarca"]) : null,
                            nombreModelo = dr["nombreModelo"] != DBNull.Value ? Convert.ToString(dr["nombreModelo"]) : null,
                            anio = Convert.ToInt32(dr["anio"]),
                            color = dr["color"] != DBNull.Value ? Convert.ToString(dr["color"]) : null,
                            numeroMotor = dr["numeroMotor"] != DBNull.Value ? Convert.ToString(dr["numeroMotor"]) : null,
                            numeroChasis = dr["numeroChasis"] != DBNull.Value ? Convert.ToString(dr["numeroChasis"]) : null,
                            kilometrajeActual = Convert.ToInt32(dr["kilometrajeActual"]),
                            precioPorDia = Convert.ToDecimal(dr["precioPorDia"]),
                            precioPorHora = dr["precioPorHora"] != DBNull.Value ? Convert.ToDecimal(dr["precioPorHora"]) : null,
                            estado = dr["estado"] != DBNull.Value ? Convert.ToString(dr["estado"]) : null
                        };
                    }
                }
            }
            return new Auto();
        }

        public string eliminar(Auto reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_auto_eliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) 
            { 
                mensaje = ex.Message; 
            }
            return mensaje;
        }

        public IEnumerable<Auto> Listado()
        {
            List<Auto> temporal = new List<Auto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_auto_listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        temporal.Add(new Auto
                        {
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            placa = dr["placa"] != DBNull.Value ? Convert.ToString(dr["placa"]) : null,
                            nombreMarca = dr["marca"] != DBNull.Value ? Convert.ToString(dr["marca"]) : null,
                            nombreModelo = dr["modelo"] != DBNull.Value ? Convert.ToString(dr["modelo"]) : null,
                            anio = Convert.ToInt32(dr["anio"]),
                            color = dr["color"] != DBNull.Value ? Convert.ToString(dr["color"]) : null,
                            kilometrajeActual = Convert.ToInt32(dr["kilometrajeActual"]),
                            precioPorDia = Convert.ToDecimal(dr["precioPorDia"]),
                            precioPorHora = dr["precioPorHora"] != DBNull.Value ? Convert.ToDecimal(dr["precioPorHora"]) : null,
                            estado = dr["estado"] != DBNull.Value ? Convert.ToString(dr["estado"]) : null
                        });
                    }
                }
            }
            return temporal;
        }
    }
}
