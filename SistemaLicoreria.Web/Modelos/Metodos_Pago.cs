using System;
using System.Collections.Generic;

namespace SistemaLicoreria.Web.Modelos;

public partial class Metodos_Pago
{
    public int id_metodo { get; set; }

    public string nombre_metodo { get; set; } = null!;

    public virtual ICollection<Pagos_Compra> Pagos_Compras { get; set; } = new List<Pagos_Compra>();

    public virtual ICollection<Pagos_Venta> Pagos_Venta { get; set; } = new List<Pagos_Venta>();
}
