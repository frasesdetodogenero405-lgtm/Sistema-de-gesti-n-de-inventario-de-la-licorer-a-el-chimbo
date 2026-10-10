using System.ComponentModel.DataAnnotations;

namespace SistemaLicoreria.Web.ModelosVista;

public class AccesoViewModel
{
    [Required(ErrorMessage = "Escribe tu usuario.")]
    [Display(Name = "Usuario")]
    public string nombre_usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string contrasena { get; set; } = string.Empty;
}
