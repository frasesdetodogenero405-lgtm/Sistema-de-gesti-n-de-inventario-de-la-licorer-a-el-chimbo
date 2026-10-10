using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class VentaNegocio
    {
        private readonly VentaDatos objVentaDatos = new VentaDatos();

        public int Registrar(
            Ventas venta,
            IList<Detalle_Ventas> detalles,
            int idMetodoPago,
            out string mensaje)
        {
            return objVentaDatos.Registrar(venta, detalles, idMetodoPago, out mensaje);
        }
    }
}
