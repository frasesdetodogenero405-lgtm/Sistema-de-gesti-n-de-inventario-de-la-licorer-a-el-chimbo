using System.ComponentModel.DataAnnotations;

namespace SistemaLicoreria.Web.ModelosVista;

public class RolFormularioViewModel
{
    public int? id_rol { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del rol.")]
    [StringLength(50, ErrorMessage = "El nombre del rol admite hasta 50 caracteres.")]
    [Display(Name = "Nombre del rol")]
    public string nombre_rol { get; set; } = string.Empty;

    public List<string> permisos_seleccionados { get; set; } = new();

    public IReadOnlyList<PermisoDisponibleViewModel> permisos_disponibles { get; set; } = Array.Empty<PermisoDisponibleViewModel>();
}

public class PermisoDisponibleViewModel
{
    public string accion { get; set; } = string.Empty;
    public string modulo { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
}

public class RolListaViewModel
{
    public int id_rol { get; set; }
    public string nombre_rol { get; set; } = string.Empty;
    public int cantidad_usuarios { get; set; }
    public int cantidad_permisos { get; set; }
}

public class UsuariosRolesViewModel
{
    public int id_usuario_sesion { get; set; }
    public IReadOnlyList<UsuarioRolViewModel> usuarios { get; set; } = Array.Empty<UsuarioRolViewModel>();
    public IReadOnlyList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> roles { get; set; } = Array.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
}

public class UsuarioRolViewModel
{
    public int id_usuario { get; set; }
    public string nombre_usuario { get; set; } = string.Empty;
    public int id_rol { get; set; }
    public string nombre_rol { get; set; } = string.Empty;
}
