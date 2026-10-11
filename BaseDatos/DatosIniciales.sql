USE [Licoreria_El_Chimbo];
GO

IF NOT EXISTS (SELECT 1 FROM [Roles_Usuarios] WHERE [nombre_rol] = 'Administrador')
    INSERT INTO [Roles_Usuarios] ([nombre_rol]) VALUES ('Administrador');
IF NOT EXISTS (SELECT 1 FROM [Roles_Usuarios] WHERE [nombre_rol] = 'Cajero')
    INSERT INTO [Roles_Usuarios] ([nombre_rol]) VALUES ('Cajero');

DECLARE @IdRolAdminPermisos INT = (SELECT TOP (1) [id_rol] FROM [Roles_Usuarios] WHERE [nombre_rol] = 'Administrador' ORDER BY [id_rol]);
DECLARE @IdRolCajeroPermisos INT = (SELECT TOP (1) [id_rol] FROM [Roles_Usuarios] WHERE [nombre_rol] = 'Cajero' ORDER BY [id_rol]);
DECLARE @PermisosAdministrador TABLE ([accion] VARCHAR(100) NOT NULL);
DECLARE @PermisosCajero TABLE ([accion] VARCHAR(100) NOT NULL);

INSERT INTO @PermisosAdministrador ([accion]) VALUES
    ('Ajustar stock'), ('Consultar movimientos de stock'), ('Gestionar categorías'),
    ('Gestionar clientes'), ('Gestionar compras'), ('Gestionar inventario'),
    ('Gestionar marcas'), ('Gestionar métodos de pago'), ('Gestionar permisos'),
    ('Gestionar productos'), ('Gestionar proveedores'), ('Gestionar roles'),
    ('Gestionar usuarios'), ('Gestionar ventas'), ('Consultar clientes'),
    ('Consultar inventario'), ('consultar productos'), ('Registrar clientes'),
    ('Registrar ventas'), ('Usar métodos de pago');

INSERT INTO @PermisosCajero ([accion]) VALUES
    ('Consultar clientes'), ('Consultar inventario'), ('consultar productos'),
    ('Registrar clientes'), ('Registrar ventas'), ('Usar métodos de pago');

INSERT INTO [Permisos] ([id_rol], [accion])
SELECT @IdRolAdminPermisos, [catalogo].[accion]
FROM @PermisosAdministrador AS [catalogo]
WHERE NOT EXISTS (
    SELECT 1 FROM [Permisos] AS [existente]
    WHERE [existente].[id_rol] = @IdRolAdminPermisos
      AND LOWER([existente].[accion]) = LOWER([catalogo].[accion])
);

INSERT INTO [Permisos] ([id_rol], [accion])
SELECT @IdRolCajeroPermisos, [catalogo].[accion]
FROM @PermisosCajero AS [catalogo]
WHERE NOT EXISTS (
    SELECT 1 FROM [Permisos] AS [existente]
    WHERE [existente].[id_rol] = @IdRolCajeroPermisos
      AND LOWER([existente].[accion]) = LOWER([catalogo].[accion])
);

IF NOT EXISTS (SELECT 1 FROM [Categorias_Productos] WHERE [nombre_categoria] = 'Licores')
    INSERT INTO [Categorias_Productos] ([nombre_categoria]) VALUES ('Licores');
IF NOT EXISTS (SELECT 1 FROM [Categorias_Productos] WHERE [nombre_categoria] = 'Cervezas')
    INSERT INTO [Categorias_Productos] ([nombre_categoria]) VALUES ('Cervezas');
IF NOT EXISTS (SELECT 1 FROM [Categorias_Productos] WHERE [nombre_categoria] = 'Vinos')
    INSERT INTO [Categorias_Productos] ([nombre_categoria]) VALUES ('Vinos');
IF NOT EXISTS (SELECT 1 FROM [Categorias_Productos] WHERE [nombre_categoria] = 'Bebidas sin alcohol')
    INSERT INTO [Categorias_Productos] ([nombre_categoria]) VALUES ('Bebidas sin alcohol');

IF NOT EXISTS (SELECT 1 FROM [Marcas] WHERE [nombre_marca] = 'Genérica')
    INSERT INTO [Marcas] ([nombre_marca]) VALUES ('Genérica');

IF NOT EXISTS (SELECT 1 FROM [Metodos_Pago] WHERE [nombre_metodo] = 'Efectivo')
    INSERT INTO [Metodos_Pago] ([nombre_metodo]) VALUES ('Efectivo');
IF NOT EXISTS (SELECT 1 FROM [Metodos_Pago] WHERE [nombre_metodo] = 'Tarjeta')
    INSERT INTO [Metodos_Pago] ([nombre_metodo]) VALUES ('Tarjeta');
IF NOT EXISTS (SELECT 1 FROM [Metodos_Pago] WHERE [nombre_metodo] = 'Transferencia')
    INSERT INTO [Metodos_Pago] ([nombre_metodo]) VALUES ('Transferencia');

DECLARE @NombreUsuario VARCHAR(100) = 'administrador';
DECLARE @ContrasenaInicial VARCHAR(100) = 'REEMPLAZA-ESTA-CLAVE';
DECLARE @IdRolAdministrador INT = (
    SELECT TOP (1) [id_rol]
    FROM [Roles_Usuarios]
    WHERE [nombre_rol] = 'Administrador'
    ORDER BY [id_rol]
);

IF @ContrasenaInicial = 'REEMPLAZA-ESTA-CLAVE'
    THROW 51000, 'Edita DatosIniciales.sql y cambia la contraseña temporal antes de ejecutarlo.', 1;

IF NOT EXISTS (SELECT 1 FROM [Usuarios])
BEGIN
    INSERT INTO [Usuarios] ([nombre_usuario], [contrasena], [id_rol])
    VALUES (@NombreUsuario, @ContrasenaInicial, @IdRolAdministrador);
END;
GO
