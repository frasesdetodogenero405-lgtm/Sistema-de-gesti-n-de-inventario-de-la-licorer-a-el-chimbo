using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class PermisosDatos
    {
        private Conexion conexion = new Conexion();

        public List<Permisos> Listar()
        {
            List<Permisos> lista = new List<Permisos>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT id_permiso, id_rol, accion FROM permisos",
                    cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Permisos
                        {
                            IdPermiso = Convert.ToInt32(dr["id_permiso"]),
                            IdRol = Convert.ToInt32(dr["id_rol"]),
                            Accion = dr["accion"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Permisos permiso)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO permisos
                      (id_rol, accion)
                      VALUES
                      (@id_rol, @accion)",
                    cn);

                cmd.Parameters.AddWithValue("@id_rol", permiso.IdRol);
                cmd.Parameters.AddWithValue("@accion", permiso.Accion);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(Permisos permiso)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE permisos
                      SET id_rol = @id_rol,
                          accion = @accion
                      WHERE id_permiso = @id_permiso",
                    cn);

                cmd.Parameters.AddWithValue("@id_rol", permiso.IdRol);
                cmd.Parameters.AddWithValue("@accion", permiso.Accion);
                cmd.Parameters.AddWithValue("@id_permiso", permiso.IdPermiso);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM permisos WHERE id_permiso = @id_permiso",
                    cn);

                cmd.Parameters.AddWithValue("@id_permiso", id);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
