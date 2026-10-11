using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.ModelosVista;

namespace SistemaLicoreria.Web.Controllers;

[AllowAnonymous]
public class AccesoController : Controller
{
    private readonly LicoreriaContexto _baseDatos;

    public AccesoController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    [HttpGet]
    public IActionResult IniciarSesion(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Tablero", "Inicio");
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Title"] = "Iniciar sesión";
        return View(new AccesoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IniciarSesion(AccesoViewModel modelo, string? returnUrl = null)
    {
        ViewData["Title"] = "Iniciar sesión";
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(modelo);

        var nombre = modelo.nombre_usuario.Trim();
        var usuario = await _baseDatos.Usuarios
            .Include(elemento => elemento.id_rolNavigation)
            .FirstOrDefaultAsync(elemento => elemento.nombre_usuario == nombre);

        if (usuario == null || !CompararContrasenas(usuario.contrasena, modelo.contrasena))
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            modelo.contrasena = string.Empty;
            return View(modelo);
        }

        var afirmaciones = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()),
            new(ClaimTypes.Name, usuario.nombre_usuario),
            new(ClaimTypes.Role, usuario.id_rolNavigation.nombre_rol)
        };
        var permisos = await _baseDatos.Permisos.AsNoTracking()
            .Where(permiso => permiso.id_rol == usuario.id_rol)
            .Select(permiso => permiso.accion)
            .ToListAsync();
        afirmaciones.AddRange(permisos.Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(permiso => new Claim("permiso", permiso)));
        var identidad = new ClaimsIdentity(afirmaciones, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad),
            new AuthenticationProperties { IsPersistent = false, AllowRefresh = true });

        var esAdministrador = string.Equals(usuario.id_rolNavigation.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase) ||
            permisos.Contains("Gestionar usuarios", StringComparer.OrdinalIgnoreCase);
        var rutaSolicitada = returnUrl?.Split('?')[0].Trim('/');
        var esRutaInicial = string.IsNullOrEmpty(rutaSolicitada) ||
            string.Equals(rutaSolicitada, "Inicio/Tablero", StringComparison.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl) &&
            (esAdministrador || !esRutaInicial))
            return LocalRedirect(returnUrl);
        if (esAdministrador)
            return RedirectToAction("Tablero", "Inicio");
        if (permisos.Contains("Registrar ventas", StringComparer.OrdinalIgnoreCase) ||
            permisos.Contains("Gestionar ventas", StringComparer.OrdinalIgnoreCase))
            return RedirectToAction("Lista", "Ventas");
        if (permisos.Contains("consultar productos", StringComparer.OrdinalIgnoreCase) ||
            permisos.Contains("Gestionar productos", StringComparer.OrdinalIgnoreCase))
            return RedirectToAction("Lista", "Productos");
        if (permisos.Contains("Consultar inventario", StringComparer.OrdinalIgnoreCase) ||
            permisos.Contains("Gestionar inventario", StringComparer.OrdinalIgnoreCase))
            return RedirectToAction("Lista", "Inventario");
        if (permisos.Contains("Consultar clientes", StringComparer.OrdinalIgnoreCase) ||
            permisos.Contains("Registrar clientes", StringComparer.OrdinalIgnoreCase) ||
            permisos.Contains("Gestionar clientes", StringComparer.OrdinalIgnoreCase))
            return RedirectToAction("Lista", "Clientes");
        return RedirectToAction(nameof(Denegado));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(IniciarSesion));
    }

    [HttpGet]
    public IActionResult Denegado()
    {
        ViewData["Title"] = "Acceso denegado";
        return View();
    }

    private static bool CompararContrasenas(string almacenada, string ingresada)
    {
        var bytesAlmacenados = Encoding.UTF8.GetBytes(almacenada);
        var bytesIngresados = Encoding.UTF8.GetBytes(ingresada);
        return bytesAlmacenados.Length == bytesIngresados.Length &&
               CryptographicOperations.FixedTimeEquals(bytesAlmacenados, bytesIngresados);
    }
}
