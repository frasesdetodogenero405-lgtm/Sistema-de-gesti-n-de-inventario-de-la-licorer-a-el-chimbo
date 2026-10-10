using System;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class DetalleVentaNegocio
    {
        private DetalleVentaDatos objDetalleVentaDatos = new DetalleVentaDatos();

        public bool Registrar(Detalle_Ventas obj, out string Mensaje)
        {
            return objDetalleVentaDatos.Registrar(obj, out Mensaje);
        }
    }
}