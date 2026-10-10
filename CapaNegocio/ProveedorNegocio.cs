using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{

    public class ProveedorNegocio
    {
        private ProveedorDatos objetoDatos = new ProveedorDatos();

        public DataTable Listar()
        {
            return objetoDatos.Listar();
        }
        public void Registrar(Proveedor proveedor)
        {
            objetoDatos.Registrar(proveedor);
        }
        public bool Editar(Proveedor proveedor)
        {
            return objetoDatos.Editar(proveedor);
        }
        public bool Eliminar(int idProveedor)
        {
            return objetoDatos.Eliminar(idProveedor);
        }
    }
}

