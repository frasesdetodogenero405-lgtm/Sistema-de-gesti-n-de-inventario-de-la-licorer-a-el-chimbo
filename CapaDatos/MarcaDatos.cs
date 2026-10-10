using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CapaEntidad;
using Sistema_Licoreria_El_Chimbo;

namespace CapaDatos
{
    public class MarcaDatos
    {
        private Conexion conexion = new Conexion();

        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "SELECT id_marca, nombre_marca FROM marcas";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Marca marca = new Marca();

                            marca.IdMarca = Convert.ToInt32(dr["id_marca"]);
                            marca.NombreMarca = dr["nombre_marca"].ToString();

                            lista.Add(marca);
                        }
                    }
                }
            }

            return lista;
        }

        public bool Registrar(Marca marca)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "INSERT INTO marcas (nombre_marca) VALUES (@nombre_marca)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre_marca", marca.NombreMarca);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;
        }

        public bool Editar(Marca marca)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "UPDATE marcas SET nombre_marca = @nombre_marca WHERE id_marca = @id_marca";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre_marca", marca.NombreMarca);
                    cmd.Parameters.AddWithValue("@id_marca", marca.IdMarca);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;
        }

        public bool Eliminar(int idMarca)
        {
            bool respuesta = false;

            using (SqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "DELETE FROM marcas WHERE id_marca = @id_marca";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id_marca", idMarca);

                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
            }

            return respuesta;
        }
    }
}
