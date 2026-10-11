using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaLicoreria.Web.ModelosVista;

public class UsuarioFormularioViewModel
{
    [Required(ErrorMessage = "Escribe el nombre de usuario.")]
    [StringLength(100, ErrorMessage = "El usuario admite hasta 100 caracteres.")]
    [Display(Name = "Nombre de usuario")]
    public string nombre_usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe una contraseña.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la contraseña.")]
    [Compare(nameof(contrasena), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    public string confirmar_contrasena { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un rol para el usuario.")]
    [Display(Name = "Rol")]
    public int id_rol { get; set; }

    public List<SelectListItem> roles { get; set; } = new();
}
