using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaLicoreria.Web.Datos.Migraciones
{
    /// <inheritdoc />
    public partial class EsquemaInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias_Productos",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_categoria = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__CD54BC5A59C82B18", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_cliente = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__677F38F52D0C220D", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Licoreria",
                columns: table => new
                {
                    Id_Licoreria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Telefono = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Correo = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licoreria", x => x.Id_Licoreria);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    id_marca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_marca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Marcas__7E43E99E6522CC18", x => x.id_marca);
                });

            migrationBuilder.CreateTable(
                name: "Metodos_Pago",
                columns: table => new
                {
                    id_metodo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_metodo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Metodos___1BBFF0F4CC1369A8", x => x.id_metodo);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_proveedor = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__8D3DFE287089B27C", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "Roles_Usuarios",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles_Us__6ABCB5E0695ACA53", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_producto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    precio_venta = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    id_categoria = table.Column<int>(type: "int", nullable: false),
                    id_marca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__FF341C0DAEE42915", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias",
                        column: x => x.id_categoria,
                        principalTable: "Categorias_Productos",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "FK_Productos_Marcas",
                        column: x => x.id_marca,
                        principalTable: "Marcas",
                        principalColumn: "id_marca");
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    id_permiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    accion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permisos__228F224F361A24F3", x => x.id_permiso);
                    table.ForeignKey(
                        name: "FK_Permisos_Roles",
                        column: x => x.id_rol,
                        principalTable: "Roles_Usuarios",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_usuario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    contrasena = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    id_licoreria = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__4E3E04ADCBA27014", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Licoreria",
                        column: x => x.id_licoreria,
                        principalTable: "Licoreria",
                        principalColumn: "Id_Licoreria");
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles",
                        column: x => x.id_rol,
                        principalTable: "Roles_Usuarios",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    id_inventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    stock_minimo = table.Column<int>(type: "int", nullable: false),
                    stock_maximo = table.Column<int>(type: "int", nullable: false),
                    stock_actual = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventar__013AEB51C87635F7", x => x.id_inventario);
                    table.ForeignKey(
                        name: "FK_Inventario_Productos",
                        column: x => x.id_producto,
                        principalTable: "Productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "Movimientos_Stock",
                columns: table => new
                {
                    id_movimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    referencia = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Movimien__2A071C24DB95AAA8", x => x.id_movimiento);
                    table.ForeignKey(
                        name: "FK_MovInv_Productos",
                        column: x => x.id_producto,
                        principalTable: "Productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    id_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_proveedor = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Compras__C4BAA6043EDA0E37", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_Compras_Proveedores",
                        column: x => x.id_proveedor,
                        principalTable: "Proveedores",
                        principalColumn: "id_proveedor");
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    id_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    metodo_pago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "Efectivo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__459533BFE998FCE4", x => x.id_venta);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "FK_Ventas_Usuarios",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Compras",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_compra = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_compra = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___4F1332DE7B0791B6", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_DetalleCompras_Compras",
                        column: x => x.id_compra,
                        principalTable: "Compras",
                        principalColumn: "id_compra");
                    table.ForeignKey(
                        name: "FK_DetalleCompras_Productos",
                        column: x => x.id_producto,
                        principalTable: "Productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "Pagos_Compras",
                columns: table => new
                {
                    id_pago_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_compra = table.Column<int>(type: "int", nullable: false),
                    id_metodo = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos_Co__7CD8E8816053BEF9", x => x.id_pago_compra);
                    table.ForeignKey(
                        name: "FK_PagosCompras_Compras",
                        column: x => x.id_compra,
                        principalTable: "Compras",
                        principalColumn: "id_compra");
                    table.ForeignKey(
                        name: "FK_PagosCompras_Metodos",
                        column: x => x.id_metodo,
                        principalTable: "Metodos_Pago",
                        principalColumn: "id_metodo");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Ventas",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_venta = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___4F1332DE44C1EFF6", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_DetalleVentas_Productos",
                        column: x => x.id_producto,
                        principalTable: "Productos",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "FK_DetalleVentas_Ventas",
                        column: x => x.id_venta,
                        principalTable: "Ventas",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "Pagos_Ventas",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: false),
                    id_metodo = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pagos_Ve__0941B074EE350B3C", x => x.id_pago);
                    table.ForeignKey(
                        name: "FK_PagosVentas_Metodos",
                        column: x => x.id_metodo,
                        principalTable: "Metodos_Pago",
                        principalColumn: "id_metodo");
                    table.ForeignKey(
                        name: "FK_PagosVentas_Ventas",
                        column: x => x.id_venta,
                        principalTable: "Ventas",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_id_proveedor",
                table: "Compras",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_id_usuario",
                table: "Compras",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Compras_id_compra",
                table: "Detalle_Compras",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Compras_id_producto",
                table: "Detalle_Compras",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Ventas_id_producto",
                table: "Detalle_Ventas",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Ventas_id_venta",
                table: "Detalle_Ventas",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_id_producto",
                table: "Inventario",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_Stock_id_producto",
                table: "Movimientos_Stock",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Compras_id_compra",
                table: "Pagos_Compras",
                column: "id_compra");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Compras_id_metodo",
                table: "Pagos_Compras",
                column: "id_metodo");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Ventas_id_metodo",
                table: "Pagos_Ventas",
                column: "id_metodo");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_Ventas_id_venta",
                table: "Pagos_Ventas",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_id_rol",
                table: "Permisos",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_id_categoria",
                table: "Productos",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_id_marca",
                table: "Productos",
                column: "id_marca");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_licoreria",
                table: "Usuarios",
                column: "id_licoreria");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_rol",
                table: "Usuarios",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_id_cliente",
                table: "Ventas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_id_usuario",
                table: "Ventas",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalle_Compras");

            migrationBuilder.DropTable(
                name: "Detalle_Ventas");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Movimientos_Stock");

            migrationBuilder.DropTable(
                name: "Pagos_Compras");

            migrationBuilder.DropTable(
                name: "Pagos_Ventas");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Metodos_Pago");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Categorias_Productos");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Licoreria");

            migrationBuilder.DropTable(
                name: "Roles_Usuarios");
        }
    }
}
