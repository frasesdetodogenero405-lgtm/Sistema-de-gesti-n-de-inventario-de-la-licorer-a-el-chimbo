using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Inventario
{
    public int id_inventario { get; set; }

    public int id_producto { get; set; }

    public int stock_minimo { get; set; }

    public int stock_maximo { get; set; }

    public int stock_actual { get; set; }

    public virtual Producto id_productoNavigation { get; set; } = null!;
}
