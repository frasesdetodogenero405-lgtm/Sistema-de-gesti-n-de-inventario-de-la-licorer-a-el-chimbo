using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Compra
{
    public int id_compra { get; set; }

    public int id_proveedor { get; set; }

    public int id_usuario { get; set; }

    public DateTime? fecha { get; set; }

    public decimal total { get; set; }

    public virtual ICollection<Detalle_Compra> Detalle_Compras { get; set; } = new List<Detalle_Compra>();

    public virtual ICollection<Pagos_Compra> Pagos_Compras { get; set; } = new List<Pagos_Compra>();

    public virtual Proveedor id_proveedorNavigation { get; set; } = null!;

    public virtual Usuario id_usuarioNavigation { get; set; } = null!;
}
