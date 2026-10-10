using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Pagos_Venta
{
    public int id_pago { get; set; }

    public int id_venta { get; set; }

    public int id_metodo { get; set; }

    public decimal monto { get; set; }

    public DateTime? fecha_pago { get; set; }

    public virtual Metodos_Pago id_metodoNavigation { get; set; } = null!;

    public virtual Venta id_ventaNavigation { get; set; } = null!;
}
