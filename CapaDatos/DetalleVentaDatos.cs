using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class DetalleVentaDatos
    {
        public bool Registrar(Detalle_Ventas obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = "";

            try
            {
                Conexion conexion = new Conexion();

                using (SqlConnection oconexion = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRAR_DETALLE_VENTA", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdVenta", obj.IdVenta);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioVenta", obj.PrecioVenta);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}
