using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.ModelosVista;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

public class ProductosController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public ProductosController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    [Permiso("Gestionar productos", "consultar productos")]
    public async Task<IActionResult> Lista(string? busqueda)
    {
        var consulta = _baseDatos.Productos.AsNoTracking()
            .Include(producto => producto.id_categoriaNavigation)
            .Include(producto => producto.id_marcaNavigation)
            .Include(producto => producto.Inventarios)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            consulta = consulta.Where(producto => producto.nombre_producto.Contains(termino));
        }

        ViewData["Busqueda"] = busqueda;
        ViewData["Title"] = "Productos";
        ViewData["Seccion"] = "Productos";
        return View(await consulta.OrderBy(producto => producto.nombre_producto).ToListAsync());
    }

    [Permiso("Gestionar productos")]
    public async Task<IActionResult> Crear()
    {
        var modelo = new ProductoFormularioViewModel();
        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Nuevo producto";
        ViewData["Seccion"] = "Productos";
        return View("Formulario", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permiso("Gestionar productos")]
    public async Task<IActionResult> Crear(ProductoFormularioViewModel modelo)
    {
        if (modelo.stock_maximo < modelo.stock_minimo)
            ModelState.AddModelError(nameof(modelo.stock_maximo), "El stock máximo debe ser igual o mayor al mínimo.");
        await ValidarCatalogosAsync(modelo);

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(modelo);
            ViewData["Title"] = "Nuevo producto";
            ViewData["Seccion"] = "Productos";
            return View("Formulario", modelo);
        }

        var producto = new Producto
        {
            nombre_producto = modelo.nombre_producto.Trim(),
            precio_venta = modelo.precio_venta,
            id_categoria = modelo.id_categoria,
            id_marca = modelo.id_marca
        };
        var inventario = new Inventario
        {
            stock_minimo = modelo.stock_minimo,
            stock_maximo = modelo.stock_maximo,
            stock_actual = modelo.stock_actual,
            id_productoNavigation = producto
        };
        producto.Inventarios.Add(inventario);
        _baseDatos.Productos.Add(producto);
        await _baseDatos.SaveChangesAsync();

        TempData["Mensaje"] = "Producto creado y agregado al inventario.";
        return RedirectToAction(nameof(Lista));
    }

    [Permiso("Gestionar productos")]
    public async Task<IActionResult> Editar(int id)
    {
        var producto = await _baseDatos.Productos.AsNoTracking()
            .Include(elemento => elemento.Inventarios)
            .FirstOrDefaultAsync(elemento => elemento.id_producto == id);
        if (producto == null) return NotFound();

        var inventario = producto.Inventarios.FirstOrDefault();
        var modelo = new ProductoFormularioViewModel
        {
            id_producto = producto.id_producto,
            nombre_producto = producto.nombre_producto,
            precio_venta = producto.precio_venta,
            id_categoria = producto.id_categoria,
            id_marca = producto.id_marca,
            stock_minimo = inventario?.stock_minimo ?? 0,
            stock_maximo = inventario?.stock_maximo ?? 0,
            stock_actual = inventario?.stock_actual ?? 0
        };
        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Editar producto";
        ViewData["Seccion"] = "Productos";
        return View("Formulario", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permiso("Gestionar productos")]
    public async Task<IActionResult> Editar(int id, ProductoFormularioViewModel modelo)
    {
        if (id != modelo.id_producto) return BadRequest();
        if (modelo.stock_maximo < modelo.stock_minimo)
            ModelState.AddModelError(nameof(modelo.stock_maximo), "El stock máximo debe ser igual o mayor al mínimo.");
        await ValidarCatalogosAsync(modelo);

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(modelo);
            ViewData["Title"] = "Editar producto";
            ViewData["Seccion"] = "Productos";
            return View("Formulario", modelo);
        }

        var producto = await _baseDatos.Productos
            .Include(elemento => elemento.Inventarios)
            .FirstOrDefaultAsync(elemento => elemento.id_producto == id);
        if (producto == null) return NotFound();

        producto.nombre_producto = modelo.nombre_producto.Trim();
        producto.precio_venta = modelo.precio_venta;
        producto.id_categoria = modelo.id_categoria;
        producto.id_marca = modelo.id_marca;

        var inventario = producto.Inventarios.FirstOrDefault();
        if (inventario == null)
        {
            inventario = new Inventario { id_productoNavigation = producto, stock_actual = modelo.stock_actual };
            producto.Inventarios.Add(inventario);
        }
        inventario.stock_minimo = modelo.stock_minimo;
        inventario.stock_maximo = modelo.stock_maximo;
        await _baseDatos.SaveChangesAsync();

        TempData["Mensaje"] = "Producto actualizado.";
        return RedirectToAction(nameof(Lista));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Permiso("Gestionar productos")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var producto = await _baseDatos.Productos
            .Include(elemento => elemento.Inventarios)
            .FirstOrDefaultAsync(elemento => elemento.id_producto == id);
        if (producto == null) return NotFound();

        _baseDatos.Inventarios.RemoveRange(producto.Inventarios);
        _baseDatos.Productos.Remove(producto);
        try
        {
            await _baseDatos.SaveChangesAsync();
            TempData["Mensaje"] = "Producto eliminado.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "No se puede eliminar este producto porque ya tiene ventas, compras o movimientos asociados.";
        }
        return RedirectToAction(nameof(Lista));
    }

    private async Task CargarOpcionesAsync(ProductoFormularioViewModel modelo)
    {
        modelo.Categorias = await _baseDatos.Categorias_Productos.AsNoTracking()
            .OrderBy(categoria => categoria.nombre_categoria)
            .Select(categoria => new SelectListItem(categoria.nombre_categoria, categoria.id_categoria.ToString()))
            .ToListAsync();
        modelo.Marcas = await _baseDatos.Marcas.AsNoTracking()
            .OrderBy(marca => marca.nombre_marca)
            .Select(marca => new SelectListItem(marca.nombre_marca, marca.id_marca.ToString()))
            .ToListAsync();
    }

    private async Task ValidarCatalogosAsync(ProductoFormularioViewModel modelo)
    {
        if (modelo.id_categoria > 0 &&
            !await _baseDatos.Categorias_Productos.AnyAsync(categoria => categoria.id_categoria == modelo.id_categoria))
            ModelState.AddModelError(nameof(modelo.id_categoria), "La categoría seleccionada ya no existe.");

        if (modelo.id_marca > 0 &&
            !await _baseDatos.Marcas.AnyAsync(marca => marca.id_marca == modelo.id_marca))
            ModelState.AddModelError(nameof(modelo.id_marca), "La marca seleccionada ya no existe.");
    }
}
