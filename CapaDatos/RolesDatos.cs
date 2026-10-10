using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class RolesDatos
    {
        private Conexion conexion = new Conexion();

        public List<Roles> Listar()
        {
            List<Roles> lista = new List<Roles>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT id_rol, nombre_rol FROM roles_usuarios",
                    cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Roles
                        {
                            IdRol = Convert.ToInt32(dr["id_rol"]),
                            NombreRol = dr["nombre_rol"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Roles rol)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO roles_usuarios (nombre_rol) VALUES (@nombre_rol)",
                    cn);

                cmd.Parameters.AddWithValue("@nombre_rol", rol.NombreRol);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(Roles rol)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE roles_usuarios
                      SET nombre_rol = @nombre_rol
                      WHERE id_rol = @id_rol",
                    cn);

                cmd.Parameters.AddWithValue("@nombre_rol", rol.NombreRol);
                cmd.Parameters.AddWithValue("@id_rol", rol.IdRol);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM roles_usuarios WHERE id_rol = @id_rol",
                    cn);

                cmd.Parameters.AddWithValue("@id_rol", id);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}