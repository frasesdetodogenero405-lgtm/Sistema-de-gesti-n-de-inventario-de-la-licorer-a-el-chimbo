using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class PermisosNegocio
    {
        private PermisosDatos objetoDatos = new PermisosDatos();

        public List<Permisos> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Permisos permiso)
        {
            return objetoDatos.Registrar(permiso);
        }

        public bool Editar(Permisos permiso)
        {
            return objetoDatos.Editar(permiso);
        }

        public bool Eliminar(int id)
        {
            return objetoDatos.Eliminar(id);
        }
    }
}