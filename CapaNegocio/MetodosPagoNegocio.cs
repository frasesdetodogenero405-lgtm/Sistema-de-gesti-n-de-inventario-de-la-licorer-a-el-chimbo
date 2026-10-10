using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class MetodosPagoNegocio
    {
        private MetodosPagoDatos objetoDatos = new MetodosPagoDatos();

        public List<Metodos_Pago> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Metodos_Pago metodo)
        {
            return objetoDatos.Registrar(metodo);
        }

        public bool Editar(Metodos_Pago metodo)
        {
            return objetoDatos.Editar(metodo);
        }

        public bool Eliminar(int id)
        {
            return objetoDatos.Eliminar(id);
        }
    }
}