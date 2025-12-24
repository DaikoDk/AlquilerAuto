using Microsoft.Data.SqlClient;
using AlquilerAuto.Repositorio;
using AlquilerAuto.ViewModels;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class DashboardDAO : IDashboard
    {
        string cadena = (new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("cn") ?? "");

        public IEnumerable<AutoDisponibleVM> ObtenerAutosDisponibles()
        {
            List<AutoDisponibleVM> lista = new List<AutoDisponibleVM>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM vw_autos_disponibles", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new AutoDisponibleVM
                        {
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            placa = Convert.ToString(dr["placa"]),
                            marca = Convert.ToString(dr["marca"]),
                            modelo = Convert.ToString(dr["modelo"]),
                            categoria = dr["categoria"] != DBNull.Value ? Convert.ToString(dr["categoria"]) : null,
                            anio = Convert.ToInt32(dr["anio"]),
                            color = dr["color"] != DBNull.Value ? Convert.ToString(dr["color"]) : null,
                            kilometrajeActual = Convert.ToInt32(dr["kilometrajeActual"]),
                            precioPorDia = Convert.ToDecimal(dr["precioPorDia"]),
                            precioPorHora = dr["precioPorHora"] != DBNull.Value ? Convert.ToDecimal(dr["precioPorHora"]) : null
                        });
                    }
                }
            }

            return lista;
        }

        public IEnumerable<ReservaActivaVM> ObtenerReservasActivas()
        {
            List<ReservaActivaVM> lista = new List<ReservaActivaVM>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM vw_reservas_activas", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ReservaActivaVM
                        {
                            idReserva = Convert.ToInt32(dr["idReserva"]),
                            cliente = Convert.ToString(dr["cliente"]),
                            dni = Convert.ToString(dr["dni"]),
                            telefono = Convert.ToString(dr["telefono"]),
                            auto = Convert.ToString(dr["auto"]),
                            placa = Convert.ToString(dr["placa"]),
                            fechaInicio = Convert.ToDateTime(dr["fechaInicio"]),
                            horaInicio = (TimeSpan)dr["horaInicio"],
                            fechaFin = Convert.ToDateTime(dr["fechaFin"]),
                            horaFin = (TimeSpan)dr["horaFin"],
                            estado = Convert.ToString(dr["estado"]),
                            diasRestantes = Convert.ToInt32(dr["diasRestantes"])
                        });
                    }
                }
            }

            return lista;
        }

        public IEnumerable<AutoMantenimientoVM> ObtenerAutosMantenimiento()
        {
            List<AutoMantenimientoVM> lista = new List<AutoMantenimientoVM>();

            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM vw_autos_mantenimiento", cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new AutoMantenimientoVM
                        {
                            idAuto = Convert.ToInt32(dr["idAuto"]),
                            placa = Convert.ToString(dr["placa"]),
                            marca = Convert.ToString(dr["marca"]),
                            modelo = Convert.ToString(dr["modelo"]),
                            kilometrajeActual = Convert.ToInt32(dr["kilometrajeActual"]),
                            ultimaRevisionKm = dr["ultimaRevisionKm"] != DBNull.Value ? Convert.ToInt32(dr["ultimaRevisionKm"]) : null,
                            proximaRevisionKm = dr["proximaRevisionKm"] != DBNull.Value ? Convert.ToInt32(dr["proximaRevisionKm"]) : null,
                            kmDesdeUltimaRevision = Convert.ToInt32(dr["kmDesdeUltimaRevision"]),
                            estadoMantenimiento = Convert.ToString(dr["estadoMantenimiento"])
                        });
                    }
                }
            }

            return lista;
        }

        public int ContarAutos()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_auto WHERE activo = 1", cn))
            {
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int ContarAutosDisponibles()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_auto WHERE estado = 'Disponible' AND activo = 1", cn))
            {
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int ContarAutosAlquilados()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_auto WHERE estado = 'Alquilado' AND activo = 1", cn))
            {
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int ContarClientes()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_cliente WHERE activo = 1", cn))
            {
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public int ContarReservasActivas()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tb_reserva WHERE estado IN ('Reservado', 'Alquilado')", cn))
            {
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public decimal ObtenerIngresosMes()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(total), 0) 
                FROM tb_reserva 
                WHERE estado = 'Finalizado' 
                AND MONTH(fechaFinalizacion) = MONTH(GETDATE()) 
                AND YEAR(fechaFinalizacion) = YEAR(GETDATE())", cn))
            {
                cn.Open();
                return (decimal)cmd.ExecuteScalar();
            }
        }
    }
}
