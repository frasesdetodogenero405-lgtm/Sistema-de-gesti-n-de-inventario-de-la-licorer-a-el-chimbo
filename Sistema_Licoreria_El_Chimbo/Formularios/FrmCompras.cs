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
        private ProductoNegocio productoNegocio = new ProductoNegocio();

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

            Compras compra = new Compras();

            compra.IdProveedor = Convert.ToInt32(cmbProveedor.SelectedValue);
            compra.IdUsuario = 2;
            compra.Fecha = dtpFecha.Value;
            compra.Total = Convert.ToDecimal(txtTotal.Text);

            bool respuesta = comprasNegocio.Registrar(compra);

            if (!respuesta)
            {
                MessageBox.Show("No se pudo guardar la compra.");
                return;
            }

            DetalleComprasNegocio detalleNegocio = new DetalleComprasNegocio();

            bool detallesCorrectos = true;
            List<Producto> productos = productoNegocio.Listar();

            foreach (DataGridViewRow fila in dgvDetalleCompra.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                if (fila.Tag != null)
                {
                    Detalle_Compras detalle = new Detalle_Compras();

                    detalle.IdCompra = compra.IdCompra;
                    detalle.IdProducto = Convert.ToInt32(fila.Tag);
                    detalle.Cantidad = Convert.ToInt32(fila.Cells[1].Value);
                    detalle.PrecioCompra = Convert.ToDecimal(fila.Cells[2].Value);

                    bool detalleRegistrado = detalleNegocio.Registrar(detalle);

                    if (!detalleRegistrado)
                    {
                        detallesCorrectos = false;
                        break;
                    }
                }
            }

            if (detallesCorrectos)
            {
                foreach (DataGridViewRow fila in dgvDetalleCompra.Rows)
                {
                    if (fila.IsNewRow)
                        continue;

                    if (fila.Tag != null)
                    {
                        int idProducto = Convert.ToInt32(fila.Tag);
                        decimal precioVenta = Convert.ToDecimal(fila.Cells[3].Value);

                        Producto producto = productos.Find(x => x.IdProducto == idProducto);

                        if (producto != null)
                        {
                            producto.PrecioVenta = precioVenta;
                            productoNegocio.Editar(producto);
                        }
                    }
                }
                MessageBox.Show("Compra guardada correctamente y el stock fue actualizado.");

                dgvDetalleCompra.Rows.Clear();
                txtTotal.Text = "0.00";
            }
            else
            {
                MessageBox.Show("La compra se registró, pero hubo un problema al registrar el detalle.");
            }
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