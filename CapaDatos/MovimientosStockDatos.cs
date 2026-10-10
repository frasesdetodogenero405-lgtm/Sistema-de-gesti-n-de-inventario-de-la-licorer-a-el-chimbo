using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class MovimientosStockDatos
    {
        private Conexion conexion = new Conexion();

        public bool Registrar(Movimientos_Stock movimiento)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO movimientos_stock
                                 (id_producto, tipo_movimiento, cantidad, fecha, referencia)
                                 VALUES
                                 (@id_producto, @tipo_movimiento, @cantidad, @fecha, @referencia)";

                SqlCommand cmd = new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue("@id_producto", movimiento.IdProducto);
                cmd.Parameters.AddWithValue("@tipo_movimiento", movimiento.TipoMovimiento);
                cmd.Parameters.AddWithValue("@cantidad", movimiento.Cantidad);
                cmd.Parameters.AddWithValue("@fecha", movimiento.Fecha);
                cmd.Parameters.AddWithValue("@referencia", movimiento.Referencia);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }
        public List<Movimientos_Stock> Listar()
        {
            List<Movimientos_Stock> lista = new List<Movimientos_Stock>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string query = @"SELECT 
                            ms.id_movimiento,
                            ms.id_producto,
                            p.nombre_producto,
                            ms.tipo_movimiento,
                            ms.cantidad,
                            ms.fecha,
                            ms.referencia
                         FROM movimientos_stock ms
                         INNER JOIN productos p 
                            ON ms.id_producto = p.id_producto
                         ORDER BY ms.fecha DESC";

                SqlCommand cmd = new SqlCommand(query, cn);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Movimientos_Stock movimiento = new Movimientos_Stock();

                        movimiento.IdMovimiento = Convert.ToInt32(dr["id_movimiento"]);
                        movimiento.IdProducto = Convert.ToInt32(dr["id_producto"]);
                        movimiento.NombreProducto = dr["nombre_producto"].ToString();
                        movimiento.TipoMovimiento = dr["tipo_movimiento"].ToString();
                        movimiento.Cantidad = Convert.ToInt32(dr["cantidad"]);
                        movimiento.Fecha = Convert.ToDateTime(dr["fecha"]);
                        movimiento.Referencia = dr["referencia"].ToString();

                        lista.Add(movimiento);
                    }
                }
            }

            return lista;
        }
    }
}