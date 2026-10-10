using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmCompras : Form
    {
        private ComprasNegocio comprasNegocio = new ComprasNegocio();

        public FrmCompras()
        {
            InitializeComponent();
            CargarProveedores();
            CargarProductos();
        }
        private void CargarProductos()
        {
            ProductoNegocio negocio = new ProductoNegocio();

            cmbProducto.DataSource = negocio.Listar();
            cmbProducto.DisplayMember = "NombreProducto";
            cmbProducto.ValueMember = "IdProducto";
        }
        private void CargarProveedores()
        {
            ProveedorNegocio negocio = new ProveedorNegocio();

            DataTable tabla = negocio.Listar();

            cmbProveedor.DataSource = tabla;
            cmbProveedor.DisplayMember = "nombre_proveedor";
            cmbProveedor.ValueMember = "id_proveedor";
        }

        private void lblFecha_Click(object sender, EventArgs e)
        {

        }

        private void lblPrecioCompra_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalCompra_Click(object sender, EventArgs e)
        {

        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            if (dgvDetalleCompra.Rows.Cast<DataGridViewRow>().Count(fila => !fila.IsNewRow) == 0)
            {
                MessageBox.Show("Agregue al menos un producto a la compra.");
                return;
            }

            List<Detalle_Compras> detalles = new List<Detalle_Compras>();

            foreach (DataGridViewRow fila in dgvDetalleCompra.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                int cantidad;
                decimal precioCompra;
                decimal precioVenta;
                if (fila.Tag == null ||
                    !int.TryParse(Convert.ToString(fila.Cells[1].Value), out cantidad) ||
                    !decimal.TryParse(Convert.ToString(fila.Cells[2].Value), out precioCompra) ||
                    !decimal.TryParse(Convert.ToString(fila.Cells[3].Value), out precioVenta))
                {
                    MessageBox.Show("Revise los productos, cantidades y precios de la compra.");
                    return;
                }

                detalles.Add(new Detalle_Compras
                {
                    IdProducto = Convert.ToInt32(fila.Tag),
                    Cantidad = cantidad,
                    PrecioCompra = precioCompra,
                    PrecioVenta = precioVenta
                });
            }

            Compras compra = new Compras
            {
                IdProveedor = Convert.ToInt32(cmbProveedor.SelectedValue),
                IdUsuario = FrmLogin.IdUsuarioActual,
                Fecha = dtpFecha.Value
            };

            string mensaje;
            if (!comprasNegocio.Registrar(compra, detalles, out mensaje))
            {
                MessageBox.Show("No se pudo guardar la compra.\n" + mensaje);
                return;
            }

            MessageBox.Show("Compra guardada correctamente. Número de compra: " + compra.IdCompra);
            dgvDetalleCompra.Rows.Clear();
            txtTotal.Text = "0.00";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCantidad.Text))
            {
                MessageBox.Show("Ingrese la cantidad.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                MessageBox.Show("Ingrese el precio de compra.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a 0.");
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MessageBox.Show("Ingrese un precio de compra válido mayor a 0.");
                return;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta) || precioVenta <= 0)
            {
                MessageBox.Show("Ingrese un precio de venta válido mayor a 0.");
                return;
            }


            decimal subtotal = cantidad * precioCompra;

            foreach (DataGridViewRow filaExistente in dgvDetalleCompra.Rows)
            {
                if (filaExistente.IsNewRow)
                    continue;

                if (filaExistente.Tag != null &&
                    Convert.ToInt32(filaExistente.Tag) == Convert.ToInt32(cmbProducto.SelectedValue))
                {
                    MessageBox.Show("Este producto ya fue agregado a la compra.");
                    return;
                }
            }
            string producto = cmbProducto.Text;
            int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);

            int fila = dgvDetalleCompra.Rows.Add(
                producto,
                cantidad,
                precioCompra,
                precioVenta,
                subtotal
            );

            dgvDetalleCompra.Rows[fila].Tag = idProducto;

            decimal total = 0;

            foreach (DataGridViewRow filaActual in dgvDetalleCompra.Rows)
            {
                if (filaActual.IsNewRow)
                    continue;

                if (filaActual.Cells["colSubtotal"].Value != null)
                {
                    total += Convert.ToDecimal(filaActual.Cells["colSubtotal"].Value);
                }
            }

            txtTotal.Text = total.ToString("0.00");

            txtCantidad.Clear();
            txtPrecioCompra.Clear();
            cmbProducto.SelectedIndex = -1;
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is Producto producto)
            {
                txtPrecioVenta.Text = producto.PrecioVenta.ToString("0.00");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
           
                dgvDetalleCompra.Rows.Clear();

                txtCantidad.Clear();
                txtPrecioCompra.Clear();
                txtPrecioVenta.Clear();

                txtTotal.Text = "0.00";

                cmbProveedor.SelectedIndex = -1;
                cmbProducto.SelectedIndex = -1;
                dtpFecha.Value = DateTime.Now;
            }
        }
    }
