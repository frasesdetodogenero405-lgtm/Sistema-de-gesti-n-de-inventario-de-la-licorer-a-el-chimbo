
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmVentas : Form
    {
        private VentaNegocio objVentaNegocio = new VentaNegocio();
        private InventarioNegocio objInventarioNegocio = new InventarioNegocio();
        private ClienteNegocio objClienteNegocio = new ClienteNegocio();
        private ProductoNegocio objProductoNegocio = new ProductoNegocio();
        private MetodosPagoNegocio objMetodosPagoNegocio = new MetodosPagoNegocio();

        private List<Detalle_Ventas> listaDetalle = new List<Detalle_Ventas>();
        public FrmVentas()
        {
            InitializeComponent();
            CargarDatos();
        }
        private void CargarDatos()
        {
            cmbCliente.DataSource = objClienteNegocio.Listar();
            cmbCliente.DisplayMember = "NombreCliente";
            cmbCliente.ValueMember = "IdCliente";

            cmbProducto.DataSource = objProductoNegocio.Listar();
            cmbProducto.DisplayMember = "NombreProducto";
            cmbProducto.ValueMember = "IdProducto";

            cmbMetodoPago.DataSource = objMetodosPagoNegocio.Listar();
            cmbMetodoPago.DisplayMember = "NombreMetodo";
            cmbMetodoPago.ValueMember = "IdMetodoPago";
        }
        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void lblCliente_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precioVenta) || precioVenta <= 0)
            {
                MessageBox.Show("Ingrese un precio de venta válido.");
                return;
            }

            foreach (Detalle_Ventas detalleExistente in listaDetalle)
            {
                if (detalleExistente.IdProducto == Convert.ToInt32(cmbProducto.SelectedValue))
                {
                    MessageBox.Show("Este producto ya fue agregado a la venta.");
                    return;
                }
            }


            int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);


            Detalle_Ventas detalle = new Detalle_Ventas
            {
                IdProducto = idProducto,
                Cantidad = cantidad,
                PrecioVenta = precioVenta
            };

            listaDetalle.Add(detalle);

            dgvDetalleVenta.Rows.Add(
                cmbProducto.Text,
                cantidad,
                precioVenta,
                cantidad * precioVenta
            );

            CalcularTotal();

            txtCantidad.Clear();
        }
        private void CalcularTotal()
        {
            decimal total = 0;

            foreach (Detalle_Ventas detalle in listaDetalle)
            {
                total += detalle.Cantidad * detalle.PrecioVenta;
            }

            lblTotal.Text = total.ToString("0.00");
        }
        private void FrmVentas_Load(object sender, EventArgs e)
        {

        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is Producto producto)
            {
                txtPrecio.Text = producto.PrecioVenta.ToString("0.00");
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            if (listaDetalle.Count == 0)
            {
                MessageBox.Show("Agregue al menos un producto a la venta.");
                return;
            }

            if (cmbMetodoPago.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un método de pago.");
                return;
            }

            // Verificar que todos los productos tengan stock suficiente
            List<Inventario> inventarioActual = objInventarioNegocio.Listar();

            foreach (Detalle_Ventas detalle in listaDetalle)
            {
                Inventario inventario = inventarioActual.Find(
                    x => x.IdProducto == detalle.IdProducto
                );

                if (inventario == null)
                {
                    MessageBox.Show(
                        "El producto seleccionado no tiene un registro de inventario."
                    );
                    return;
                }

                if (detalle.Cantidad > inventario.StockActual)
                {
                    MessageBox.Show(
                        "STOCK INSUFICIENTE\n\n" +
                        "Producto: " + inventario.NombreProducto + "\n" +
                        "Stock disponible: " + inventario.StockActual + "\n" +
                        "Cantidad solicitada: " + detalle.Cantidad,
                        "Stock insuficiente",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
            }

            decimal total = 0;

            foreach (Detalle_Ventas detalle in listaDetalle)
            {
                total += detalle.Cantidad * detalle.PrecioVenta;
            }

            Ventas venta = new Ventas
            {
                IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                IdUsuario = FrmLogin.IdUsuarioActual,
                Fecha = DateTime.Now,
                Total = total,
                MetodoPago = cmbMetodoPago.Text
            };

            string mensaje;

            int idVenta = objVentaNegocio.Registrar(
                venta,
                listaDetalle,
                Convert.ToInt32(cmbMetodoPago.SelectedValue),
                out mensaje);

            if (idVenta == 0)
            {
                MessageBox.Show("No se pudo registrar la venta.\n" + mensaje);
                return;
            }

            MessageBox.Show(
                "Venta registrada correctamente.\nNúmero de venta: " + idVenta
            );

            listaDetalle.Clear();
            dgvDetalleVenta.Rows.Clear();

            txtCantidad.Clear();
            txtPrecio.Clear();

            lblTotal.Text = "0.00";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            listaDetalle.Clear();
            dgvDetalleVenta.Rows.Clear();

            txtCantidad.Clear();
            txtPrecio.Clear();

            lblTotal.Text = "0.00";
        }

        private void cmbMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbCliente.SelectedIndex = -1;
            cmbProducto.SelectedIndex = -1;
            cmbMetodoPago.SelectedIndex = -1;

            txtCantidad.Clear();
            txtPrecio.Clear();

            listaDetalle.Clear();
            dgvDetalleVenta.DataSource = null;

            lblTotal.Text = "0.00";

            cmbCliente.Focus();
        }
    }

}


