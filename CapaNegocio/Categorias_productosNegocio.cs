
using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CategoriaProductoNegocio
    {
        private CategoriaDatos objetoDatos = new CategoriaDatos();

        public List<Categorias_productos> Listar()
        {
            return objetoDatos.Listar();
        }

        public void Registrar(Categorias_productos categoria)
        {
            objetoDatos.Registrar(categoria);
        }

        public void Editar(Categorias_productos categoria)
        {
            objetoDatos.Editar(categoria);
        }

        public void Eliminar(int id)
        {
            objetoDatos.Eliminar(id);
        }
    }
}