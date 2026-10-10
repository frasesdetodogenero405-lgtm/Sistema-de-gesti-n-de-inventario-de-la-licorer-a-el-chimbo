using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class UsuariosDatos
    {
        private Conexion conexion = new Conexion();

        public List<Usuarios> Listar()
        {
            List<Usuarios> lista = new List<Usuarios>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT id_usuario, nombre_usuario, contrasena, id_rol FROM usuarios",
                    cn);

                cmd.CommandType = CommandType.Text;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuarios
                        {
                            IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                            NombreUsuario = dr["nombre_usuario"].ToString(),
                            Contrasena = dr["contrasena"].ToString(),
                            IdRol = Convert.ToInt32(dr["id_rol"])
                        });
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Usuarios usuario)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO usuarios
                      (nombre_usuario, contrasena, id_rol)
                      VALUES
                      (@nombre_usuario, @contrasena, @id_rol)",
                    cn);

                cmd.Parameters.AddWithValue(
                    "@nombre_usuario", usuario.NombreUsuario);

                cmd.Parameters.AddWithValue(
                    "@contrasena", usuario.Contrasena);

                cmd.Parameters.AddWithValue(
                    "@id_rol", usuario.IdRol);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(Usuarios usuario)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE usuarios
                      SET nombre_usuario = @nombre_usuario,
                          contrasena = @contrasena,
                          id_rol = @id_rol
                      WHERE id_usuario = @id_usuario",
                    cn);

                cmd.Parameters.AddWithValue(
                    "@nombre_usuario", usuario.NombreUsuario);

                cmd.Parameters.AddWithValue(
                    "@contrasena", usuario.Contrasena);

                cmd.Parameters.AddWithValue(
                    "@id_rol", usuario.IdRol);

                cmd.Parameters.AddWithValue(
                    "@id_usuario", usuario.IdUsuario);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM usuarios WHERE id_usuario = @id_usuario",
                    cn);

                cmd.Parameters.AddWithValue("@id_usuario", id);

                cn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}