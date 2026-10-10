using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class InventarioDatos
    {
        public bool Registrar(Inventario inventario)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO inventario
                         (id_producto, stock_minimo, stock_maximo, stock_actual)
                         VALUES
                         (@id_producto, @stock_minimo, @stock_maximo, @stock_actual)";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_producto", inventario.IdProducto);
                cmd.Parameters.AddWithValue("@stock_minimo", inventario.StockMinimo);
                cmd.Parameters.AddWithValue("@stock_maximo", inventario.StockMaximo);
                cmd.Parameters.AddWithValue("@stock_actual", inventario.StockActual);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }

        public bool DescontarStock(int idProducto, int cantidad)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"UPDATE inventario
                         SET stock_actual = stock_actual - @cantidad
                         WHERE id_producto = @id_producto
                         AND stock_actual >= @cantidad";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_producto", idProducto);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }
        public bool Editar(Inventario inventario)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"UPDATE inventario
                         SET id_producto = @id_producto,
                             stock_minimo = @stock_minimo,
                             stock_maximo = @stock_maximo,
                             stock_actual = @stock_actual
                         WHERE id_inventario = @id_inventario";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_producto", inventario.IdProducto);
                cmd.Parameters.AddWithValue("@stock_minimo", inventario.StockMinimo);
                cmd.Parameters.AddWithValue("@stock_maximo", inventario.StockMaximo);
                cmd.Parameters.AddWithValue("@stock_actual", inventario.StockActual);
                cmd.Parameters.AddWithValue("@id_inventario", inventario.IdInventario);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }
        private Conexion conexion = new Conexion();
        public bool Eliminar(int idInventario)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"DELETE FROM inventario
                                 WHERE id_inventario = @id_inventario";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_inventario", idInventario);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }

        public List<Inventario> Listar()
        {
            List<Inventario> lista = new List<Inventario>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(@"
    SELECT 
        i.id_inventario,
        i.id_producto,
        p.nombre_producto,
        i.stock_minimo,
        i.stock_maximo,
        i.stock_actual
    FROM inventario i
    INNER JOIN productos p ON i.id_producto = p.id_producto", con);
                cmd.CommandType = CommandType.Text;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Inventario
                        {
                            IdInventario = Convert.ToInt32(dr["id_inventario"]),
                            IdProducto = Convert.ToInt32(dr["id_producto"]),
                            NombreProducto = dr["nombre_producto"].ToString(),
                            StockMinimo = Convert.ToInt32(dr["stock_minimo"]),
                            StockMaximo = Convert.ToInt32(dr["stock_maximo"]),
                            StockActual = Convert.ToInt32(dr["stock_actual"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}
