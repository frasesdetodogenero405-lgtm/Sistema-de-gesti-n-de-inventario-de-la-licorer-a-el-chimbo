using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;


namespace CapaDatos
{
    public class DetalleComprasDatos
    {
        private Conexion conexion = new Conexion();

        public bool Registrar(Detalle_Compras detalle)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                SqlTransaction transaccion = cn.BeginTransaction();

                try
                {
                    // 1. Registrar el detalle de la compra
                    string queryDetalle = @"
                        INSERT INTO Detalle_Compras
                        (id_compra, id_producto, cantidad, precio_compra)
                        VALUES
                        (@id_compra, @id_producto, @cantidad, @precio_compra)";

                    using (SqlCommand cmdDetalle = new SqlCommand(queryDetalle, cn, transaccion))
                    {
                        cmdDetalle.Parameters.AddWithValue("@id_compra", detalle.IdCompra);
                        cmdDetalle.Parameters.AddWithValue("@id_producto", detalle.IdProducto);
                        cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmdDetalle.Parameters.AddWithValue("@precio_compra", detalle.PrecioCompra);

                        cmdDetalle.ExecuteNonQuery();
                    }

                    // 2. Aumentar el stock actual
                    string queryStock = @"
                        UPDATE inventario
                        SET stock_actual = stock_actual + @cantidad
                        WHERE id_producto = @id_producto";

                    using (SqlCommand cmdStock = new SqlCommand(queryStock, cn, transaccion))
                    {
                        cmdStock.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmdStock.Parameters.AddWithValue("@id_producto", detalle.IdProducto);

                        cmdStock.ExecuteNonQuery();
                    }

                    // 3. Registrar el movimiento de entrada
                    string queryMovimiento = @"
                        INSERT INTO movimientos_stock
                        (id_producto, tipo_movimiento, cantidad, fecha, referencia)
                        VALUES
                        (@id_producto, 'Entrada', @cantidad, GETDATE(), @referencia)";

                    using (SqlCommand cmdMovimiento = new SqlCommand(queryMovimiento, cn, transaccion))
                    {
                        cmdMovimiento.Parameters.AddWithValue("@id_producto", detalle.IdProducto);
                        cmdMovimiento.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        int numeroCompra = Convert.ToInt32(
                        new SqlCommand("SELECT COUNT(*) FROM compras WHERE id_compra <= " + detalle.IdCompra, cn, transaccion).ExecuteScalar()
                    );

                        cmdMovimiento.Parameters.AddWithValue(
                            "@referencia",
                            "Compra #" + numeroCompra
                        );

                        cmdMovimiento.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                    respuesta = true;
                }
                catch
                {
                    transaccion.Rollback();
                    respuesta = false;
                }
            }

            return respuesta;
        }
    }
}