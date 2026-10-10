using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class UsuariosNegocio
    {
        private UsuariosDatos objetoDatos = new UsuariosDatos();

        public List<Usuarios> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Usuarios usuario)
        {
            return objetoDatos.Registrar(usuario);
        }

        public bool Editar(Usuarios usuario)
        {
            return objetoDatos.Editar(usuario);
        }

        public bool Eliminar(int id)
        {
            return objetoDatos.Eliminar(id);
        }
    }
}