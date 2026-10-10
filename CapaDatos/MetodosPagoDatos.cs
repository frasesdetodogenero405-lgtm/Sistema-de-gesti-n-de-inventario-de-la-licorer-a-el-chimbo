using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class MetodosPagoDatos
    {
        private Conexion conexion = new Conexion();

        public List<Metodos_Pago> Listar()
        {
            List<Metodos_Pago> lista = new List<Metodos_Pago>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand( "SELECT id_metodo, nombre_metodo FROM metodos_pago", cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Metodos_Pago
                        {
                            IdMetodoPago = Convert.ToInt32(dr["id_metodo"]),
                            NombreMetodo = dr["nombre_metodo"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Metodos_Pago metodo)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO metodos_pago (nombre_metodo) VALUES (@nombre)", cn);

                cmd.Parameters.AddWithValue("@nombre", metodo.NombreMetodo);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(Metodos_Pago metodo)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
              "UPDATE metodos_pago SET nombre_metodo = @nombre WHERE id_metodo = @id",
              cn);

                cmd.Parameters.AddWithValue("@nombre", metodo.NombreMetodo);
                cmd.Parameters.AddWithValue("@id", metodo.IdMetodoPago);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                "DELETE FROM metodos_pago WHERE id_metodo = @id", cn);

                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}