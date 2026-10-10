using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Ventas
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
    }
}
