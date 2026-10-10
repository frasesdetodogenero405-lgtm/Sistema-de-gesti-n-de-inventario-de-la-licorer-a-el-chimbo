using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RolesNegocio
    {
        private RolesDatos objetoDatos = new RolesDatos();

        public List<Roles> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Roles rol)
        {
            return objetoDatos.Registrar(rol);
        }

        public bool Editar(Roles rol)
        {
            return objetoDatos.Editar(rol);
        }

        public bool Eliminar(int id)
        {
            return objetoDatos.Eliminar(id);
        }
    }
}