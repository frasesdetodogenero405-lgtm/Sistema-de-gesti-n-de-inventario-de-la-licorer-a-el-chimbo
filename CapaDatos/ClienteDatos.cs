using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ClienteDatos
    {
        private Conexion conexion = new Conexion();

        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "SELECT * FROM clientes";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente
                            {
                                IdCliente = Convert.ToInt32(dr["id_cliente"]),
                                NombreCliente = dr["nombre_cliente"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Direccion = dr["direccion"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Cliente cliente)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"INSERT INTO clientes 
                                (nombre_cliente, telefono, direccion)
                                VALUES 
                                (@nombre, @telefono, @direccion)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", cliente.NombreCliente);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;


        }
        public bool Editar(Cliente cliente)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"UPDATE clientes SET
                         nombre_cliente = @nombre,
                         telefono = @telefono,
                         direccion = @direccion
                         WHERE id_cliente = @id_cliente";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id_cliente", cliente.IdCliente);
                    cmd.Parameters.AddWithValue("@nombre", cliente.NombreCliente);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;
        }
        public bool Eliminar(Cliente cliente)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"DELETE FROM clientes
                         WHERE id_cliente = @id_cliente";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id_cliente", cliente.IdCliente);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;
        }
    }

}

