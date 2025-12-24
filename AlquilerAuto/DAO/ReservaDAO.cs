using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;
using AlquilerAuto.ViewModels;

namespace AlquilerAuto.DAO
{
    public class ReservaDAO : IReserva
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");
        public string agregar(Reserva reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reserva_agregar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", reg.idCliente);
                    cmd.Parameters.AddWithValue("@idAuto", reg.idAuto);
                    cmd.Parameters.AddWithValue("@fechaInicio", reg.fechaInicio);
                    cmd.Parameters.AddWithValue("@horaInicio", reg.horaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", reg.fechaFin);
                    cmd.Parameters.AddWithValue("@horaFin", reg.horaFin);
                    cmd.Parameters.AddWithValue("@subtotal", reg.subtotal);
                    cmd.Parameters.AddWithValue("@usuarioCreacion", reg.usuarioCreacion ?? (object)DBNull.Value);

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

        public Reserva buscar(int codigo)
        {
            Reserva reg = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_reserva_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idReserva", codigo);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        reg = new Reserva()
                        {
                            idReserva = Convert.ToInt32(dr["idReserva"]),
                            idCliente = Convert.ToInt32(dr["idCliente"]),
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            fechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            horaInicio = (TimeSpan)dr["horaInicio"],
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            horaFin = (TimeSpan)dr["horaFin"],
                            kilometrajeInicio = dr["kilometrajeInicio"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeInicio"]) : null,
                            kilometrajeFin = dr["kilometrajeFin"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeFin"]) : null,
                            subtotal = Convert.ToDecimal(dr["subtotal"]),
                            mora = Convert.ToDecimal(dr["mora"]),
                            costoReparaciones = Convert.ToDecimal(dr["costoReparaciones"]),
                            total = Convert.ToDecimal(dr["total"]),
                            estado = Convert.ToString(dr["estado"]),
                            estadoEntrega = dr["estadoEntrega"] != DBNull.Value ? Convert.ToString(dr["estadoEntrega"]) : null
                        };
                    }
                }
            }
            return reg ?? new Reserva();
        }

        public string Cancelar(int idReserva)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reserva_cancelar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva",idReserva);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar().ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }            

        public string Finalizar(int idReserva)
        {
            string mensaje = "";
            try
            {
                using SqlConnection cn = new SqlConnection(cadena);
                using SqlCommand cmd = new SqlCommand("usp_reserva_finalizar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idReserva", idReserva);

                cn.Open();
                mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }
        public List<Reserva> listarReservados()
        {
            List<Reserva> lista = new List<Reserva>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_reserva_listar_reservados", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Reserva
                        {
                            idReserva = Convert.ToInt32(dr["idReserva"]),
                            cliente = Convert.ToString(dr["cliente"]),
                            placa = Convert.ToString(dr["placa"]),
                            estado = Convert.ToString(dr["estado"])
                        });
                    }
                }
            }
            return lista;
        }

        public IEnumerable<Reserva> Listado()
        {
            List<Reserva> lista = new List<Reserva>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_reserva_listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Reserva()
                        {
                            idReserva = Convert.ToInt32(dr["idReserva"]),
                            cliente = Convert.ToString(dr["cliente"]),
                            placa = Convert.ToString(dr["placa"]),
                            autoModelo = dr["autoModelo"] != DBNull.Value ? Convert.ToString(dr["autoModelo"]) : null,
                            fechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            horaInicio = (TimeSpan)dr["horaInicio"],
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            horaFin = (TimeSpan)dr["horaFin"],
                            subtotal = Convert.ToDecimal(dr["subtotal"]),
                            mora = Convert.ToDecimal(dr["mora"]),
                            costoReparaciones = Convert.ToDecimal(dr["costoReparaciones"]),
                            total = Convert.ToDecimal(dr["total"]),
                            estado = Convert.ToString(dr["estado"]),
                            estadoEntrega = dr["estadoEntrega"] != DBNull.Value ? Convert.ToString(dr["estadoEntrega"]) : null
                        });
                    }
                }
            }
            return lista;
        }
        public ReservaDetalleVM BuscarDetalle(int idReserva)
        {
            ReservaDetalleVM? reg = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reserva_buscar_detalle", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", idReserva);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            reg = new ReservaDetalleVM
                            {
                                IdReserva = Convert.ToInt32(dr["idReserva"]),
                                Estado = Convert.ToString(dr["estado"]),
                                EstadoEntrega = dr["estadoEntrega"] != DBNull.Value ? Convert.ToString(dr["estadoEntrega"]) : null,
                                FechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                                HoraInicio = (TimeSpan)dr["horaInicio"],
                                FechaFin = Convert.ToDateTime(dr["fechaFin"]),
                                HoraFin = (TimeSpan)dr["horaFin"],
                                FechaHoraInicioReal = dr["fechaHoraInicioReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaHoraInicioReal"]) : null,
                                FechaHoraFinReal = dr["fechaHoraFinReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaHoraFinReal"]) : null,
                                KilometrajeInicio = dr["kilometrajeInicio"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeInicio"]) : null,
                                KilometrajeFin = dr["kilometrajeFin"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeFin"]) : null,
                                KilometrosRecorridos = dr["kilometrosRecorridos"] != DBNull.Value ? Convert.ToInt32(dr["kilometrosRecorridos"]) : null,
                                Subtotal = Convert.ToDecimal(dr["subtotal"]),
                                Mora = Convert.ToDecimal(dr["mora"]),
                                CostoReparaciones = Convert.ToDecimal(dr["costoReparaciones"]),
                                Total = Convert.ToDecimal(dr["total"]),
                                ObservacionesEntrega = dr["observacionesEntrega"] != DBNull.Value ? Convert.ToString(dr["observacionesEntrega"]) : null,
                                nombreCliente = Convert.ToString(dr["nombreCliente"]),
                                dniCliente = Convert.ToString(dr["dniCliente"]),
                                placaAuto = Convert.ToString(dr["placaAuto"]),
                                modeloAuto = Convert.ToString(dr["autoModelo"]),
                                colorAuto = dr["color"] != DBNull.Value ? Convert.ToString(dr["color"]) : null,
                                kilometrajeActualAuto = dr["kilometrajeActual"] != DBNull.Value ? Convert.ToInt32(dr["kilometrajeActual"]) : null
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                // En caso de error, retornar objeto vacío
                reg = new ReservaDetalleVM();
            }
            return reg ?? new ReservaDetalleVM();
        }     
        public string eliminar(Reserva reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reserva_eliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", reg.idReserva);

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

        public string actualizar(Reserva reg)
        {
            throw new NotImplementedException();
        }

        public string IniciarAlquiler(int idReserva, string usuarioInicio)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                using (SqlCommand cmd = new SqlCommand("usp_reserva_iniciar_alquiler", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", idReserva);
                    cmd.Parameters.AddWithValue("@usuarioInicio", usuarioInicio ?? (object)DBNull.Value);

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

        public string Finalizar(int idReserva, int kilometrajeFin, string estadoEntrega, string observaciones, string usuarioFinalizacion)
        {
            string mensaje = "";
            try
            {
                using SqlConnection cn = new SqlConnection(cadena);
                using SqlCommand cmd = new SqlCommand("usp_reserva_finalizar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idReserva", idReserva);
                cmd.Parameters.AddWithValue("@kilometrajeFin", kilometrajeFin);
                cmd.Parameters.AddWithValue("@estadoEntrega", estadoEntrega);
                cmd.Parameters.AddWithValue("@observaciones", observaciones ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@usuarioFinalizacion", usuarioFinalizacion ?? (object)DBNull.Value);

                cn.Open();
                mensaje = cmd.ExecuteScalar()?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }

        public bool ValidarDisponibilidad(int idAuto, DateTime fechaInicio, TimeSpan horaInicio, DateTime fechaFin, TimeSpan horaFin, int? idReservaExcluir = null)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_reserva_validar_disponibilidad", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAuto", idAuto);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@horaInicio", horaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                cmd.Parameters.AddWithValue("@horaFin", horaFin);
                cmd.Parameters.AddWithValue("@idReservaExcluir", idReservaExcluir ?? (object)DBNull.Value);

                cn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToBoolean(result);
            }
        }
    }
}
