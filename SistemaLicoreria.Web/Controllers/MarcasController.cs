using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[Permiso("Gestionar marcas")]
public class MarcasController : Controller
{
    private readonly LicoreriaContexto _baseDatos;
    public MarcasController(LicoreriaContexto baseDatos) => _baseDatos = baseDatos;

    public async Task<IActionResult> Lista(string? busqueda)
    {
        var consulta = _baseDatos.Marcas.AsNoTracking().Include(marca => marca.Productos).AsQueryable();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            consulta = consulta.Where(marca => marca.nombre_marca.Contains(termino));
        }
        ViewData["Busqueda"] = busqueda;
        PrepararPagina("Marcas");
        return View(await consulta.OrderBy(marca => marca.nombre_marca).ToListAsync());
    }

    public IActionResult Crear()
    {
        PrepararPagina("Nueva marca");
        return View("Formulario", new Marca { nombre_marca = string.Empty });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Marca modelo)
    {
        modelo.nombre_marca = modelo.nombre_marca?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_marca);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Nueva marca");
        _baseDatos.Marcas.Add(new Marca { nombre_marca = modelo.nombre_marca });
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Marca registrada.";
        return RedirectToAction(nameof(Lista));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var marca = await _baseDatos.Marcas.AsNoTracking().FirstOrDefaultAsync(item => item.id_marca == id);
        if (marca == null) return NotFound();
        PrepararPagina("Editar marca");
        return View("Formulario", marca);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Marca modelo)
    {
        if (id != modelo.id_marca) return BadRequest();
        modelo.nombre_marca = modelo.nombre_marca?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_marca, id);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Editar marca");
        var marca = await _baseDatos.Marcas.FirstOrDefaultAsync(item => item.id_marca == id);
        if (marca == null) return NotFound();
        marca.nombre_marca = modelo.nombre_marca;
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Marca actualizada.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var marca = await _baseDatos.Marcas.FirstOrDefaultAsync(item => item.id_marca == id);
        if (marca == null) return NotFound();
        _baseDatos.Marcas.Remove(marca);
        try { await _baseDatos.SaveChangesAsync(); TempData["Mensaje"] = "Marca eliminada."; }
        catch (DbUpdateException) { TempData["Error"] = "No se puede eliminar una marca asignada a productos."; }
        return RedirectToAction(nameof(Lista));
    }

    private async Task ValidarNombreAsync(string nombre, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) ModelState.AddModelError(nameof(Marca.nombre_marca), "Escribe el nombre de la marca.");
        else if (nombre.Length > 50) ModelState.AddModelError(nameof(Marca.nombre_marca), "El nombre admite hasta 50 caracteres.");
        else if (await _baseDatos.Marcas.AnyAsync(item => (!id.HasValue || item.id_marca != id.Value) && item.nombre_marca.ToLower() == nombre.ToLower()))
            ModelState.AddModelError(nameof(Marca.nombre_marca), "Ya existe una marca con ese nombre.");
    }

    private IActionResult MostrarFormulario(Marca modelo, string titulo)
    {
        PrepararPagina(titulo);
        return View("Formulario", modelo);
    }

    private void PrepararPagina(string titulo) { ViewData["Title"] = titulo; ViewData["Seccion"] = "Marcas"; }
}
