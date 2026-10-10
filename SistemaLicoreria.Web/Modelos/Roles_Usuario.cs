using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Roles_Usuario
{
    public int id_rol { get; set; }

    public string nombre_rol { get; set; } = null!;

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
