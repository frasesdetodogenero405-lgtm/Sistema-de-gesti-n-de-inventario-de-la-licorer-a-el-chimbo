using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaLicoreria.Web.ModelosVista;

public class CompraFormularioViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un proveedor.")]
    [Display(Name = "Proveedor")]
    public int id_proveedor { get; set; }

    public List<DetalleCompraFormularioViewModel> Detalles { get; set; } = new() { new() };
    public List<SelectListItem> Proveedores { get; set; } = new();
    public List<OpcionProductoViewModel> Productos { get; set; } = new();
}

public class DetalleCompraFormularioViewModel
{
    public int id_producto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int cantidad { get; set; }

    [Range(typeof(decimal), "0.01", "99999999.99", ErrorMessage = "El costo debe ser mayor que cero.")]
    [Display(Name = "Precio de compra")]
    public decimal precio_compra { get; set; }

    [Range(typeof(decimal), "0.01", "99999999.99", ErrorMessage = "El precio de venta debe ser mayor que cero.")]
    [Display(Name = "Precio de venta")]
    public decimal precio_venta { get; set; }
}
