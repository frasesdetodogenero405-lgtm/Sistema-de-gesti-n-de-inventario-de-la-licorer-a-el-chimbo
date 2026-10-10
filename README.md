# Sistema de gestión de inventario — Licorería El Chimbo

Aplicación de escritorio Windows Forms para administrar productos, inventario, compras, ventas, clientes, proveedores, usuarios y métodos de pago. Está organizada en tres capas: presentación, negocio y datos; las entidades se comparten desde `CapaEntidad`.

## Requisitos

- Visual Studio 2022 con la carga de trabajo **Desarrollo de escritorio de .NET**.
- .NET Framework 4.7.2 Developer Pack.
- SQL Server Express y una base de datos `Licoreria_El_Chimbo` con las tablas y procedimientos usados por la aplicación.

## Abrir y compilar

Abre `Sistema_Licoreria_El_Chimbo\Sistema_Licoreria_El_Chimbo.sln` en Visual Studio y compila la solución. El ejecutable de depuración se genera en `Sistema_Licoreria_El_Chimbo\bin\Debug`.

## Configurar SQL Server

La cadena `LicoreriaDb` está en `Sistema_Licoreria_El_Chimbo\App.config`. De forma predeterminada usa la instancia local `SQLEXPRESS`, autenticación integrada de Windows y la base `Licoreria_El_Chimbo`. Si tu instancia tiene otro nombre, cambia el valor `Server` antes de compilar.

El código no incluye datos de clientes, usuarios ni una copia de la base local. La base debe existir en SQL Server antes de iniciar sesión; `Conexion.ProbarConexion()` permite comprobar la conectividad.

## Registro de ventas

Cada venta guarda cabecera, detalles, pago, movimientos de inventario y descuento de stock dentro de una sola transacción. Si falla cualquiera de esos pasos, se revierte la operación completa para evitar ventas parciales o inventario inconsistente.
