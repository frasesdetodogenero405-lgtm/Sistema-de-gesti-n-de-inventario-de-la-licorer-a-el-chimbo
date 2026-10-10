using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioVenta { get; set; }
        public int IdCategoria { get; set; }
        public int IdMarca { get; set; }
    }
}
