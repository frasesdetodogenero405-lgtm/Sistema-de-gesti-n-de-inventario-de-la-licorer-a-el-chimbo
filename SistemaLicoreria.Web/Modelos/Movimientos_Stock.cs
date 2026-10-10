using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Movimientos_Stock
{
    public int id_movimiento { get; set; }

    public int id_producto { get; set; }

    public string? tipo_movimiento { get; set; }

    public int cantidad { get; set; }

    public DateTime? fecha { get; set; }

    public string? referencia { get; set; }

    public virtual Producto id_productoNavigation { get; set; } = null!;
}
