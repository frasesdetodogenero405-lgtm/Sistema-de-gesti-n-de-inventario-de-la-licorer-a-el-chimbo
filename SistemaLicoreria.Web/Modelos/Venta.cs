using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Venta
{
    public int id_venta { get; set; }

    public int id_cliente { get; set; }

    public int id_usuario { get; set; }

    public DateTime? fecha { get; set; }

    public decimal total { get; set; }

    public string metodo_pago { get; set; } = null!;

    public virtual ICollection<Detalle_Venta> Detalle_Venta { get; set; } = new List<Detalle_Venta>();

    public virtual ICollection<Pagos_Venta> Pagos_Venta { get; set; } = new List<Pagos_Venta>();

    public virtual Cliente id_clienteNavigation { get; set; } = null!;

    public virtual Usuario id_usuarioNavigation { get; set; } = null!;
}
