using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class ReparacionDAO : IReparacion
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public IEnumerable<CatalogoReparacion> ListadoCatalogo()
        {
            List<CatalogoReparacion> lista = new List<CatalogoReparacion>();
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_catalogo_reparacion_listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new CatalogoReparacion
                        {
                            idCatalogoReparacion = Convert.ToInt32(dr["idCatalogoReparacion"]),
                            descripcion = Convert.ToString(dr["descripcion"]),
                            costoEstimado = Convert.ToDecimal(dr["costoEstimado"]),
                            tiempoEstimadoHoras = Convert.ToInt32(dr["tiempoEstimadoHoras"])
                        });
                    }
                }
            }
            return lista;
        }

        public IEnumerable<Reparacion> ListadoPorReserva(int idReserva)
        {
            List<Reparacion> lista = new List<Reparacion>();
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_reparacion_listar_por_reserva", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idReserva", idReserva);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Reparacion
                        {
                            idReparacion = Convert.ToInt32(dr["idReparacion"]),
                            descripcion = Convert.ToString(dr["descripcion"]),
                            costo = Convert.ToDecimal(dr["costo"]),
                            estado = Convert.ToString(dr["estado"]),
                            responsable = Convert.ToString(dr["responsable"]),
                            descripcionCatalogo = dr["descripcionCatalogo"] != DBNull.Value 
                                ? Convert.ToString(dr["descripcionCatalogo"]) 
                                : null,
                            fechaReporte = dr["fechaReporte"] != DBNull.Value 
                                ? Convert.ToDateTime(dr["fechaReporte"]) 
                                : null,
                            fechaInicio = dr["fechaInicio"] != DBNull.Value 
                                ? Convert.ToDateTime(dr["fechaInicio"]) 
                                : null,
                            fechaFin = dr["fechaFin"] != DBNull.Value 
                                ? Convert.ToDateTime(dr["fechaFin"]) 
                                : null
                        });
                    }
                }
            }
            return lista;
        }

        public string agregar(Reparacion reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reparacion_agregar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", reg.idReserva);
                    cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);
                    cmd.Parameters.AddWithValue("@idCatalogoReparacion", reg.idCatalogoReparacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@descripcion", reg.descripcion);
                    cmd.Parameters.AddWithValue("@costo", reg.costo);
                    cmd.Parameters.AddWithValue("@responsable", reg.responsable ?? "Cliente");
                    cmd.Parameters.AddWithValue("@usuarioReporte", reg.usuarioReporte ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string ActualizarEstado(int idReparacion, string estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reparacion_actualizar_estado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReparacion", idReparacion);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin ?? (object)DBNull.Value);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string actualizar(Reparacion reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE tb_reparacion 
                    SET costo = @costo,
                        estado = @estado,
                        descripcion = @descripcion
                    WHERE idReparacion = @idReparacion", cn))
                {
                    cmd.Parameters.AddWithValue("@idReparacion", reg.idReparacion);
                    cmd.Parameters.AddWithValue("@costo", reg.costo);
                    cmd.Parameters.AddWithValue("@estado", reg.estado ?? "Pendiente");
                    cmd.Parameters.AddWithValue("@descripcion", reg.descripcion ?? "");

                    cn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    mensaje = filasAfectadas > 0 ? "OK" : "No se actualizó ningún registro";
                }
            }
            catch (Exception ex) 
            { 
                mensaje = ex.Message; 
            }
            
            return mensaje;
        }
    }
}
