using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Pagos_Ventas
    {
        public int IdPago { get; set; }
        public int IdVenta { get; set; }
        public int IdMetodo { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
