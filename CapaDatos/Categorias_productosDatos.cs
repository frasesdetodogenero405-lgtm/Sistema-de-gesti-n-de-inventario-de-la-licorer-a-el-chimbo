
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class CategoriaDatos
    {
        private Conexion conexion = new Conexion();

        public List<Categorias_productos> Listar()
        {
            List<Categorias_productos> lista = new List<Categorias_productos>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "SELECT id_categoria, nombre_categoria FROM Categorias_productos";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Categorias_productos categoria = new Categorias_productos();

                        categoria.IdCategoria = Convert.ToInt32(dr["id_categoria"]);
                        categoria.NombreCategoria = dr["nombre_categoria"].ToString();

                        lista.Add(categoria);
                    }
                }
            }

            return lista;
        }

        public void Registrar(Categorias_productos categoria)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "INSERT INTO Categorias_productos (nombre_categoria) VALUES (@nombre)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", categoria.NombreCategoria);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(Categorias_productos categoria)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"UPDATE Categorias_productos
                                 SET nombre_categoria = @nombre
                                 WHERE id_categoria = @id";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@nombre", categoria.NombreCategoria);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "DELETE FROM Categorias_productos WHERE id_categoria = @id";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}