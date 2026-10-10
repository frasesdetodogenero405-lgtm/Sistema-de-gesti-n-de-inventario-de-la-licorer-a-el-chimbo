using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Categorias_Producto
{
    public int id_categoria { get; set; }

    public string nombre_categoria { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
