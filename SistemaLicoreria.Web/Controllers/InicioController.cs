using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.ModelosVista;

namespace SistemaLicoreria.Web.Controllers;

public class InicioController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public InicioController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task<IActionResult> Tablero()
    {
        var tablero = new TableroViewModel();
        var hoy = DateTime.Today;
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
        var inicioGrafica = inicioMes.AddMonths(-5);

        try
        {
            tablero.CantidadProductos = await _baseDatos.Productos.CountAsync();
            tablero.CantidadClientes = await _baseDatos.Clientes.CountAsync();
            tablero.ProductosConStockBajo = await _baseDatos.Inventarios
                .CountAsync(inventario => inventario.stock_actual <= inventario.stock_minimo);

            tablero.VentasDelMes = await _baseDatos.Ventas
                .Where(venta => venta.fecha >= inicioMes)
                .SumAsync(venta => (decimal?)venta.total) ?? 0m;
            tablero.ComprasDelMes = await _baseDatos.Compras
                .Where(compra => compra.fecha >= inicioMes)
                .SumAsync(compra => (decimal?)compra.total) ?? 0m;

            var ventas = await _baseDatos.Ventas.AsNoTracking()
                .Where(venta => venta.fecha >= inicioGrafica && venta.fecha != null)
                .Select(venta => new { Fecha = venta.fecha!.Value, venta.total })
                .ToListAsync();
            var compras = await _baseDatos.Compras.AsNoTracking()
                .Where(compra => compra.fecha >= inicioGrafica && compra.fecha != null)
                .Select(compra => new { Fecha = compra.fecha!.Value, compra.total })
                .ToListAsync();

            var cultura = CultureInfo.GetCultureInfo("es-BO");
            var meses = Enumerable.Range(0, 6)
                .Select(indice => inicioGrafica.AddMonths(indice))
                .ToArray();

            tablero.Meses = meses.Select(mes => cultura.TextInfo
                .ToTitleCase(mes.ToString("MMM", cultura))).ToArray();
            tablero.VentasMensuales = meses.Select(mes => ventas
                .Where(venta => venta.Fecha.Year == mes.Year && venta.Fecha.Month == mes.Month)
                .Sum(venta => venta.total)).ToArray();
            tablero.ComprasMensuales = meses.Select(mes => compras
                .Where(compra => compra.Fecha.Year == mes.Year && compra.Fecha.Month == mes.Month)
                .Sum(compra => compra.total)).ToArray();

            tablero.VentasRecientes = await _baseDatos.Ventas.AsNoTracking()
                .Include(venta => venta.id_clienteNavigation)
                .OrderByDescending(venta => venta.fecha)
                .Take(8)
                .Select(venta => new VentaRecienteViewModel
                {
                    Numero = venta.id_venta,
                    Cliente = venta.id_clienteNavigation.nombre_cliente,
                    Fecha = venta.fecha ?? DateTime.MinValue,
                    Total = venta.total,
                    MetodoPago = venta.metodo_pago
                })
                .ToListAsync();
        }
        catch (Exception)
        {
            tablero.ErrorConexion = "No se pudo consultar la base de datos. Revisa SQL Server y la cadena de conexión en appsettings.json.";
        }

        return View(tablero);
    }

    public IActionResult Error()
    {
        return View();
    }
}
