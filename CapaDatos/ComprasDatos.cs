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
        private readonly Conexion conexion = new Conexion();

        public List<Compras> Listar()
        {
            List<Compras> lista = new List<Compras>();
            using (SqlConnection cn = conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(@"
SELECT id_compra, id_proveedor, id_usuario, fecha, total
FROM compras
ORDER BY id_compra DESC;", cn))
            {
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

        public bool Registrar(Compras compra, IList<Detalle_Compras> detalles, out string mensaje)
        {
            mensaje = string.Empty;
            if (compra == null || detalles == null || detalles.Count == 0)
            {
                mensaje = "Agregue al menos un producto a la compra.";
                return false;
            }

            if (compra.IdProveedor <= 0 || compra.IdUsuario <= 0)
            {
                mensaje = "Seleccione un proveedor y un usuario válidos.";
                return false;
            }

            decimal total = 0m;
            foreach (Detalle_Compras detalle in detalles)
            {
                if (detalle == null || detalle.IdProducto <= 0 || detalle.Cantidad <= 0 ||
                    detalle.PrecioCompra <= 0m || detalle.PrecioVenta <= 0m)
                {
                    mensaje = "La compra contiene un producto, cantidad o precio inválido.";
                    return false;
                }

                total += detalle.Cantidad * detalle.PrecioCompra;
            }

            compra.Total = total;
            if (compra.Fecha == DateTime.MinValue)
            {
                compra.Fecha = DateTime.Now;
            }

            try
            {
                using (SqlConnection cn = conexion.ObtenerConexion())
                {
                    cn.Open();
                    using (SqlTransaction transaccion = cn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO compras (id_proveedor, id_usuario, fecha, total)
VALUES (@id_proveedor, @id_usuario, @fecha, @total);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, transaccion))
                            {
                                cmd.Parameters.Add("@id_proveedor", SqlDbType.Int).Value = compra.IdProveedor;
                                cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = compra.IdUsuario;
                                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = compra.Fecha;
                                SqlParameter totalParam = cmd.Parameters.Add("@total", SqlDbType.Decimal);
                                totalParam.Precision = 10;
                                totalParam.Scale = 2;
                                totalParam.Value = total;
                                compra.IdCompra = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            foreach (Detalle_Compras detalle in detalles)
                            {
                                using (SqlCommand cmd = new SqlCommand(@"
UPDATE inventario WITH (UPDLOCK, HOLDLOCK)
SET stock_actual = stock_actual + @cantidad
WHERE id_producto = @id_producto;", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    if (cmd.ExecuteNonQuery() != 1)
                                    {
                                        throw new InvalidOperationException(
                                            "No existe un registro de inventario para el producto " + detalle.IdProducto + ".");
                                    }
                                }

                                using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO detalle_compras (id_compra, id_producto, cantidad, precio_compra)
VALUES (@id_compra, @id_producto, @cantidad, @precio_compra);", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_compra", SqlDbType.Int).Value = compra.IdCompra;
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    SqlParameter precioParam = cmd.Parameters.Add("@precio_compra", SqlDbType.Decimal);
                                    precioParam.Precision = 10;
                                    precioParam.Scale = 2;
                                    precioParam.Value = detalle.PrecioCompra;
                                    cmd.ExecuteNonQuery();
                                }

                                using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO movimientos_stock
    (id_producto, tipo_movimiento, cantidad, fecha, referencia)
VALUES
    (@id_producto, 'entrada', @cantidad, @fecha, @referencia);", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = compra.Fecha;
                                    cmd.Parameters.Add("@referencia", SqlDbType.VarChar, 100).Value = "Compra #" + compra.IdCompra;
                                    cmd.ExecuteNonQuery();
                                }

                                using (SqlCommand cmd = new SqlCommand(@"
UPDATE productos SET precio_venta = @precio_venta WHERE id_producto = @id_producto;", cn, transaccion))
                                {
                                    cmd.Parameters.Add("@id_producto", SqlDbType.Int).Value = detalle.IdProducto;
                                    SqlParameter precioParam = cmd.Parameters.Add("@precio_venta", SqlDbType.Decimal);
                                    precioParam.Precision = 10;
                                    precioParam.Scale = 2;
                                    precioParam.Value = detalle.PrecioVenta;
                                    if (cmd.ExecuteNonQuery() != 1)
                                    {
                                        throw new InvalidOperationException(
                                            "No se pudo actualizar el precio del producto " + detalle.IdProducto + ".");
                                    }
                                }
                            }

                            transaccion.Commit();
                            mensaje = "Compra registrada correctamente.";
                            return true;
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
                return false;
            }
        }
    }
}
