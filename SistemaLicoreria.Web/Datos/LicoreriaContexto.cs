using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaLicoreria.Web.Modelos;

namespace SistemaLicoreria.Web.Datos;

public partial class LicoreriaContexto : DbContext
{
    public LicoreriaContexto(DbContextOptions<LicoreriaContexto> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorias_Producto> Categorias_Productos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Detalle_Compra> Detalle_Compras { get; set; }

    public virtual DbSet<Detalle_Venta> Detalle_Ventas { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Licoreria> Licoreria { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Metodos_Pago> Metodos_Pagos { get; set; }

    public virtual DbSet<Movimientos_Stock> Movimientos_Stocks { get; set; }

    public virtual DbSet<Pagos_Compra> Pagos_Compras { get; set; }

    public virtual DbSet<Pagos_Venta> Pagos_Ventas { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<Roles_Usuario> Roles_Usuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorias_Producto>(entity =>
        {
            entity.HasKey(e => e.id_categoria).HasName("PK__Categori__CD54BC5A59C82B18");

            entity.Property(e => e.nombre_categoria)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.id_cliente).HasName("PK__Clientes__677F38F52D0C220D");

            entity.Property(e => e.direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.nombre_cliente)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.id_compra).HasName("PK__Compras__C4BAA6043EDA0E37");

            entity.Property(e => e.fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_proveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.id_proveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Proveedores");

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.id_usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Usuarios");
        });

        modelBuilder.Entity<Detalle_Compra>(entity =>
        {
            entity.HasKey(e => e.id_detalle).HasName("PK__Detalle___4F1332DE7B0791B6");

            entity.Property(e => e.precio_compra).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_compraNavigation).WithMany(p => p.Detalle_Compras)
                .HasForeignKey(d => d.id_compra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompras_Compras");

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.Detalle_Compras)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompras_Productos");
        });

        modelBuilder.Entity<Detalle_Venta>(entity =>
        {
            entity.HasKey(e => e.id_detalle).HasName("PK__Detalle___4F1332DE44C1EFF6");

            entity.Property(e => e.precio_venta).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.Detalle_Venta)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Productos");

            entity.HasOne(d => d.id_ventaNavigation).WithMany(p => p.Detalle_Venta)
                .HasForeignKey(d => d.id_venta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentas_Ventas");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.id_inventario).HasName("PK__Inventar__013AEB51C87635F7");

            entity.ToTable("Inventario");

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Productos");
        });

        modelBuilder.Entity<Licoreria>(entity =>
        {
            entity.HasKey(e => e.Id_Licoreria);

            entity.Property(e => e.Correo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.id_marca).HasName("PK__Marcas__7E43E99E6522CC18");

            entity.Property(e => e.nombre_marca)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Metodos_Pago>(entity =>
        {
            entity.HasKey(e => e.id_metodo).HasName("PK__Metodos___1BBFF0F4CC1369A8");

            entity.ToTable("Metodos_Pago");

            entity.Property(e => e.nombre_metodo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movimientos_Stock>(entity =>
        {
            entity.HasKey(e => e.id_movimiento).HasName("PK__Movimien__2A071C24DB95AAA8");

            entity.ToTable("Movimientos_Stock");

            entity.Property(e => e.fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.referencia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.tipo_movimiento)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.Movimientos_Stocks)
                .HasForeignKey(d => d.id_producto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovInv_Productos");
        });

        modelBuilder.Entity<Pagos_Compra>(entity =>
        {
            entity.HasKey(e => e.id_pago_compra).HasName("PK__Pagos_Co__7CD8E8816053BEF9");

            entity.Property(e => e.fecha_pago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_compraNavigation).WithMany(p => p.Pagos_Compras)
                .HasForeignKey(d => d.id_compra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PagosCompras_Compras");

            entity.HasOne(d => d.id_metodoNavigation).WithMany(p => p.Pagos_Compras)
                .HasForeignKey(d => d.id_metodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PagosCompras_Metodos");
        });

        modelBuilder.Entity<Pagos_Venta>(entity =>
        {
            entity.HasKey(e => e.id_pago).HasName("PK__Pagos_Ve__0941B074EE350B3C");

            entity.Property(e => e.fecha_pago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_metodoNavigation).WithMany(p => p.Pagos_Venta)
                .HasForeignKey(d => d.id_metodo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PagosVentas_Metodos");

            entity.HasOne(d => d.id_ventaNavigation).WithMany(p => p.Pagos_Venta)
                .HasForeignKey(d => d.id_venta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PagosVentas_Ventas");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.id_permiso).HasName("PK__Permisos__228F224F361A24F3");

            entity.Property(e => e.accion)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.id_rolNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.id_rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permisos_Roles");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.id_producto).HasName("PK__Producto__FF341C0DAEE42915");

            entity.Property(e => e.nombre_producto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.precio_venta).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_categoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.id_categoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Categorias");

            entity.HasOne(d => d.id_marcaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.id_marca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Marcas");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.id_proveedor).HasName("PK__Proveedo__8D3DFE287089B27C");

            entity.Property(e => e.direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.nombre_proveedor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Roles_Usuario>(entity =>
        {
            entity.HasKey(e => e.id_rol).HasName("PK__Roles_Us__6ABCB5E0695ACA53");

            entity.Property(e => e.nombre_rol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("PK__Usuarios__4E3E04ADCBA27014");

            entity.Property(e => e.contrasena)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.nombre_usuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.id_licoreriaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.id_licoreria)
                .HasConstraintName("FK_Usuarios_Licoreria");

            entity.HasOne(d => d.id_rolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.id_rol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.id_venta).HasName("PK__Ventas__459533BFE998FCE4");

            entity.Property(e => e.fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.metodo_pago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Efectivo");
            entity.Property(e => e.total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.id_clienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.id_cliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Clientes");

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.id_usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
