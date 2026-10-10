using System.ComponentModel.DataAnnotations;

namespace SistemaLicoreria.Web.ModelosVista;

public class ProveedorFormularioViewModel
{
    public int id_proveedor { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del proveedor.")]
    [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
    [Display(Name = "Nombre del proveedor")]
    public string nombre_proveedor { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El teléfono admite hasta 20 caracteres.")]
    [Display(Name = "Teléfono")]
    public string? telefono { get; set; }

    [StringLength(200, ErrorMessage = "La dirección admite hasta 200 caracteres.")]
    [Display(Name = "Dirección")]
    public string? direccion { get; set; }
}
