using System.ComponentModel.DataAnnotations;

namespace SistemaLicoreria.Web.ModelosVista;

public class ClienteFormularioViewModel
{
    public int id_cliente { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del cliente.")]
    [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
    [Display(Name = "Nombre del cliente")]
    public string nombre_cliente { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El teléfono admite hasta 20 caracteres.")]
    [Display(Name = "Teléfono")]
    public string? telefono { get; set; }

    [StringLength(200, ErrorMessage = "La dirección admite hasta 200 caracteres.")]
    [Display(Name = "Dirección")]
    public string? direccion { get; set; }
}
