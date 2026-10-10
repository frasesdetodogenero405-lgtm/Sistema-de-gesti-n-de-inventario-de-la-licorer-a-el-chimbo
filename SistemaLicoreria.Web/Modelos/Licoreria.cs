using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Licoreria
{
    public int Id_Licoreria { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
