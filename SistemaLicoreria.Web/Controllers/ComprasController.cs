using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.ModelosVista;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[Permiso("Gestionar compras")]
public class ComprasController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public ComprasController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Lista()
    {
        ViewData["Title"] = "Compras";
        ViewData["Seccion"] = "Compras";
        var compras = await _baseDatos.Compras.AsNoTracking()
            .Include(compra => compra.id_proveedorNavigation)
            .Include(compra => compra.id_usuarioNavigation)
            .OrderByDescending(compra => compra.fecha)
            .Take(100)
            .ToListAsync();
        return View(compras);
    }

    public async Task<IActionResult> Crear()
    {
        var modelo = new CompraFormularioViewModel();
        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Nueva compra";
        ViewData["Seccion"] = "Compras";
        return View("Formulario", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CompraFormularioViewModel modelo)
    {
        modelo.Detalles ??= new List<DetalleCompraFormularioViewModel>();
        if (modelo.Detalles.Count == 0)
            ModelState.AddModelError(nameof(modelo.Detalles), "Agrega al menos un producto a la compra.");
        else
        {
            foreach (var detalle in modelo.Detalles)
            {
                if (detalle.id_producto <= 0)
                    ModelState.AddModelError(nameof(modelo.Detalles), "Selecciona un producto en cada línea.");
            }
            if (modelo.Detalles.Select(detalle => detalle.id_producto).Distinct().Count() != modelo.Detalles.Count)
                ModelState.AddModelError(nameof(modelo.Detalles), "No repitas productos en la misma compra.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(modelo);
            ViewData["Title"] = "Nueva compra";
            ViewData["Seccion"] = "Compras";
            return View("Formulario", modelo);
        }

        var idUsuario = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioActual)
            ? usuarioActual : 0;
        if (idUsuario <= 0)
        {
            ModelState.AddModelError(string.Empty, "La sesión no contiene un usuario válido. Vuelve a iniciar sesión.");
            await CargarOpcionesAsync(modelo);
            return View("Formulario", modelo);
        }

        try
        {
            await using var transaccion = await _baseDatos.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var proveedorExiste = await _baseDatos.Proveedores.AnyAsync(proveedor => proveedor.id_proveedor == modelo.id_proveedor);
            if (!proveedorExiste)
                throw new InvalidOperationException("El proveedor seleccionado ya no existe.");

            var idsProducto = modelo.Detalles.Select(detalle => detalle.id_producto).ToArray();
            var inventarios = await _baseDatos.Inventarios
                .Include(inventario => inventario.id_productoNavigation)
                .Where(inventario => idsProducto.Contains(inventario.id_producto))
                .ToListAsync();
            if (inventarios.Count != idsProducto.Length)
                throw new InvalidOperationException("Uno o más productos no tienen un registro de inventario.");

            decimal total = 0m;
            foreach (var detalle in modelo.Detalles)
            {
                if (detalle.cantidad <= 0 || detalle.precio_compra <= 0m || detalle.precio_venta <= 0m)
                    throw new InvalidOperationException("Revisa la cantidad y los precios de cada producto.");
                total += detalle.cantidad * detalle.precio_compra;
            }
            if (total <= 0m || total > 99999999.99m)
                throw new InvalidOperationException("El total de la compra excede el límite permitido.");

            var compra = new Compra
            {
                id_proveedor = modelo.id_proveedor,
                id_usuario = idUsuario,
                fecha = DateTime.Now,
                total = total
            };
            _baseDatos.Compras.Add(compra);
            await _baseDatos.SaveChangesAsync();

            foreach (var detalle in modelo.Detalles)
            {
                var inventario = inventarios.Single(elemento => elemento.id_producto == detalle.id_producto);
                var producto = inventario.id_productoNavigation;
                inventario.stock_actual += detalle.cantidad;
                producto.precio_venta = detalle.precio_venta;
                compra.Detalle_Compras.Add(new Detalle_Compra
                {
                    id_compra = compra.id_compra,
                    id_producto = producto.id_producto,
                    cantidad = detalle.cantidad,
                    precio_compra = detalle.precio_compra,
                    id_compraNavigation = compra,
                    id_productoNavigation = producto
                });
                _baseDatos.Movimientos_Stocks.Add(new Movimientos_Stock
                {
                    id_producto = producto.id_producto,
                    tipo_movimiento = "entrada",
                    cantidad = detalle.cantidad,
                    fecha = compra.fecha,
                    referencia = "Compra #" + compra.id_compra,
                    id_productoNavigation = producto
                });
            }
            await _baseDatos.SaveChangesAsync();
            await transaccion.CommitAsync();

            TempData["Mensaje"] = "Compra " + compra.id_compra + " registrada. Total: Bs " + total.ToString("N2");
            return RedirectToAction(nameof(Lista));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex is InvalidOperationException
                ? ex.Message
                : "No se pudo guardar la compra. Revisa la conexión y los datos de la base de datos.");
        }

        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Nueva compra";
        ViewData["Seccion"] = "Compras";
        return View("Formulario", modelo);
    }

    private async Task CargarOpcionesAsync(CompraFormularioViewModel modelo)
    {
        modelo.Proveedores = await _baseDatos.Proveedores.AsNoTracking().OrderBy(proveedor => proveedor.nombre_proveedor)
            .Select(proveedor => new SelectListItem(proveedor.nombre_proveedor, proveedor.id_proveedor.ToString())).ToListAsync();
        modelo.Productos = await _baseDatos.Productos.AsNoTracking().OrderBy(producto => producto.nombre_producto)
            .Select(producto => new OpcionProductoViewModel
            {
                id_producto = producto.id_producto,
                nombre_producto = producto.nombre_producto,
                precio_venta = producto.precio_venta
            }).ToListAsync();
        if (modelo.Detalles.Count == 0)
            modelo.Detalles = new List<DetalleCompraFormularioViewModel> { new() };
    }
}
