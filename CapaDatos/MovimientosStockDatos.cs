using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class MovimientosStockDatos
    {
        private Conexion conexion = new Conexion();

        public bool AjustarStock(int idProducto, string tipoMovimiento, int cantidad, string referencia, out int stockActualizado)
        {
            stockActualizado = 0;
            if (idProducto <= 0 || cantidad <= 0 ||
                (tipoMovimiento != "Entrada" && tipoMovimiento != "Salida"))
            {
                return false;
            }

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();
                using (SqlTransaction transaccion = cn.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (SqlCommand cmdStock = new SqlCommand(@"
UPDATE inventario WITH (UPDLOCK, HOLDLOCK)
SET stock_actual = CASE WHEN @tipo = 'Entrada'
                        THEN stock_actual + @cantidad
                        ELSE stock_actual - @cantidad END
OUTPUT inserted.stock_actual
WHERE id_producto = @id_producto
  AND (@tipo = 'Entrada' OR stock_actual >= @cantidad);", cn, transaccion))
                        {
                            cmdStock.Parameters.Add("@tipo", SqlDbType.VarChar, 20).Value = tipoMovimiento;
                            cmdStock.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                            cmdStock.Parameters.Add("@id_producto", SqlDbType.Int).Value = idProducto;
                            object resultado = cmdStock.ExecuteScalar();
                            if (resultado == null || resultado == DBNull.Value)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                            stockActualizado = Convert.ToInt32(resultado);
                        }

                        using (SqlCommand cmdMovimiento = new SqlCommand(@"
INSERT INTO movimientos_stock
    (id_producto, tipo_movimiento, cantidad, fecha, referencia)
VALUES
    (@id_producto, @tipo, @cantidad, GETDATE(), @referencia);", cn, transaccion))
                        {
                            cmdMovimiento.Parameters.Add("@id_producto", SqlDbType.Int).Value = idProducto;
                            cmdMovimiento.Parameters.Add("@tipo", SqlDbType.VarChar, 20).Value = tipoMovimiento;
                            cmdMovimiento.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;
                            cmdMovimiento.Parameters.Add("@referencia", SqlDbType.VarChar, 100).Value = referencia ?? string.Empty;
                            cmdMovimiento.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch
                    {
                        try { transaccion.Rollback(); }
                        catch { /* SQL Server puede haber revertido la transacción. */ }
                        throw;
                    }
                }
            }
        }

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
