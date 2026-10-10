using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class MarcaNegocio
    {
        private MarcaDatos datos = new MarcaDatos();

        public List<Marca> Listar()
        {
            return datos.Listar();
        }

        public bool Registrar(Marca marca)
        {
            return datos.Registrar(marca);
        }

        public bool Editar(Marca marca)
        {
            return datos.Editar(marca);
        }

        public bool Eliminar(int idMarca)
        {
            return datos.Eliminar(idMarca);
        }
    }
}