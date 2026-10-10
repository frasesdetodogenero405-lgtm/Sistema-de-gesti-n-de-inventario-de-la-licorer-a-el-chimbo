using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Usuario
{
    public int id_usuario { get; set; }

    public string nombre_usuario { get; set; } = null!;

    public string contrasena { get; set; } = null!;

    public int id_rol { get; set; }

    public int? id_licoreria { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual Licoreria? id_licoreriaNavigation { get; set; }

    public virtual Roles_Usuario id_rolNavigation { get; set; } = null!;
}
