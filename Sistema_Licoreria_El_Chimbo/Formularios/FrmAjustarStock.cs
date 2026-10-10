using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmAjustarStock : Form
    {
        public FrmAjustarStock()
        {
            InitializeComponent();
        }

        private void FrmAjustarStock_Load(object sender, EventArgs e)
        {
            cmbTipoMovimiento.Items.Add("Entrada");
            cmbTipoMovimiento.Items.Add("Salida");
            ProductoNegocio negocioProducto = new ProductoNegocio();
            cmbProducto.DataSource = negocioProducto.Listar();
            cmbProducto.DisplayMember = "NombreProducto";
            cmbProducto.ValueMember = "IdProducto";
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
                return;

            if (!int.TryParse(cmbProducto.SelectedValue.ToString(), out int idProducto))
                return;

            InventarioNegocio negocioInventario = new InventarioNegocio();
            List<Inventario> lista = negocioInventario.Listar();

            Inventario inventario = lista.Find(x => x.IdProducto == idProducto);

            if (inventario != null)
            {
                txtStockActual.Text = inventario.StockActual.ToString();
            }
            else
            {
                txtStockActual.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }

            if (cmbTipoMovimiento.SelectedItem == null)
            {
                MessageBox.Show("Seleccione el tipo de movimiento.");
                return;
            }

            int cantidad;

            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida.");
                return;
            }

            if (txtMotivo.Text.Length > 100)
            {
                MessageBox.Show("El motivo no puede superar 100 caracteres.");
                return;
            }

            int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);
            string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();

            MovimientosStockNegocio negocioMovimiento = new MovimientosStockNegocio();
            int stockActualizado;
            bool registrado;
            try
            {
                registrado = negocioMovimiento.AjustarStock(
                    idProducto, tipoMovimiento, cantidad, txtMotivo.Text, out stockActualizado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo ajustar el stock: " + ex.Message);
                return;
            }

            if (registrado)
            {
                MessageBox.Show("Stock ajustado correctamente.");
                txtStockActual.Text = stockActualizado.ToString();
                txtCantidad.Clear();
            }
            else
            {
                MessageBox.Show("No se pudo ajustar el stock. Verifique que exista inventario para el producto y que haya stock suficiente para una salida.");
            }
        }
    }
}
