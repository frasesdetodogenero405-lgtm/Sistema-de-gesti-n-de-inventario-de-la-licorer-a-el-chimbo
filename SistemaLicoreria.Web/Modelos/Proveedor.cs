using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Proveedor
{
    public int id_proveedor { get; set; }

    public string nombre_proveedor { get; set; } = null!;

    public string? telefono { get; set; }

    public string? direccion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
