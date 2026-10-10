using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
  public class Pagos_Compras
    {
        public int IdPagoCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdMetodo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
