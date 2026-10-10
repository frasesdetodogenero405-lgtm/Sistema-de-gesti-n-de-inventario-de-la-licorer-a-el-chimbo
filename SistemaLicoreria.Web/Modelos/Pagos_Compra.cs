using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Pagos_Compra
{
    public int id_pago_compra { get; set; }

    public int id_compra { get; set; }

    public int id_metodo { get; set; }

    public decimal monto { get; set; }

    public DateTime? fecha_pago { get; set; }

    public virtual Compra id_compraNavigation { get; set; } = null!;

    public virtual Metodos_Pago id_metodoNavigation { get; set; } = null!;
}
