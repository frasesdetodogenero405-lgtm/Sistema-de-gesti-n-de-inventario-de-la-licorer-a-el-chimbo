IF DB_ID(N'Licoreria_El_Chimbo') IS NULL
BEGIN
    CREATE DATABASE [Licoreria_El_Chimbo];
END;
GO
USE [Licoreria_El_Chimbo];
GO

IF OBJECT_ID(N'[Historial_Migraciones]') IS NULL
BEGIN
    CREATE TABLE [Historial_Migraciones] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_Historial_Migraciones] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categorias_Productos] (
    [id_categoria] int NOT NULL IDENTITY,
    [nombre_categoria] varchar(50) NOT NULL,
    CONSTRAINT [PK__Categori__CD54BC5A59C82B18] PRIMARY KEY ([id_categoria])
);
GO

CREATE TABLE [Clientes] (
    [id_cliente] int NOT NULL IDENTITY,
    [nombre_cliente] varchar(100) NOT NULL,
    [telefono] varchar(20) NULL,
    [direccion] varchar(200) NULL,
    CONSTRAINT [PK__Clientes__677F38F52D0C220D] PRIMARY KEY ([id_cliente])
);
GO

CREATE TABLE [Licoreria] (
    [Id_Licoreria] int NOT NULL IDENTITY,
    [Nombre] varchar(200) NULL,
    [Direccion] varchar(200) NULL,
    [Telefono] varchar(200) NULL,
    [Correo] varchar(200) NULL,
    CONSTRAINT [PK_Licoreria] PRIMARY KEY ([Id_Licoreria])
);
GO

CREATE TABLE [Marcas] (
    [id_marca] int NOT NULL IDENTITY,
    [nombre_marca] varchar(50) NOT NULL,
    CONSTRAINT [PK__Marcas__7E43E99E6522CC18] PRIMARY KEY ([id_marca])
);
GO

CREATE TABLE [Metodos_Pago] (
    [id_metodo] int NOT NULL IDENTITY,
    [nombre_metodo] varchar(50) NOT NULL,
    CONSTRAINT [PK__Metodos___1BBFF0F4CC1369A8] PRIMARY KEY ([id_metodo])
);
GO

CREATE TABLE [Proveedores] (
    [id_proveedor] int NOT NULL IDENTITY,
    [nombre_proveedor] varchar(100) NOT NULL,
    [telefono] varchar(20) NULL,
    [direccion] varchar(200) NULL,
    CONSTRAINT [PK__Proveedo__8D3DFE287089B27C] PRIMARY KEY ([id_proveedor])
);
GO

CREATE TABLE [Roles_Usuarios] (
    [id_rol] int NOT NULL IDENTITY,
    [nombre_rol] varchar(50) NOT NULL,
    CONSTRAINT [PK__Roles_Us__6ABCB5E0695ACA53] PRIMARY KEY ([id_rol])
);
GO

CREATE TABLE [Productos] (
    [id_producto] int NOT NULL IDENTITY,
    [nombre_producto] varchar(100) NOT NULL,
    [precio_venta] decimal(10,2) NOT NULL,
    [id_categoria] int NOT NULL,
    [id_marca] int NOT NULL,
    CONSTRAINT [PK__Producto__FF341C0DAEE42915] PRIMARY KEY ([id_producto]),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([id_categoria]) REFERENCES [Categorias_Productos] ([id_categoria]),
    CONSTRAINT [FK_Productos_Marcas] FOREIGN KEY ([id_marca]) REFERENCES [Marcas] ([id_marca])
);
GO

CREATE TABLE [Permisos] (
    [id_permiso] int NOT NULL IDENTITY,
    [id_rol] int NOT NULL,
    [accion] varchar(100) NOT NULL,
    CONSTRAINT [PK__Permisos__228F224F361A24F3] PRIMARY KEY ([id_permiso]),
    CONSTRAINT [FK_Permisos_Roles] FOREIGN KEY ([id_rol]) REFERENCES [Roles_Usuarios] ([id_rol])
);
GO

CREATE TABLE [Usuarios] (
    [id_usuario] int NOT NULL IDENTITY,
    [nombre_usuario] varchar(100) NOT NULL,
    [contrasena] varchar(100) NOT NULL,
    [id_rol] int NOT NULL,
    [id_licoreria] int NULL,
    CONSTRAINT [PK__Usuarios__4E3E04ADCBA27014] PRIMARY KEY ([id_usuario]),
    CONSTRAINT [FK_Usuarios_Licoreria] FOREIGN KEY ([id_licoreria]) REFERENCES [Licoreria] ([Id_Licoreria]),
    CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([id_rol]) REFERENCES [Roles_Usuarios] ([id_rol])
);
GO

CREATE TABLE [Inventario] (
    [id_inventario] int NOT NULL IDENTITY,
    [id_producto] int NOT NULL,
    [stock_minimo] int NOT NULL,
    [stock_maximo] int NOT NULL,
    [stock_actual] int NOT NULL,
    CONSTRAINT [PK__Inventar__013AEB51C87635F7] PRIMARY KEY ([id_inventario]),
    CONSTRAINT [FK_Inventario_Productos] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto])
);
GO

CREATE TABLE [Movimientos_Stock] (
    [id_movimiento] int NOT NULL IDENTITY,
    [id_producto] int NOT NULL,
    [tipo_movimiento] varchar(10) NULL,
    [cantidad] int NOT NULL,
    [fecha] datetime NULL DEFAULT ((getdate())),
    [referencia] varchar(100) NULL,
    CONSTRAINT [PK__Movimien__2A071C24DB95AAA8] PRIMARY KEY ([id_movimiento]),
    CONSTRAINT [FK_MovInv_Productos] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto])
);
GO

CREATE TABLE [Compras] (
    [id_compra] int NOT NULL IDENTITY,
    [id_proveedor] int NOT NULL,
    [id_usuario] int NOT NULL,
    [fecha] datetime NULL DEFAULT ((getdate())),
    [total] decimal(10,2) NOT NULL,
    CONSTRAINT [PK__Compras__C4BAA6043EDA0E37] PRIMARY KEY ([id_compra]),
    CONSTRAINT [FK_Compras_Proveedores] FOREIGN KEY ([id_proveedor]) REFERENCES [Proveedores] ([id_proveedor]),
    CONSTRAINT [FK_Compras_Usuarios] FOREIGN KEY ([id_usuario]) REFERENCES [Usuarios] ([id_usuario])
);
GO

CREATE TABLE [Ventas] (
    [id_venta] int NOT NULL IDENTITY,
    [id_cliente] int NOT NULL,
    [id_usuario] int NOT NULL,
    [fecha] datetime NULL DEFAULT ((getdate())),
    [total] decimal(10,2) NOT NULL,
    [metodo_pago] varchar(50) NOT NULL DEFAULT 'Efectivo',
    CONSTRAINT [PK__Ventas__459533BFE998FCE4] PRIMARY KEY ([id_venta]),
    CONSTRAINT [FK_Ventas_Clientes] FOREIGN KEY ([id_cliente]) REFERENCES [Clientes] ([id_cliente]),
    CONSTRAINT [FK_Ventas_Usuarios] FOREIGN KEY ([id_usuario]) REFERENCES [Usuarios] ([id_usuario])
);
GO

CREATE TABLE [Detalle_Compras] (
    [id_detalle] int NOT NULL IDENTITY,
    [id_compra] int NOT NULL,
    [id_producto] int NOT NULL,
    [cantidad] int NOT NULL,
    [precio_compra] decimal(10,2) NOT NULL,
    CONSTRAINT [PK__Detalle___4F1332DE7B0791B6] PRIMARY KEY ([id_detalle]),
    CONSTRAINT [FK_DetalleCompras_Compras] FOREIGN KEY ([id_compra]) REFERENCES [Compras] ([id_compra]),
    CONSTRAINT [FK_DetalleCompras_Productos] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto])
);
GO

CREATE TABLE [Pagos_Compras] (
    [id_pago_compra] int NOT NULL IDENTITY,
    [id_compra] int NOT NULL,
    [id_metodo] int NOT NULL,
    [monto] decimal(10,2) NOT NULL,
    [fecha_pago] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Pagos_Co__7CD8E8816053BEF9] PRIMARY KEY ([id_pago_compra]),
    CONSTRAINT [FK_PagosCompras_Compras] FOREIGN KEY ([id_compra]) REFERENCES [Compras] ([id_compra]),
    CONSTRAINT [FK_PagosCompras_Metodos] FOREIGN KEY ([id_metodo]) REFERENCES [Metodos_Pago] ([id_metodo])
);
GO

CREATE TABLE [Detalle_Ventas] (
    [id_detalle] int NOT NULL IDENTITY,
    [id_venta] int NOT NULL,
    [id_producto] int NOT NULL,
    [cantidad] int NOT NULL,
    [precio_venta] decimal(10,2) NOT NULL,
    CONSTRAINT [PK__Detalle___4F1332DE44C1EFF6] PRIMARY KEY ([id_detalle]),
    CONSTRAINT [FK_DetalleVentas_Productos] FOREIGN KEY ([id_producto]) REFERENCES [Productos] ([id_producto]),
    CONSTRAINT [FK_DetalleVentas_Ventas] FOREIGN KEY ([id_venta]) REFERENCES [Ventas] ([id_venta])
);
GO

CREATE TABLE [Pagos_Ventas] (
    [id_pago] int NOT NULL IDENTITY,
    [id_venta] int NOT NULL,
    [id_metodo] int NOT NULL,
    [monto] decimal(10,2) NOT NULL,
    [fecha_pago] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Pagos_Ve__0941B074EE350B3C] PRIMARY KEY ([id_pago]),
    CONSTRAINT [FK_PagosVentas_Metodos] FOREIGN KEY ([id_metodo]) REFERENCES [Metodos_Pago] ([id_metodo]),
    CONSTRAINT [FK_PagosVentas_Ventas] FOREIGN KEY ([id_venta]) REFERENCES [Ventas] ([id_venta])
);
GO

CREATE INDEX [IX_Compras_id_proveedor] ON [Compras] ([id_proveedor]);
GO

CREATE INDEX [IX_Compras_id_usuario] ON [Compras] ([id_usuario]);
GO

CREATE INDEX [IX_Detalle_Compras_id_compra] ON [Detalle_Compras] ([id_compra]);
GO

CREATE INDEX [IX_Detalle_Compras_id_producto] ON [Detalle_Compras] ([id_producto]);
GO

CREATE INDEX [IX_Detalle_Ventas_id_producto] ON [Detalle_Ventas] ([id_producto]);
GO

CREATE INDEX [IX_Detalle_Ventas_id_venta] ON [Detalle_Ventas] ([id_venta]);
GO

CREATE INDEX [IX_Inventario_id_producto] ON [Inventario] ([id_producto]);
GO

CREATE INDEX [IX_Movimientos_Stock_id_producto] ON [Movimientos_Stock] ([id_producto]);
GO

CREATE INDEX [IX_Pagos_Compras_id_compra] ON [Pagos_Compras] ([id_compra]);
GO

CREATE INDEX [IX_Pagos_Compras_id_metodo] ON [Pagos_Compras] ([id_metodo]);
GO

CREATE INDEX [IX_Pagos_Ventas_id_metodo] ON [Pagos_Ventas] ([id_metodo]);
GO

CREATE INDEX [IX_Pagos_Ventas_id_venta] ON [Pagos_Ventas] ([id_venta]);
GO

CREATE INDEX [IX_Permisos_id_rol] ON [Permisos] ([id_rol]);
GO

CREATE INDEX [IX_Productos_id_categoria] ON [Productos] ([id_categoria]);
GO

CREATE INDEX [IX_Productos_id_marca] ON [Productos] ([id_marca]);
GO

CREATE INDEX [IX_Usuarios_id_licoreria] ON [Usuarios] ([id_licoreria]);
GO

CREATE INDEX [IX_Usuarios_id_rol] ON [Usuarios] ([id_rol]);
GO

CREATE INDEX [IX_Ventas_id_cliente] ON [Ventas] ([id_cliente]);
GO

CREATE INDEX [IX_Ventas_id_usuario] ON [Ventas] ([id_usuario]);
GO

INSERT INTO [Historial_Migraciones] ([MigrationId], [ProductVersion])
VALUES (N'20261010234459_EsquemaInicial', N'8.0.31');
GO

COMMIT;
GO
