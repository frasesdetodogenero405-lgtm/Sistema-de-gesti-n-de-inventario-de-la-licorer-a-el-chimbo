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

[Permiso("Gestionar ventas", "Registrar ventas")]
public class VentasController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public VentasController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Lista()
    {
        ViewData["Title"] = "Ventas";
        ViewData["Seccion"] = "Ventas";
        var ventas = await _baseDatos.Ventas.AsNoTracking()
            .Include(venta => venta.id_clienteNavigation)
            .Include(venta => venta.id_usuarioNavigation)
            .OrderByDescending(venta => venta.fecha)
            .Take(100)
            .ToListAsync();
        return View(ventas);
    }

    public async Task<IActionResult> Crear()
    {
        var modelo = new VentaFormularioViewModel();
        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Nueva venta";
        ViewData["Seccion"] = "Ventas";
        return View("Formulario", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(VentaFormularioViewModel modelo)
    {
        modelo.Detalles ??= new List<DetalleVentaFormularioViewModel>();
        if (modelo.Detalles.Count == 0)
            ModelState.AddModelError(nameof(modelo.Detalles), "Agrega al menos un producto a la venta.");
        else
        {
            foreach (var detalle in modelo.Detalles)
            {
                if (detalle.id_producto <= 0)
                    ModelState.AddModelError(nameof(modelo.Detalles), "Selecciona un producto en cada línea.");
            }
            if (modelo.Detalles.Select(detalle => detalle.id_producto).Distinct().Count() != modelo.Detalles.Count)
                ModelState.AddModelError(nameof(modelo.Detalles), "No repitas productos en la misma venta.");
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(modelo);
            ViewData["Title"] = "Nueva venta";
            ViewData["Seccion"] = "Ventas";
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
            var clienteExiste = await _baseDatos.Clientes.AnyAsync(cliente => cliente.id_cliente == modelo.id_cliente);
            var metodo = await _baseDatos.Metodos_Pagos.FirstOrDefaultAsync(elemento => elemento.id_metodo == modelo.id_metodo);
            if (!clienteExiste || metodo == null)
                throw new InvalidOperationException("El cliente o método de pago seleccionado ya no existe.");

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
                var inventario = inventarios.Single(elemento => elemento.id_producto == detalle.id_producto);
                if (detalle.cantidad <= 0 || inventario.stock_actual < detalle.cantidad)
                    throw new InvalidOperationException("No hay existencias suficientes de " + inventario.id_productoNavigation.nombre_producto + ".");
                total += inventario.id_productoNavigation.precio_venta * detalle.cantidad;
            }
            if (total <= 0m || total > 99999999.99m)
                throw new InvalidOperationException("El total de la venta excede el límite permitido.");

            var venta = new Venta
            {
                id_cliente = modelo.id_cliente,
                id_usuario = idUsuario,
                fecha = DateTime.Now,
                total = total,
                metodo_pago = metodo.nombre_metodo
            };
            _baseDatos.Ventas.Add(venta);
            await _baseDatos.SaveChangesAsync();

            foreach (var detalle in modelo.Detalles)
            {
                var inventario = inventarios.Single(elemento => elemento.id_producto == detalle.id_producto);
                var producto = inventario.id_productoNavigation;
                inventario.stock_actual -= detalle.cantidad;
                venta.Detalle_Venta.Add(new Detalle_Venta
                {
                    id_venta = venta.id_venta,
                    id_producto = producto.id_producto,
                    cantidad = detalle.cantidad,
                    precio_venta = producto.precio_venta,
                    id_productoNavigation = producto,
                    id_ventaNavigation = venta
                });
                _baseDatos.Movimientos_Stocks.Add(new Movimientos_Stock
                {
                    id_producto = producto.id_producto,
                    tipo_movimiento = "salida",
                    cantidad = detalle.cantidad,
                    fecha = venta.fecha,
                    referencia = "Venta #" + venta.id_venta,
                    id_productoNavigation = producto
                });
            }
            _baseDatos.Pagos_Ventas.Add(new Pagos_Venta
            {
                id_venta = venta.id_venta,
                id_metodo = metodo.id_metodo,
                monto = total,
                fecha_pago = venta.fecha,
                id_ventaNavigation = venta,
                id_metodoNavigation = metodo
            });
            await _baseDatos.SaveChangesAsync();
            await transaccion.CommitAsync();

            TempData["Mensaje"] = "Venta " + venta.id_venta + " registrada. Total: Bs " + total.ToString("N2");
            return RedirectToAction(nameof(Lista));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex is InvalidOperationException
                ? ex.Message
                : "No se pudo guardar la venta. Revisa la conexión y los datos de la base de datos.");
        }

        await CargarOpcionesAsync(modelo);
        ViewData["Title"] = "Nueva venta";
        ViewData["Seccion"] = "Ventas";
        return View("Formulario", modelo);
    }

    private async Task CargarOpcionesAsync(VentaFormularioViewModel modelo)
    {
        modelo.Clientes = await _baseDatos.Clientes.AsNoTracking().OrderBy(cliente => cliente.nombre_cliente)
            .Select(cliente => new SelectListItem(cliente.nombre_cliente, cliente.id_cliente.ToString())).ToListAsync();
        modelo.MetodosPago = await _baseDatos.Metodos_Pagos.AsNoTracking().OrderBy(metodo => metodo.nombre_metodo)
            .Select(metodo => new SelectListItem(metodo.nombre_metodo, metodo.id_metodo.ToString())).ToListAsync();
        modelo.Productos = await _baseDatos.Productos.AsNoTracking().OrderBy(producto => producto.nombre_producto)
            .Select(producto => new OpcionProductoViewModel
            {
                id_producto = producto.id_producto,
                nombre_producto = producto.nombre_producto,
                precio_venta = producto.precio_venta
            }).ToListAsync();
        if (modelo.Detalles.Count == 0)
            modelo.Detalles = new List<DetalleVentaFormularioViewModel> { new() };
    }
}
