using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Sistema_Licoreria_El_Chimbo
{
    public class Conexion
    {
        private readonly string cadenaConexion;

        public Conexion()
        {
            ConnectionStringSettings configuracion =
                ConfigurationManager.ConnectionStrings["LicoreriaDb"];

            if (configuracion == null || string.IsNullOrWhiteSpace(configuracion.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión 'LicoreriaDb' en App.config.");
            }

            cadenaConexion = configuracion.ConnectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        public bool ProbarConexion()
        {
            try
            {
                using (SqlConnection conexion = ObtenerConexion())
                {
                    conexion.Open();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
