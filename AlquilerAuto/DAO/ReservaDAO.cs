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
                    cmd.Parameters.AddWithValue("@fechaFin", reg.fechaFin);
                    cmd.Parameters.AddWithValue("@total", reg.total);
                    cmd.Parameters.AddWithValue("@estado", reg.estado);

                    cn.Open();
                    mensaje = cmd.ExecuteScalar().ToString()??"";
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
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            total = Convert.ToDecimal(dr["total"]),
                            estado = Convert.ToString(dr["estado"])
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
            using (SqlCommand cmd = new SqlCommand("usp_reserva", cn))
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
                            fechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            total = Convert.ToDecimal(dr["total"]),
                            estado = Convert.ToString(dr["estado"])
                        });
                    }
                }
            }
            return lista;
        }
        public ReservaDetalleVM BuscarDetalle(int idReserva)
        {
            ReservaDetalleVM reg = null;

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
                                FechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                                FechaFin = Convert.ToDateTime(dr["fechaFin"]),
                                nombreCliente = Convert.ToString(dr["nombreCliente"]),
                                dniCliente = Convert.ToString(dr["dniCliente"]),
                                placaAuto = Convert.ToString(dr["placaAuto"]),
                                modeloAuto = Convert.ToString(dr["modeloAuto"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error
                reg = new ReservaDetalleVM(); // Devuelve vacío si falla
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
       
    }
}
