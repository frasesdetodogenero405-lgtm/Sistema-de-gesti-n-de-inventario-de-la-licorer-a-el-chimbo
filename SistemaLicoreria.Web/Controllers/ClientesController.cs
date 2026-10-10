using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.ModelosVista;

namespace SistemaLicoreria.Web.Controllers;

public class ClientesController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public ClientesController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Lista(string? busqueda)
    {
        var clientes = _baseDatos.Clientes.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            clientes = clientes.Where(cliente => cliente.nombre_cliente.Contains(termino));
        }
        ViewData["Title"] = "Clientes";
        ViewData["Seccion"] = "Clientes";
        ViewData["Busqueda"] = busqueda;
        return View(await clientes.OrderBy(cliente => cliente.nombre_cliente).ToListAsync());
    }

    public IActionResult Crear()
    {
        ViewData["Title"] = "Nuevo cliente";
        ViewData["Seccion"] = "Clientes";
        return View("Formulario", new ClienteFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ClienteFormularioViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Nuevo cliente";
            ViewData["Seccion"] = "Clientes";
            return View("Formulario", modelo);
        }
        _baseDatos.Clientes.Add(new Cliente
        {
            nombre_cliente = modelo.nombre_cliente.Trim(),
            telefono = Limpiar(modelo.telefono),
            direccion = Limpiar(modelo.direccion)
        });
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Cliente registrado.";
        return RedirectToAction(nameof(Lista));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await _baseDatos.Clientes.AsNoTracking().FirstOrDefaultAsync(item => item.id_cliente == id);
        if (cliente == null) return NotFound();
        ViewData["Title"] = "Editar cliente";
        ViewData["Seccion"] = "Clientes";
        return View("Formulario", new ClienteFormularioViewModel
        {
            id_cliente = cliente.id_cliente,
            nombre_cliente = cliente.nombre_cliente,
            telefono = cliente.telefono,
            direccion = cliente.direccion
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ClienteFormularioViewModel modelo)
    {
        if (id != modelo.id_cliente) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Editar cliente";
            ViewData["Seccion"] = "Clientes";
            return View("Formulario", modelo);
        }
        var cliente = await _baseDatos.Clientes.FirstOrDefaultAsync(item => item.id_cliente == id);
        if (cliente == null) return NotFound();
        cliente.nombre_cliente = modelo.nombre_cliente.Trim();
        cliente.telefono = Limpiar(modelo.telefono);
        cliente.direccion = Limpiar(modelo.direccion);
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Datos del cliente actualizados.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var cliente = await _baseDatos.Clientes.FirstOrDefaultAsync(item => item.id_cliente == id);
        if (cliente == null) return NotFound();
        _baseDatos.Clientes.Remove(cliente);
        try
        {
            await _baseDatos.SaveChangesAsync();
            TempData["Mensaje"] = "Cliente eliminado.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se puede eliminar un cliente que ya tiene ventas registradas.";
        }
        return RedirectToAction(nameof(Lista));
    }

    private static string? Limpiar(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
