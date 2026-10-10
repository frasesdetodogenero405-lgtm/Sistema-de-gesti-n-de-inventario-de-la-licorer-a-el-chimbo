using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class MovimientosStockNegocio
    {
        private MovimientosStockDatos objetoDatos = new MovimientosStockDatos();

        public bool AjustarStock(int idProducto, string tipoMovimiento, int cantidad, string referencia, out int stockActualizado)
        {
            return objetoDatos.AjustarStock(idProducto, tipoMovimiento, cantidad, referencia, out stockActualizado);
        }

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
