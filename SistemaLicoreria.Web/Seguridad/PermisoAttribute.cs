using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;

namespace SistemaLicoreria.Web.Seguridad;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class PermisoAttribute : TypeFilterAttribute
{
    public PermisoAttribute(params string[] acciones) : base(typeof(ValidarPermisoFiltro))
    {
        Arguments = new object[] { acciones };
    }
}

public sealed class ValidarPermisoFiltro : IAsyncAuthorizationFilter
{
    private readonly string[] _accionesPermitidas;
    private readonly LicoreriaContexto _baseDatos;

    public ValidarPermisoFiltro(string[] accionesPermitidas, LicoreriaContexto baseDatos)
    {
        _accionesPermitidas = accionesPermitidas;
        _baseDatos = baseDatos;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext contexto)
    {
        var usuario = contexto.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
        {
            contexto.Result = new ChallengeResult();
            return;
        }

        if (!int.TryParse(usuario.FindFirstValue(ClaimTypes.NameIdentifier), out var idUsuario))
        {
            contexto.Result = new ForbidResult();
            return;
        }

        var cuenta = await _baseDatos.Usuarios.AsNoTracking()
            .Include(elemento => elemento.id_rolNavigation)
            .ThenInclude(rol => rol.Permisos)
            .FirstOrDefaultAsync(elemento => elemento.id_usuario == idUsuario);

        if (cuenta == null)
        {
            contexto.Result = new ForbidResult();
            return;
        }

        if (string.Equals(cuenta.id_rolNavigation.nombre_rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            return;

        if (!cuenta.id_rolNavigation.Permisos.Any(permiso =>
                _accionesPermitidas.Contains(permiso.accion, StringComparer.OrdinalIgnoreCase)))
            contexto.Result = new ForbidResult();
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class SoloAdministradorAttribute : TypeFilterAttribute
{
    public SoloAdministradorAttribute() : base(typeof(ValidarAdministradorFiltro))
    {
    }
}

public sealed class ValidarAdministradorFiltro : IAsyncAuthorizationFilter
{
    private readonly LicoreriaContexto _baseDatos;

    public ValidarAdministradorFiltro(LicoreriaContexto baseDatos)
    {
        _baseDatos = baseDatos;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext contexto)
    {
        var usuario = contexto.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
        {
            contexto.Result = new ChallengeResult();
            return;
        }

        if (!int.TryParse(usuario.FindFirstValue(ClaimTypes.NameIdentifier), out var idUsuario) ||
            !await _baseDatos.Usuarios.AsNoTracking().AnyAsync(elemento =>
                elemento.id_usuario == idUsuario && elemento.id_rolNavigation.nombre_rol == "Administrador"))
        {
            contexto.Result = new ForbidResult();
        }
    }
}
