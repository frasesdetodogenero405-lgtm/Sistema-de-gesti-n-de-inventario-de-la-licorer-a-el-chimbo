using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Producto
{
    public int id_producto { get; set; }

    public string nombre_producto { get; set; } = null!;

    public decimal precio_venta { get; set; }

    public int id_categoria { get; set; }

    public int id_marca { get; set; }

    public virtual ICollection<Detalle_Compra> Detalle_Compras { get; set; } = new List<Detalle_Compra>();

    public virtual ICollection<Detalle_Venta> Detalle_Venta { get; set; } = new List<Detalle_Venta>();

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Movimientos_Stock> Movimientos_Stocks { get; set; } = new List<Movimientos_Stock>();

    public virtual Categorias_Producto id_categoriaNavigation { get; set; } = null!;

    public virtual Marca id_marcaNavigation { get; set; } = null!;
}
