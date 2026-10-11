using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[Permiso("Gestionar categorÃ­as")]
public class CategoriasController : Controller
{
    private readonly LicoreriaContexto _baseDatos;
    public CategoriasController(LicoreriaContexto baseDatos) => _baseDatos = baseDatos;

    public async Task<IActionResult> Lista(string? busqueda)
    {
        var consulta = _baseDatos.Categorias_Productos.AsNoTracking().Include(categoria => categoria.Productos).AsQueryable();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            consulta = consulta.Where(categoria => categoria.nombre_categoria.Contains(termino));
        }
        ViewData["Busqueda"] = busqueda;
        PrepararPagina("CategorÃ­as");
        return View(await consulta.OrderBy(categoria => categoria.nombre_categoria).ToListAsync());
    }

    public IActionResult Crear()
    {
        PrepararPagina("Nueva categorÃ­a");
        return View("Formulario", new Categorias_Producto { nombre_categoria = string.Empty });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Categorias_Producto modelo)
    {
        modelo.nombre_categoria = modelo.nombre_categoria?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_categoria);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Nueva categorÃ­a");
        _baseDatos.Categorias_Productos.Add(new Categorias_Producto { nombre_categoria = modelo.nombre_categoria });
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "CategorÃ­a registrada.";
        return RedirectToAction(nameof(Lista));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var categoria = await _baseDatos.Categorias_Productos.AsNoTracking().FirstOrDefaultAsync(item => item.id_categoria == id);
        if (categoria == null) return NotFound();
        PrepararPagina("Editar categorÃ­a");
        return View("Formulario", categoria);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Categorias_Producto modelo)
    {
        if (id != modelo.id_categoria) return BadRequest();
        modelo.nombre_categoria = modelo.nombre_categoria?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_categoria, id);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Editar categorÃ­a");
        var categoria = await _baseDatos.Categorias_Productos.FirstOrDefaultAsync(item => item.id_categoria == id);
        if (categoria == null) return NotFound();
        categoria.nombre_categoria = modelo.nombre_categoria;
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "CategorÃ­a actualizada.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var categoria = await _baseDatos.Categorias_Productos.FirstOrDefaultAsync(item => item.id_categoria == id);
        if (categoria == null) return NotFound();
        _baseDatos.Categorias_Productos.Remove(categoria);
        try { await _baseDatos.SaveChangesAsync(); TempData["Mensaje"] = "CategorÃ­a eliminada."; }
        catch (DbUpdateException) { TempData["Error"] = "No se puede eliminar una categorÃ­a asignada a productos."; }
        return RedirectToAction(nameof(Lista));
    }

    private async Task ValidarNombreAsync(string nombre, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) ModelState.AddModelError(nameof(Categorias_Producto.nombre_categoria), "Escribe el nombre de la categorÃ­a.");
        else if (nombre.Length > 50) ModelState.AddModelError(nameof(Categorias_Producto.nombre_categoria), "El nombre admite hasta 50 caracteres.");
        else if (await _baseDatos.Categorias_Productos.AnyAsync(item => (!id.HasValue || item.id_categoria != id.Value) && item.nombre_categoria.ToLower() == nombre.ToLower()))
            ModelState.AddModelError(nameof(Categorias_Producto.nombre_categoria), "Ya existe una categorÃ­a con ese nombre.");
    }

    private IActionResult MostrarFormulario(Categorias_Producto modelo, string titulo)
    {
        PrepararPagina(titulo);
        return View("Formulario", modelo);
    }

    private void PrepararPagina(string titulo) { ViewData["Title"] = titulo; ViewData["Seccion"] = "CategorÃ­as"; }
}
