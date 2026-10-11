using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SistemaLicoreria.Web.Datos;
using SistemaLicoreria.Web.Modelos;
using SistemaLicoreria.Web.ModelosVista;
using SistemaLicoreria.Web.Seguridad;

namespace SistemaLicoreria.Web.Controllers;

[SoloAdministrador]
public class AdministracionController : Controller
{
    private static readonly PermisoDisponibleViewModel[] CatalogoPermisos =
    {
        new() { modulo = "Tablero", accion = "Gestionar usuarios", descripcion = "Ver el tablero y sus indicadores" },
        new() { modulo = "Productos", accion = "consultar productos", descripcion = "Consultar el catálogo de productos" },
        new() { modulo = "Productos", accion = "Gestionar productos", descripcion = "Crear, editar y eliminar productos" },
        new() { modulo = "Inventario", accion = "Consultar inventario", descripcion = "Consultar existencias" },
        new() { modulo = "Inventario", accion = "Gestionar inventario", descripcion = "Administrar el inventario" },
        new() { modulo = "Inventario", accion = "Ajustar stock", descripcion = "Ajustar cantidades disponibles" },
        new() { modulo = "Inventario", accion = "Consultar movimientos de stock", descripcion = "Ver entradas y salidas de productos" },
        new() { modulo = "Clientes", accion = "Consultar clientes", descripcion = "Consultar el directorio de clientes" },
        new() { modulo = "Clientes", accion = "Registrar clientes", descripcion = "Registrar clientes nuevos" },
        new() { modulo = "Clientes", accion = "Gestionar clientes", descripcion = "Editar y eliminar clientes" },
        new() { modulo = "Proveedores", accion = "Gestionar proveedores", descripcion = "Consultar y administrar proveedores" },
        new() { modulo = "Ventas", accion = "Registrar ventas", descripcion = "Registrar ventas y usar métodos de pago" },
        new() { modulo = "Ventas", accion = "Gestionar ventas", descripcion = "Consultar y administrar ventas" },
        new() { modulo = "Compras", accion = "Gestionar compras", descripcion = "Consultar y registrar compras" },
        new() { modulo = "Catálogos", accion = "Gestionar categorías", descripcion = "Administrar categorías de productos" },
        new() { modulo = "Catálogos", accion = "Gestionar marcas", descripcion = "Administrar marcas" },
        new() { modulo = "Catálogos", accion = "Gestionar métodos de pago", descripcion = "Administrar métodos de pago" },
        new() { modulo = "Catálogos", accion = "Usar métodos de pago", descripcion = "Seleccionar métodos de pago en ventas" },
        new() { modulo = "Administración", accion = "Gestionar roles", descripcion = "Crear y modificar roles" },
        new() { modulo = "Administración", accion = "Gestionar permisos", descripcion = "Asignar permisos a los roles" }
    };

    private readonly LicoreriaContexto _baseDatos;

    public AdministracionController(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    [HttpGet]
    public async Task<IActionResult> Roles()
    {
        ViewData["Title"] = "Roles y permisos";
        ViewData["Seccion"] = "Administración";
        var roles = await _baseDatos.Roles_Usuarios.AsNoTracking()
            .OrderBy(rol => rol.nombre_rol)
            .Select(rol => new RolListaViewModel
            {
                id_rol = rol.id_rol,
                nombre_rol = rol.nombre_rol,
                cantidad_usuarios = rol.Usuarios.Count,
                cantidad_permisos = rol.Permisos.Count
            })
            .ToListAsync();
        return View(roles);
    }

    [HttpGet]
    public async Task<IActionResult> Usuarios()
    {
        var modelo = new UsuariosRolesViewModel
        {
            id_usuario_sesion = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0,
            roles = await _baseDatos.Roles_Usuarios.AsNoTracking().OrderBy(rol => rol.nombre_rol)
                .Select(rol => new SelectListItem(rol.nombre_rol, rol.id_rol.ToString())).ToListAsync(),
            usuarios = await _baseDatos.Usuarios.AsNoTracking().OrderBy(usuario => usuario.nombre_usuario)
                .Select(usuario => new UsuarioRolViewModel
                {
                    id_usuario = usuario.id_usuario,
                    nombre_usuario = usuario.nombre_usuario,
                    id_rol = usuario.id_rol,
                    nombre_rol = usuario.id_rolNavigation.nombre_rol
                }).ToListAsync()
        };
        ViewData["Title"] = "Usuarios y roles";
        ViewData["Seccion"] = "Administración";
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarRolUsuario(int id, int id_rol)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var idUsuarioSesion))
            return Forbid();
        if (id == idUsuarioSesion)
        {
            TempData["Error"] = "No puedes cambiar el rol de tu propia cuenta mientras tienes la sesión iniciada.";
            return RedirectToAction(nameof(Usuarios));
        }

        var usuario = await _baseDatos.Usuarios.Include(elemento => elemento.id_rolNavigation)
            .FirstOrDefaultAsync(elemento => elemento.id_usuario == id);
        var nuevoRol = await _baseDatos.Roles_Usuarios.FirstOrDefaultAsync(rol => rol.id_rol == id_rol);
        if (usuario == null || nuevoRol == null) return NotFound();

        if (string.Equals(usuario.id_rolNavigation.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(nuevoRol.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase))
        {
            var administradores = await _baseDatos.Usuarios.CountAsync(elemento => elemento.id_rolNavigation.nombre_rol == "Administrador");
            if (administradores <= 1)
            {
                TempData["Error"] = "Debe quedar al menos un usuario administrador.";
                return RedirectToAction(nameof(Usuarios));
            }
        }

        usuario.id_rol = nuevoRol.id_rol;
        await _baseDatos.SaveChangesAsync();
        TempData["Mensaje"] = "Rol actualizado. El usuario debe cerrar y volver a iniciar sesión para aplicar los nuevos permisos.";
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpGet]
    public async Task<IActionResult> EditarRol(int? id)
    {
        var modelo = new RolFormularioViewModel();
        if (id.HasValue)
        {
            var rol = await _baseDatos.Roles_Usuarios.AsNoTracking()
                .Include(elemento => elemento.Permisos)
                .FirstOrDefaultAsync(elemento => elemento.id_rol == id.Value);
            if (rol == null) return NotFound();
            modelo.id_rol = rol.id_rol;
            modelo.nombre_rol = rol.nombre_rol;
            modelo.permisos_seleccionados = rol.Permisos.Select(permiso => permiso.accion).ToList();
        }

        PrepararVista(modelo);
        return View("FormularioRol", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarRol(RolFormularioViewModel modelo)
    {
        modelo.nombre_rol = (modelo.nombre_rol ?? string.Empty).Trim();
        modelo.permisos_seleccionados ??= new List<string>();
        var accionesCatalogadas = CatalogoPermisos.Select(permiso => permiso.accion).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (modelo.permisos_seleccionados.Any(accion => !accionesCatalogadas.Contains(accion)))
            ModelState.AddModelError(nameof(modelo.permisos_seleccionados), "La selección contiene un permiso que no existe.");
        if (await _baseDatos.Roles_Usuarios.AnyAsync(rol => rol.nombre_rol == modelo.nombre_rol && rol.id_rol != modelo.id_rol))
            ModelState.AddModelError(nameof(modelo.nombre_rol), "Ya existe un rol con ese nombre.");

        var existente = modelo.id_rol.HasValue
            ? await _baseDatos.Roles_Usuarios.Include(rol => rol.Permisos).FirstOrDefaultAsync(rol => rol.id_rol == modelo.id_rol.Value)
            : null;
        if (modelo.id_rol.HasValue && existente == null) return NotFound();
        if (existente != null && string.Equals(existente.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(modelo.nombre_rol, existente.nombre_rol, StringComparison.OrdinalIgnoreCase))
            ModelState.AddModelError(nameof(modelo.nombre_rol), "El nombre del rol Administrador está protegido.");

        if (!ModelState.IsValid)
        {
            PrepararVista(modelo);
            return View("FormularioRol", modelo);
        }

        await using var transaccion = await _baseDatos.Database.BeginTransactionAsync();
        var seleccionado = modelo.permisos_seleccionados.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (existente == null)
        {
            existente = new Roles_Usuario { nombre_rol = modelo.nombre_rol };
            _baseDatos.Roles_Usuarios.Add(existente);
        }
        else
        {
            existente.nombre_rol = modelo.nombre_rol;
            _baseDatos.Permisos.RemoveRange(existente.Permisos);
        }

        foreach (var permiso in CatalogoPermisos.Where(permiso => seleccionado.Contains(permiso.accion)))
        {
            existente.Permisos.Add(new Permiso { id_rolNavigation = existente, accion = permiso.accion });
        }

        await _baseDatos.SaveChangesAsync();
        await transaccion.CommitAsync();
        TempData["Mensaje"] = "El rol y sus permisos se guardaron correctamente. Los usuarios deben cerrar y volver a iniciar sesión para actualizar su menú.";
        return RedirectToAction(nameof(Roles));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarRol(int id)
    {
        var rol = await _baseDatos.Roles_Usuarios.Include(elemento => elemento.Permisos)
            .Include(elemento => elemento.Usuarios)
            .FirstOrDefaultAsync(elemento => elemento.id_rol == id);
        if (rol == null) return NotFound();
        if (string.Equals(rol.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            TempData["Error"] = "El rol Administrador no se puede eliminar.";
        else if (rol.Usuarios.Count > 0)
            TempData["Error"] = "No se puede eliminar un rol que todavía tiene usuarios asignados.";
        else
        {
            _baseDatos.Permisos.RemoveRange(rol.Permisos);
            _baseDatos.Roles_Usuarios.Remove(rol);
            await _baseDatos.SaveChangesAsync();
            TempData["Mensaje"] = "Rol eliminado.";
        }
        return RedirectToAction(nameof(Roles));
    }

    private void PrepararVista(RolFormularioViewModel modelo)
    {
        modelo.permisos_disponibles = CatalogoPermisos;
        ViewData["Title"] = modelo.id_rol.HasValue ? "Editar rol" : "Crear rol";
        ViewData["Seccion"] = "Administración";
    }
}
