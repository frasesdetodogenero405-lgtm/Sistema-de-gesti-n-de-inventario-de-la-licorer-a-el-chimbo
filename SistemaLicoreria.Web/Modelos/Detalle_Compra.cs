using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Detalle_Compra
{
    public int id_detalle { get; set; }

    public int id_compra { get; set; }

    public int id_producto { get; set; }

    public int cantidad { get; set; }

    public decimal precio_compra { get; set; }

    public virtual Compra id_compraNavigation { get; set; } = null!;

    public virtual Producto id_productoNavigation { get; set; } = null!;
}
