using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[Permiso("Gestionar métodos de pago")]
public class MetodosPagoController : Controller
{
    private readonly LicoreriaContexto _baseDatos;
    public MetodosPagoController(LicoreriaContexto baseDatos) => _baseDatos = baseDatos;

    public async Task<IActionResult> Lista(string? busqueda)
    {
        var consulta = _baseDatos.Metodos_Pagos.AsNoTracking()
            .Include(metodo => metodo.Pagos_Compras)
            .Include(metodo => metodo.Pagos_Venta)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            consulta = consulta.Where(metodo => metodo.nombre_metodo.Contains(termino));
        }
        ViewData["Busqueda"] = busqueda;
        PrepararPagina("Métodos de pago");
        return View(await consulta.OrderBy(metodo => metodo.nombre_metodo).ToListAsync());
    }

    public IActionResult Crear()
    {
        PrepararPagina("Nuevo método de pago");
        return View("Formulario", new Metodos_Pago { nombre_metodo = string.Empty });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Metodos_Pago modelo)
    {
        modelo.nombre_metodo = modelo.nombre_metodo?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_metodo);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Nuevo método de pago");
        _baseDatos.Metodos_Pagos.Add(new Metodos_Pago { nombre_metodo = modelo.nombre_metodo });
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Método de pago registrado.";
        return RedirectToAction(nameof(Lista));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var metodo = await _baseDatos.Metodos_Pagos.AsNoTracking().FirstOrDefaultAsync(item => item.id_metodo == id);
        if (metodo == null) return NotFound();
        PrepararPagina("Editar método de pago");
        return View("Formulario", metodo);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Metodos_Pago modelo)
    {
        if (id != modelo.id_metodo) return BadRequest();
        modelo.nombre_metodo = modelo.nombre_metodo?.Trim() ?? string.Empty;
        await ValidarNombreAsync(modelo.nombre_metodo, id);
        if (!ModelState.IsValid) return MostrarFormulario(modelo, "Editar método de pago");
        var metodo = await _baseDatos.Metodos_Pagos.FirstOrDefaultAsync(item => item.id_metodo == id);
        if (metodo == null) return NotFound();
        metodo.nombre_metodo = modelo.nombre_metodo;
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Método de pago actualizado.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var metodo = await _baseDatos.Metodos_Pagos.FirstOrDefaultAsync(item => item.id_metodo == id);
        if (metodo == null) return NotFound();
        _baseDatos.Metodos_Pagos.Remove(metodo);
        try { await _baseDatos.SaveChangesAsync(); TempData["Mensaje"] = "Método de pago eliminado."; }
        catch (DbUpdateException) { TempData["Error"] = "No se puede eliminar un método de pago usado en compras o ventas."; }
        return RedirectToAction(nameof(Lista));
    }

    private async Task ValidarNombreAsync(string nombre, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) ModelState.AddModelError(nameof(Metodos_Pago.nombre_metodo), "Escribe el nombre del método de pago.");
        else if (nombre.Length > 50) ModelState.AddModelError(nameof(Metodos_Pago.nombre_metodo), "El nombre admite hasta 50 caracteres.");
        else if (await _baseDatos.Metodos_Pagos.AnyAsync(item => (!id.HasValue || item.id_metodo != id.Value) && item.nombre_metodo.ToLower() == nombre.ToLower()))
            ModelState.AddModelError(nameof(Metodos_Pago.nombre_metodo), "Ya existe un método de pago con ese nombre.");
    }

    private IActionResult MostrarFormulario(Metodos_Pago modelo, string titulo)
    {
        PrepararPagina(titulo);
        return View("Formulario", modelo);
    }

    private void PrepararPagina(string titulo) { ViewData["Title"] = titulo; ViewData["Seccion"] = "Métodos de pago"; }
}
