using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class VentaDatos
    {
        public int Registrar(
            Ventas venta,
            IList<Detalle_Ventas> detalles,
            int idMetodoPago,
            out string mensaje)
        {
            mensaje = string.Empty;

            if (venta == null)
            {
                mensaje = "No se recibieron los datos de la venta.";
                return 0;
            }

            if (detalles == null || detalles.Count == 0)
            {
                mensaje = "Agregue al menos un producto a la venta.";
                return 0;
            }

            if (venta.IdCliente <= 0 || venta.IdUsuario <= 0 || idMetodoPago <= 0 ||
                string.IsNullOrWhiteSpace(venta.MetodoPago))
            {
                mensaje = "Seleccione un cliente, usuario y método de pago válidos.";
                return 0;
            }

            decimal total = 0m;
            foreach (Detalle_Ventas detalle in detalles)
            {
                if (detalle == null || detalle.IdProducto <= 0 ||
                    detalle.Cantidad <= 0 || detalle.PrecioVenta <= 0m)
                {
                    mensaje = "La venta contiene un producto, cantidad o precio inválido.";
                    return 0;
                }

                total += detalle.Cantidad * detalle.PrecioVenta;
            }

            if (total <= 0m)
            {
                mensaje = "El total de la venta debe ser mayor que cero.";
                return 0;
            }

            venta.Total = total;
            if (venta.Fecha == DateTime.MinValue)
            {
                venta.Fecha = DateTime.Now;
            }

            try
            {
                Conexion conexion = new Conexion();
                using (SqlConnection cn = conexion.ObtenerConexion())
                {
                    cn.Open();
                    using (SqlTransaction transaccion = cn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            int idVenta;
                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO ventas (id_cliente, id_usuario, fecha, total, metodo_pago)
VALUES (@id_cliente, @id_usuario, @fecha, @total, @metodo_pago);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, transaccion))
                            {
                                cmd.Parameters.Add("@id_cliente", SqlDbType.Int).Value = venta.IdCliente;
                                cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = venta.IdUsuario;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = venta.Fecha;

                                SqlParameter totalParam = cmd.Parameters.Add("@total", SqlDbType.Decimal);
                                totalParam.Precision = 18;
                                totalParam.Scale = 2;
                                totalParam.Value = total;

                                cmd.Parameters.Add("@metodo_pago", SqlDbType.VarChar, 50).Value = venta.MetodoPago;
                                idVenta = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            foreach (Detalle_Ventas detalle in detalles)
                            {
                                using (SqlCommand cmd = new SqlCommand(@"
UPDATE inventario WITH (UPDLOCK, HOLDLOCK)
SET stock_actual = stock_actual - @cantidad
WHERE id_producto = @id_producto AND stock_actual >= @cantidad;", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    if (cmd.ExecuteNonQuery() != 1)
                                    {
                                        throw new InvalidOperationException(
                                            "No hay stock suficiente para el producto " + detalle.IdProducto +
                                            " o el producto no tiene inventario registrado.");
                                    }
                                }

                                using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO detalle_ventas (id_venta, id_producto, cantidad, precio_venta)
VALUES (@id_venta, @id_producto, @cantidad, @precio_venta);", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_venta", SqlDbType.Int).Value = idVenta;
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    SqlParameter precioParam = cmd.Parameters.Add("@precio_venta", SqlDbType.Decimal);
                                    precioParam.Precision = 18;
                                    precioParam.Scale = 2;
                                    precioParam.Value = detalle.PrecioVenta;
                                    cmd.ExecuteNonQuery();
                                }

                                using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO movimientos_stock
    (id_producto, tipo_movimiento, cantidad, fecha, referencia)
VALUES
    (@id_producto, 'salida', @cantidad, @fecha, @referencia);", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = venta.Fecha;
                                    cmd.Parameters.Add("@referencia", SqlDbType.VarChar, 255).Value = "Venta #" + idVenta;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO pagos_ventas (id_venta, id_metodo, monto, fecha_pago)
VALUES (@id_venta, @id_metodo, @monto, @fecha_pago);", cn, transaccion))
                            {
                                cmd.Parameters.Add("@id_venta", SqlDbType.Int).Value = idVenta;
                                cmd.Parameters.Add("@id_metodo", SqlDbType.Int).Value = idMetodoPago;
                                SqlParameter montoParam = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
                                montoParam.Precision = 18;
                                montoParam.Scale = 2;
                                montoParam.Value = total;
                                cmd.Parameters.Add("@fecha_pago", SqlDbType.DateTime).Value = venta.Fecha;
                                cmd.ExecuteNonQuery();
                            }

                            transaccion.Commit();
                            mensaje = "Venta registrada correctamente.";
                            return idVenta;
                        }
                        catch
                        {
                            try
                            {
                                transaccion.Rollback();
                            }
                            catch
                            {
                                // SQL Server may already have rolled back a failed transaction.
                            }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return 0;
            }
        }
    }
}
