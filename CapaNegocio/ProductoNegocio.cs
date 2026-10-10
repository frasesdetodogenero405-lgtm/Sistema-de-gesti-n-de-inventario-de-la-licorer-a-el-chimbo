using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class ProductoNegocio
    {
        private ProductoDatos objetoDatos = new ProductoDatos();

        public List<Producto> Listar()
        {
            return objetoDatos.Listar();
        }
        public bool Registrar(Producto producto)
        {
            return objetoDatos.Registrar(producto);

        }
        public bool Editar(Producto producto)
        {
            return objetoDatos.Editar(producto);
        }
        public bool Eliminar(int idProducto)
        {
            return objetoDatos.Eliminar(idProducto);
        }
    }
}