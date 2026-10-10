using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class ComprasDatos
    {
        private Conexion conexion = new Conexion();

        public List<Compras> Listar()
        {
            List<Compras> lista = new List<Compras>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"
                    SELECT
                        c.id_compra,
                        c.id_proveedor,
                        c.id_usuario,
                        c.fecha,
                        c.total
                    FROM compras c
                    ORDER BY c.id_compra DESC";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Compras
                        {
                            IdCompra = Convert.ToInt32(dr["id_compra"]),
                            IdProveedor = Convert.ToInt32(dr["id_proveedor"]),
                            IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                            Fecha = Convert.ToDateTime(dr["fecha"]),
                            Total = Convert.ToDecimal(dr["total"])
                        });
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Compras compra)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO compras
            (id_proveedor, id_usuario, fecha, total)
            VALUES
            (@id_proveedor, @id_usuario, @fecha, @total);

            SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_proveedor", compra.IdProveedor);
                cmd.Parameters.AddWithValue("@id_usuario", compra.IdUsuario);
                cmd.Parameters.AddWithValue("@fecha", compra.Fecha);
                cmd.Parameters.AddWithValue("@total", compra.Total);

                cn.Open();

                object resultado = cmd.ExecuteScalar();

                if (resultado != null)
                {
                    compra.IdCompra = Convert.ToInt32(resultado);
                    respuesta = true;
                }
            }

            return respuesta;
        }
    }
}