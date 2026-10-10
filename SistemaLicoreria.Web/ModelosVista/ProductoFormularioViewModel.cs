using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaLicoreria.Web.ModelosVista;

public class ProductoFormularioViewModel
{
    public int id_producto { get; set; }

    [Required(ErrorMessage = "Escribe el nombre del producto.")]
    [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
    [Display(Name = "Nombre del producto")]
    public string nombre_producto { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "99999999.99", ErrorMessage = "El precio debe ser mayor que cero.")]
    [Display(Name = "Precio de venta")]
    public decimal precio_venta { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría.")]
    [Display(Name = "Categoría")]
    public int id_categoria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una marca.")]
    [Display(Name = "Marca")]
    public int id_marca { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    [Display(Name = "Stock mínimo")]
    public int stock_minimo { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock máximo no puede ser negativo.")]
    [Display(Name = "Stock máximo")]
    public int stock_maximo { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock inicial no puede ser negativo.")]
    [Display(Name = "Stock inicial")]
    public int stock_actual { get; set; }

    public List<SelectListItem> Categorias { get; set; } = new();
    public List<SelectListItem> Marcas { get; set; } = new();
}
