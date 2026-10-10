using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class DetalleComprasNegocio
    {
        private DetalleComprasDatos objetoDatos = new DetalleComprasDatos();

        public bool Registrar(Detalle_Compras detalle)
        {
            return objetoDatos.Registrar(detalle);
        }
    }
}