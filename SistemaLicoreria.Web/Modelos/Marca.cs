using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Marca
{
    public int id_marca { get; set; }

    public string nombre_marca { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
