using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class ProductoDatos
    {
        private Conexion conexion = new Conexion();

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"SELECT 
                                    id_producto,
                                    nombre_producto,
                                    precio_venta,
                                    id_categoria,
                                    id_marca
                                 FROM productos";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Producto producto = new Producto();

                            producto.IdProducto = Convert.ToInt32(dr["id_producto"]);
                            producto.NombreProducto = dr["nombre_producto"].ToString();
                            producto.PrecioVenta = Convert.ToDecimal(dr["precio_venta"]);
                            producto.IdCategoria = Convert.ToInt32(dr["id_categoria"]);
                            producto.IdMarca = Convert.ToInt32(dr["id_marca"]);

                            lista.Add(producto);
                        }
                    }
                }
            }

            return lista;
        }
        public bool Registrar(Producto producto)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO productos (nombre_producto, precio_venta, id_categoria, id_marca) " +
                    "VALUES (@nombre, @precio, @categoria, @marca)", cn);

                cmd.Parameters.AddWithValue("@nombre", producto.NombreProducto);
                cmd.Parameters.AddWithValue("@precio", producto.PrecioVenta);
                cmd.Parameters.AddWithValue("@categoria", producto.IdCategoria);
                cmd.Parameters.AddWithValue("@marca", producto.IdMarca);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }


            return respuesta;
        }
        public bool Editar(Producto producto)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                try
                {
                    string query = @"UPDATE productos
                             SET nombre_producto = @nombre_producto,
                                 precio_venta = @precio_venta,
                                 id_categoria = @id_categoria,
                                 id_marca = @id_marca
                             WHERE id_producto = @id_producto";

                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@nombre_producto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@precio_venta", producto.PrecioVenta);
                    cmd.Parameters.AddWithValue("@id_categoria", producto.IdCategoria);
                    cmd.Parameters.AddWithValue("@id_marca", producto.IdMarca);
                    cmd.Parameters.AddWithValue("@id_producto", producto.IdProducto);

                    cn.Open();

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        respuesta = true;
                    }
                }
                catch (Exception)
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }
        public bool Eliminar(int idProducto)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM productos WHERE id_producto = @id",
                    cn);

                cmd.Parameters.AddWithValue("@id", idProducto);

                cn.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }
    }
}
