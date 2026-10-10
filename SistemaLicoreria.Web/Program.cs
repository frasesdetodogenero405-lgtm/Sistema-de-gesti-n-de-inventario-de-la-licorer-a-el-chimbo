using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Datos;

var constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllersWithViews(opciones =>
    opciones.Filters.Add(new AuthorizeFilter()));
constructor.Services.AddDbContext<LicoreriaContexto>(opciones =>
    opciones.UseSqlServer(
        constructor.Configuration.GetConnectionString("LicoreriaDb"),
        sql => sql.MigrationsHistoryTable("Historial_Migraciones")));
constructor.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.LoginPath = "/Acceso/IniciarSesion";
        opciones.AccessDeniedPath = "/Acceso/Denegado";
        opciones.Cookie.Name = "Licoreria.Autenticacion";
        opciones.Cookie.HttpOnly = true;
        opciones.Cookie.SameSite = SameSiteMode.Lax;
        opciones.ExpireTimeSpan = TimeSpan.FromHours(8);
        opciones.SlidingExpiration = true;
    });

var aplicacion = constructor.Build();

if (!aplicacion.Environment.IsDevelopment())
{
    aplicacion.UseExceptionHandler("/Inicio/Error");
    aplicacion.UseHsts();
}

aplicacion.UseStaticFiles();
aplicacion.UseRouting();
aplicacion.UseAuthentication();
aplicacion.UseAuthorization();
aplicacion.MapControllerRoute(
    name: "predeterminada",
    pattern: "{controller=Inicio}/{action=Tablero}/{id?}");

aplicacion.Run();
