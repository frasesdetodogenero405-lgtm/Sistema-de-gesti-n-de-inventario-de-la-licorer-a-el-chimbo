using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;

namespace SistemaLicoreria.Web.Controllers;

public class InventarioController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public InventarioController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Lista()
    {
        ViewData["Title"] = "Inventario";
        ViewData["Seccion"] = "Inventario";
        var inventario = await _baseDatos.Inventarios.AsNoTracking()
            .Include(elemento => elemento.id_productoNavigation)
            .OrderBy(elemento => elemento.id_productoNavigation.nombre_producto)
            .ToListAsync();
        return View(inventario);
    }
}
