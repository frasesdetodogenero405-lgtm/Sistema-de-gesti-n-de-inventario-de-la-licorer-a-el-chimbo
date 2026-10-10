using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
   public class Compras
    {
        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
