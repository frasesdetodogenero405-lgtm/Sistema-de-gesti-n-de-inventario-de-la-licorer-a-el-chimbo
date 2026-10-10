using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Detalle_Venta
{
    public int id_detalle { get; set; }

    public int id_venta { get; set; }

    public int id_producto { get; set; }

    public int cantidad { get; set; }

    public decimal precio_venta { get; set; }

    public virtual Producto id_productoNavigation { get; set; } = null!;

    public virtual Venta id_ventaNavigation { get; set; } = null!;
}
