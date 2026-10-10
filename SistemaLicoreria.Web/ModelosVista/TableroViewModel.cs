namespace SistemaLicoreria.Web.ModelosVista;

public class TableroViewModel
{
    public int CantidadProductos { get; set; }
    public int ProductosConStockBajo { get; set; }
    public int CantidadClientes { get; set; }
    public decimal VentasDelMes { get; set; }
    public decimal ComprasDelMes { get; set; }
    public string[] Meses { get; set; } = Array.Empty<string>();
    public decimal[] VentasMensuales { get; set; } = Array.Empty<decimal>();
    public decimal[] ComprasMensuales { get; set; } = Array.Empty<decimal>();
    public List<VentaRecienteViewModel> VentasRecientes { get; set; } = new();
    public string? ErrorConexion { get; set; }
}

public class VentaRecienteViewModel
{
    public int Numero { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
}
