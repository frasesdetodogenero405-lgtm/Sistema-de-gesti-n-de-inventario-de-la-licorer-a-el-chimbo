using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Cliente
{
    public int id_cliente { get; set; }

    public string nombre_cliente { get; set; } = null!;

    public string? telefono { get; set; }

    public string? direccion { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
