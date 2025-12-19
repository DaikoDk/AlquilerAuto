using Microsoft.Data.SqlClient;
using AlquilerAuto.Models;
using AlquilerAuto.Repositorio;
using System.Data;

namespace AlquilerAuto.DAO
{
    public class ClienteDAO : ICliente
    {
        string cadena = (new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build().GetConnectionString("cn") ?? "");
        public string actualizar(Cliente reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cliente_actualizar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idCliente", reg.idCliente);
                        cmd.Parameters.AddWithValue("@nombreApe", reg.nombreApe);
                        cmd.Parameters.AddWithValue("@dni", reg.dni);
                        cmd.Parameters.AddWithValue("@telefono", reg.telefono);
                        cmd.Parameters.AddWithValue("@email", reg.email);

                        cn.Open();

                        mensaje = cmd.ExecuteScalar().ToString() ?? "";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public string agregar(Cliente reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cliente_agregar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nombreApe", reg.nombreApe);
                        cmd.Parameters.AddWithValue("@dni", reg.dni);
                        cmd.Parameters.AddWithValue("@telefono", reg.telefono);
                        cmd.Parameters.AddWithValue("@email", reg.email);

                        cn.Open();
                        mensaje = cmd.ExecuteScalar().ToString() ?? "";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public Cliente buscar(int codigo)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("usp_cliente_buscar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", codigo);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Cliente()
                        {
                            idCliente = Convert.ToInt32(dr["idCliente"]),
                            nombreApe = Convert.ToString(dr["nombreApe"]),
                            dni = Convert.ToString(dr["dni"]),
                            telefono = Convert.ToString(dr["telefono"]),
                            email = Convert.ToString(dr["email"])
                        };
                    }
                }
            }
            return new Cliente(); // o return null; según tu preferencia
        }

        public string eliminar(Cliente reg)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cliente_eliminar", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idCliente", reg.idCliente);

                        cn.Open();
                        mensaje = cmd.ExecuteScalar().ToString() ?? "";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool existeDni(string dni)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM tb_cliente WHERE dni=@dni", cn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);
                cn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool existeEmail(string email)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM tb_cliente WHERE email=@email", cn))
    {
        cmd.Parameters.AddWithValue("@email", email);
        cn.Open();
        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }
        }

        public IEnumerable<Cliente> Listado()
        {
            List<Cliente> temporal = new List<Cliente>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_cliente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            temporal.Add(new Cliente()
                            {
                                idCliente = Convert.ToInt32(dr["idCliente"]),
                                nombreApe = Convert.ToString(dr["nombreApe"]),
                                dni = Convert.ToString(dr["dni"]),
                                telefono = Convert.ToString(dr["telefono"]),
                                email = Convert.ToString(dr["email"])
                            });
                        }
                    }
                }
            }
            return temporal;
        }
    }
}
