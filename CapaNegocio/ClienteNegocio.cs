using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class ClienteNegocio
    {
        private ClienteDatos objetoDatos = new ClienteDatos();

        public List<Cliente> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Cliente cliente)
        {
            return objetoDatos.Registrar(cliente);
        }
        public bool Editar(Cliente cliente)
        {
            return objetoDatos.Editar(cliente);
        }
        public bool Eliminar(Cliente cliente)
        {
            return objetoDatos.Eliminar(cliente);
        }
    }
}