using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Movimientos_Stock
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }
    }
}
