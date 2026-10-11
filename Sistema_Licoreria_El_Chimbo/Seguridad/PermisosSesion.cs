using System;
using System.Collections.Generic;
using System.Linq;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public static class PermisosSesion
    {
        private static int idRolCargado = -1;
        private static bool administrador;
        private static HashSet<string> permisos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static bool EsAdministrador
        {
            get
            {
                CargarSiHaceFalta();
                return administrador;
            }
        }

        public static bool Tiene(params string[] acciones)
        {
            CargarSiHaceFalta();
            return administrador || acciones.Any(permisos.Contains);
        }

        public static void Recargar()
        {
            idRolCargado = -1;
            CargarSiHaceFalta();
        }

        private static void CargarSiHaceFalta()
        {
            var idRolActual = FrmLogin.IdRolActual;
            if (idRolCargado == idRolActual) return;

            idRolCargado = idRolActual;
            administrador = false;
            permisos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (idRolActual <= 0) return;

            var rol = new RolesNegocio().Listar().FirstOrDefault(elemento => elemento.IdRol == idRolActual);
            administrador = rol != null && string.Equals(rol.NombreRol, "Administrador", StringComparison.OrdinalIgnoreCase);
            permisos = new HashSet<string>(new PermisosNegocio().Listar()
                .Where(elemento => elemento.IdRol == idRolActual)
                .Select(elemento => elemento.Accion), StringComparer.OrdinalIgnoreCase);
        }
    }
}
