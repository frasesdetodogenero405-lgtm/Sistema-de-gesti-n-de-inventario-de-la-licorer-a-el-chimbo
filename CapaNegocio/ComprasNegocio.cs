using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class ComprasNegocio
    {
        private ComprasDatos objetoDatos = new ComprasDatos();

        public List<Compras> Listar()
        {
            return objetoDatos.Listar();
        }

        public bool Registrar(Compras compra, IList<Detalle_Compras> detalles, out string mensaje)
        {
            return objetoDatos.Registrar(compra, detalles, out mensaje);
        }
    }
}
