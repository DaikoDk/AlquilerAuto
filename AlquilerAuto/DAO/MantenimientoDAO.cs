using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class MantenimientoDAO : IMantenimiento
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public IEnumerable<Mantenimiento> Listado()
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT m.*, a.placa AS placaAuto, 
                       ma.nombre + ' ' + mo.nombre AS modeloAuto,
                       a.kilometrajeActual AS kilometrajeActualAuto
                FROM tb_mantenimiento m
                INNER JOIN tb_auto a ON m.idAuto = a.idAuto
                INNER JOIN tb_marca ma ON a.idMarca = ma.idMarca
                INNER JOIN tb_modelo mo ON a.idModelo = mo.idModelo
                ORDER BY m.fechaProgramada DESC", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapearMantenimiento(dr));
                    }
                }
            }

            return lista;
        }

        public IEnumerable<Mantenimiento> ListadoPorAuto(int idAuto)
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT m.*, a.placa AS placaAuto, 
                       ma.nombre + ' ' + mo.nombre AS modeloAuto,
                       a.kilometrajeActual AS kilometrajeActualAuto
                FROM tb_mantenimiento m
                INNER JOIN tb_auto a ON m.idAuto = a.idAuto
                INNER JOIN tb_marca ma ON a.idMarca = ma.idMarca
                INNER JOIN tb_modelo mo ON a.idModelo = mo.idModelo
                WHERE m.idAuto = @idAuto
                ORDER BY m.fechaRegistro DESC", cn))
            {
                cmd.Parameters.AddWithValue("@idAuto", idAuto);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapearMantenimiento(dr));
                    }
                }
            }

            return lista;
        }

        public Mantenimiento Buscar(int idMantenimiento)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT m.*, a.placa AS placaAuto, 
                       ma.nombre + ' ' + mo.nombre AS modeloAuto,
                       a.kilometrajeActual AS kilometrajeActualAuto
                FROM tb_mantenimiento m
                INNER JOIN tb_auto a ON m.idAuto = a.idAuto
                INNER JOIN tb_marca ma ON a.idMarca = ma.idMarca
                INNER JOIN tb_modelo mo ON a.idModelo = mo.idModelo
                WHERE m.idMantenimiento = @idMantenimiento", cn))
            {
                cmd.Parameters.AddWithValue("@idMantenimiento", idMantenimiento);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return MapearMantenimiento(dr);
                    }
                }
            }

            return new Mantenimiento();
        }

        public string Agregar(Mantenimiento mantenimiento)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO tb_mantenimiento (idAuto, tipo, descripcion, costo, 
                                                  kilometrajeProgramado, fechaProgramada, 
                                                  estado, proximoMantenimientoKm, taller, responsable)
                    VALUES (@idAuto, @tipo, @descripcion, @costo, 
                            @kilometrajeProgramado, @fechaProgramada, 
                            @estado, @proximoMantenimientoKm, @taller, @responsable)", cn))
                {
                    cmd.Parameters.AddWithValue("@idAuto", mantenimiento.idAuto);
                    cmd.Parameters.AddWithValue("@tipo", mantenimiento.tipo);
                    cmd.Parameters.AddWithValue("@descripcion", mantenimiento.descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", mantenimiento.costo);
                    cmd.Parameters.AddWithValue("@kilometrajeProgramado", mantenimiento.kilometrajeProgramado ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaProgramada", mantenimiento.fechaProgramada ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", mantenimiento.estado);
                    cmd.Parameters.AddWithValue("@proximoMantenimientoKm", mantenimiento.proximoMantenimientoKm ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@taller", mantenimiento.taller ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@responsable", mantenimiento.responsable ?? (object)DBNull.Value);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "OK";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

        public string Actualizar(Mantenimiento mantenimiento)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE tb_mantenimiento 
                    SET tipo = @tipo, descripcion = @descripcion, costo = @costo,
                        kilometrajeProgramado = @kilometrajeProgramado, 
                        fechaProgramada = @fechaProgramada, estado = @estado,
                        proximoMantenimientoKm = @proximoMantenimientoKm,
                        taller = @taller, responsable = @responsable
                    WHERE idMantenimiento = @idMantenimiento", cn))
                {
                    cmd.Parameters.AddWithValue("@idMantenimiento", mantenimiento.idMantenimiento);
                    cmd.Parameters.AddWithValue("@tipo", mantenimiento.tipo);
                    cmd.Parameters.AddWithValue("@descripcion", mantenimiento.descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", mantenimiento.costo);
                    cmd.Parameters.AddWithValue("@kilometrajeProgramado", mantenimiento.kilometrajeProgramado ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaProgramada", mantenimiento.fechaProgramada ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", mantenimiento.estado);
                    cmd.Parameters.AddWithValue("@proximoMantenimientoKm", mantenimiento.proximoMantenimientoKm ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@taller", mantenimiento.taller ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@responsable", mantenimiento.responsable ?? (object)DBNull.Value);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "OK";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

        public string ActualizarEstado(int idMantenimiento, string estado, DateTime? fechaRealizada = null)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE tb_mantenimiento 
                    SET estado = @estado, 
                        fechaRealizada = @fechaRealizada
                    WHERE idMantenimiento = @idMantenimiento", cn))
                {
                    cmd.Parameters.AddWithValue("@idMantenimiento", idMantenimiento);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@fechaRealizada", fechaRealizada ?? (object)DBNull.Value);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "OK";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

        public string Eliminar(int idMantenimiento)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("DELETE FROM tb_mantenimiento WHERE idMantenimiento = @idMantenimiento", cn))
                {
                    cmd.Parameters.AddWithValue("@idMantenimiento", idMantenimiento);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "OK";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

        private Mantenimiento MapearMantenimiento(SqlDataReader dr)
        {
            return new Mantenimiento
            {
                idMantenimiento = Convert.ToInt32(dr["idMantenimiento"]),
                idAuto = Convert.ToInt32(dr["idAuto"]),
                tipo = Convert.ToString(dr["tipo"]),
                descripcion = dr["descripcion"] != DBNull.Value ? Convert.ToString(dr["descripcion"]) : null,
                costo = Convert.ToDecimal(dr["costo"]),
                kilometrajeProgramado = dr["kilometrajeProgramado"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeProgramado"]) : null,
                fechaProgramada = dr["fechaProgramada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaProgramada"]) : null,
                fechaRealizada = dr["fechaRealizada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaRealizada"]) : null,
                estado = Convert.ToString(dr["estado"]),
                proximoMantenimientoKm = dr["proximoMantenimientoKm"] != DBNull.Value ? Convert.ToInt32(dr["proximoMantenimientoKm"]) : null,
                taller = dr["taller"] != DBNull.Value ? Convert.ToString(dr["taller"]) : null,
                responsable = dr["responsable"] != DBNull.Value ? Convert.ToString(dr["responsable"]) : null,
                fechaRegistro = dr["fechaRegistro"] != DBNull.Value ? Convert.ToDateTime(dr["fechaRegistro"]) : null,
                placaAuto = dr["placaAuto"] != DBNull.Value ? Convert.ToString(dr["placaAuto"]) : null,
                modeloAuto = dr["modeloAuto"] != DBNull.Value ? Convert.ToString(dr["modeloAuto"]) : null,
                kilometrajeActualAuto = dr["kilometrajeActualAuto"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeActualAuto"]) : null
            };
        }
    }
}
