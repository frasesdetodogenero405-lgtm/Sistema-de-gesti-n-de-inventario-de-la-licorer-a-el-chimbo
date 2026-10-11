# Sistema de gestión de inventario — Licorería El Chimbo

El repositorio conserva la aplicación de escritorio WinForms y agrega una plataforma web en español. La web usa ASP.NET Core MVC, Entity Framework Core, SQL Server, Bootstrap 5, JavaScript y Chart.js. YARP funciona como puerta de entrada local y reenvía las solicitudes a la aplicación MVC.

## Proyectos

- `SistemaLicoreria.Web`: tablero, autenticación, productos, marcas, categorías, métodos de pago, inventario, clientes, proveedores, ventas, compras y administración de roles y permisos.
- `SistemaLicoreria.Proxy`: proxy inverso YARP, que entrega la aplicación web en `http://127.0.0.1:5070`.
- `CapaDatos`, `CapaEntidad`, `CapaNegocio` y `Sistema_Licoreria_El_Chimbo`: aplicación de escritorio existente.

El contexto y los modelos de Entity Framework se generaron desde el esquema SQL actual. Las tablas y campos de negocio conservan sus nombres en español. La aplicación web no ejecuta migraciones ni modifica el esquema automáticamente.

Los permisos se guardan en `Permisos` y se validan en las acciones web del servidor. El administrador tiene acceso completo; el cajero puede consultar productos, inventario y clientes, registrar clientes y ventas, y usar métodos de pago. El cajero no puede administrar productos, proveedores, compras, usuarios, roles ni permisos. La pantalla **Roles y permisos** permite crear roles, seleccionar permisos y asignar roles a usuarios. La aplicación de escritorio también oculta las opciones no permitidas y protege las acciones de edición de productos, clientes e inventario.

## Requisitos

- Visual Studio 2022 con Desarrollo web de ASP.NET y Desarrollo de escritorio de .NET.
- SDK y runtime de .NET 8.
- SQL Server Express con la base `Licoreria_El_Chimbo`.
- Un usuario registrado en la tabla `Usuarios`.

## Iniciar en esta computadora

Ejecuta `IniciarSistema.bat`. El archivo abre la solución en Visual Studio 2022 y levanta MVC y YARP si sus puertos están libres. La aplicación queda en `http://127.0.0.1:5070`; MVC también responde en `http://127.0.0.1:5080`. Si ya hay un proceso en uno de esos puertos, el script no inicia otro.

Para depurar desde Visual Studio, configura `SistemaLicoreria.Web` y `SistemaLicoreria.Proxy` como proyectos de inicio simultáneos.

## Conexión a SQL Server

La cadena local está en `SistemaLicoreria.Web\appsettings.json`:

```text
Server=.\SQLEXPRESS;Database=Licoreria_El_Chimbo;Trusted_Connection=True;TrustServerCertificate=True;
```

Si la instancia o la base tienen otro nombre, cambia `LicoreriaDb`. No se incluyen registros de clientes, usuarios ni productos en GitHub. El tablero consulta ventas y compras de la base existente; los formularios web de ventas y compras guardan cabecera, detalles, existencias y movimientos en una transacción.

Para preparar una computadora con una base vacía, abre SQL Server Management Studio y ejecuta `BaseDatos\CrearEsquema.sql`. Luego cambia `REEMPLAZA-ESTA-CLAVE` por una clave propia en `BaseDatos\DatosIniciales.sql` y ejecuta ese archivo para crear catálogos básicos y la primera cuenta administradora. **No ejecutes el archivo de esquema sobre la base que ya usa el negocio**: contiene instrucciones para crear tablas y se usa solo en una base nueva.

## Componentes web

| Tecnología | Uso |
| --- | --- |
| C# | Lógica del sistema |
| ASP.NET Core MVC | Aplicación web y vistas |
| YARP | Puerta de entrada y proxy inverso |
| SQL Server | Datos del inventario y las operaciones |
| Entity Framework Core | Lectura y escritura de la base de datos |
| HTML, CSS y JavaScript | Interfaz web |
| Bootstrap 5 | Diseño adaptable |
| Chart.js | Gráfico de ventas y compras por mes |
| Visual Studio 2022 | Desarrollo y ejecución |

## Estado y limitaciones

- La base de esta computadora se validó y responde. Para usar la web en otra máquina, primero debe instalarse SQL Server y crearse la misma base; el repositorio no incluye datos ni un respaldo.
- El inicio de sesión web usa las cuentas existentes. El esquema heredado almacena contraseñas en texto; antes de publicar la aplicación fuera de la computadora local, se debe migrar a contraseñas protegidas y acceso HTTPS.
- La web permite registrar, buscar, editar y eliminar marcas y categorías; impide borrar catálogos asociados a productos. La web administra los roles, sus permisos y la asignación de roles a usuarios.
- Al cambiar permisos o asignar otro rol, la persona debe cerrar y volver a iniciar sesión para que el menú muestre los accesos nuevos. La autorización del servidor consulta el rol y los permisos vigentes en la base de datos.
- La solución completa se compila con MSBuild de Visual Studio. Para verificar la web por separado, compila `SistemaLicoreria.Web` y `SistemaLicoreria.Proxy`.
