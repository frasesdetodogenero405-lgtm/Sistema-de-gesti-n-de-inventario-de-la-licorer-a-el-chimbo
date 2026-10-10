using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Inventario
    {
        public int IdInventario { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public int StockActual { get; set; }
        public string Estado { get; set; }
    }
}
