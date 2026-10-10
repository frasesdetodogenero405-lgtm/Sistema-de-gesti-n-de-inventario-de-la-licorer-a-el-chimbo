using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class ProveedorDatos
    {
        private Conexion conexion = new Conexion();

        public DataTable Listar()

        {

            DataTable tabla = new DataTable();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                string consulta = "SELECT * FROM proveedores";

                SqlCommand cmd = new SqlCommand(consulta, cn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(tabla);
            }

            return tabla;
        }
        public void Registrar(Proveedor proveedor)
            {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = "INSERT INTO proveedores (nombre_proveedor, telefono, direccion) " +
                                  "VALUES (@nombre, @telefono, @direccion)";

                SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@nombre", proveedor.NombreProveedor);
                cmd.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                cmd.Parameters.AddWithValue("@direccion", proveedor.Direccion);

                cmd.ExecuteNonQuery();


            }
            }
        public bool Editar(Proveedor proveedor)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = "UPDATE proveedores SET nombre_proveedor = @nombre, telefono = @telefono, direccion = @direccion " +
                                  "WHERE id_proveedor = @id_proveedor";

                SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@id_proveedor", proveedor.IdProveedor);
                cmd.Parameters.AddWithValue("@nombre", proveedor.NombreProveedor);
                cmd.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                cmd.Parameters.AddWithValue("@direccion", proveedor.Direccion);

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }
        public bool Eliminar(int idProveedor)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string consulta = "DELETE FROM proveedores WHERE id_proveedor = @id_proveedor";

                SqlCommand cmd = new SqlCommand(consulta, cn);

                cmd.Parameters.AddWithValue("@id_proveedor", idProveedor);

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }
    }
}
 
