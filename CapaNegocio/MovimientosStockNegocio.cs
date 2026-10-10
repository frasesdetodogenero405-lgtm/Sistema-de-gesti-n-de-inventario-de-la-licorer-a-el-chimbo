using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class MovimientosStockNegocio
    {
        private MovimientosStockDatos objetoDatos = new MovimientosStockDatos();

        public bool Registrar(Movimientos_Stock movimiento)
        {
            return objetoDatos.Registrar(movimiento);
        }
        public List<Movimientos_Stock> Listar()
        {
            return objetoDatos.Listar();
        }
    }
}