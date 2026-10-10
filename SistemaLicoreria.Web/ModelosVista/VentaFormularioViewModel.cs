using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaLicoreria.Web.ModelosVista;

public class VentaFormularioViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    [Display(Name = "Cliente")]
    public int id_cliente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona el método de pago.")]
    [Display(Name = "Método de pago")]
    public int id_metodo { get; set; }

    public List<DetalleVentaFormularioViewModel> Detalles { get; set; } = new() { new() };
    public List<SelectListItem> Clientes { get; set; } = new();
    public List<SelectListItem> MetodosPago { get; set; } = new();
    public List<OpcionProductoViewModel> Productos { get; set; } = new();
}

public class DetalleVentaFormularioViewModel
{
    public int id_producto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int cantidad { get; set; }
}
