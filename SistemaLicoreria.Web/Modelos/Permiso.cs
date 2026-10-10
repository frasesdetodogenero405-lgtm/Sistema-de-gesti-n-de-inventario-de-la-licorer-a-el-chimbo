using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Permiso
{
    public int id_permiso { get; set; }

    public int id_rol { get; set; }

    public string accion { get; set; } = null!;

    public virtual Roles_Usuario id_rolNavigation { get; set; } = null!;
}
