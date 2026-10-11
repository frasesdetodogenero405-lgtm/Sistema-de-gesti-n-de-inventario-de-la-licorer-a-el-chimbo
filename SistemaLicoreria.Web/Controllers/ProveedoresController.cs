using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.ModelosVista;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[Permiso("Gestionar proveedores")]
public class ProveedoresController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public ProveedoresController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Lista(string? busqueda)
    {
        var proveedores = _baseDatos.Proveedores.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            proveedores = proveedores.Where(proveedor => proveedor.nombre_proveedor.Contains(termino));
        }
        ViewData["Title"] = "Proveedores";
        ViewData["Seccion"] = "Proveedores";
        ViewData["Busqueda"] = busqueda;
        return View(await proveedores.OrderBy(proveedor => proveedor.nombre_proveedor).ToListAsync());
    }

    public IActionResult Crear()
    {
        ViewData["Title"] = "Nuevo proveedor";
        ViewData["Seccion"] = "Proveedores";
        return View("Formulario", new ProveedorFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProveedorFormularioViewModel modelo)
    {
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Nuevo proveedor");
        _baseDatos.Proveedores.Add(new Proveedor
        {
            nombre_proveedor = modelo.nombre_proveedor.Trim(),
            telefono = Limpiar(modelo.telefono),
            direccion = Limpiar(modelo.direccion)
        });
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Proveedor registrado.";
        return RedirectToAction(nameof(Lista));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var proveedor = await _baseDatos.Proveedores.AsNoTracking()
            .FirstOrDefaultAsync(elemento => elemento.id_proveedor == id);
        if (proveedor == null) return NotFound();
        ViewData["Title"] = "Editar proveedor";
        ViewData["Seccion"] = "Proveedores";
        return View("Formulario", new ProveedorFormularioViewModel
        {
            id_proveedor = proveedor.id_proveedor,
            nombre_proveedor = proveedor.nombre_proveedor,
            telefono = proveedor.telefono,
            direccion = proveedor.direccion
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ProveedorFormularioViewModel modelo)
    {
        if (id != modelo.id_proveedor) return BadRequest();
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Editar proveedor");
        var proveedor = await _baseDatos.Proveedores.FirstOrDefaultAsync(elemento => elemento.id_proveedor == id);
        if (proveedor == null) return NotFound();
        proveedor.nombre_proveedor = modelo.nombre_proveedor.Trim();
        proveedor.telefono = Limpiar(modelo.telefono);
        proveedor.direccion = Limpiar(modelo.direccion);
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Proveedor actualizado.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var proveedor = await _baseDatos.Proveedores.FirstOrDefaultAsync(elemento => elemento.id_proveedor == id);
        if (proveedor == null) return NotFound();
        _baseDatos.Proveedores.Remove(proveedor);
        try
        {
            await _baseDatos.SaveChangesAsync();
            TempData["Mensaje"] = "Proveedor eliminado.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se puede eliminar un proveedor que ya tiene compras registradas.";
        }
        return RedirectToAction(nameof(Lista));
    }

    private IActionResult MostrarFormulario(ProveedorFormularioViewModel modelo, string titulo)
    {
        ViewData["Title"] = titulo;
        ViewData["Seccion"] = "Proveedores";
        return View("Formulario", modelo);
    }

    private static string? Limpiar(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
