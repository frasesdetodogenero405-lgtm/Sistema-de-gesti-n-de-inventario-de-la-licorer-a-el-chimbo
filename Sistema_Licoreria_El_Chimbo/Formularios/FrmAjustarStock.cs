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

            int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);
            string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();

            InventarioNegocio negocioInventario = new InventarioNegocio();
            List<Inventario> lista = negocioInventario.Listar();

            Inventario inventario = lista.Find(x => x.IdProducto == idProducto);

            if (inventario == null)
            {
                MessageBox.Show("El producto no tiene un registro en inventario.");
                return;
            }

            if (tipoMovimiento == "Salida")
            {
                if (cantidad > inventario.StockActual)
                {
                    MessageBox.Show("No puede retirar una cantidad mayor al stock actual.");
                    return;
                }

                inventario.StockActual -= cantidad;
            }
            else
            {
                inventario.StockActual += cantidad;
            }

            bool actualizado = negocioInventario.Editar(inventario);

            if (!actualizado)
            {
                MessageBox.Show("No se pudo actualizar el stock.");
                return;
            }

            Movimientos_Stock movimiento = new Movimientos_Stock();

            movimiento.IdProducto = idProducto;
            movimiento.TipoMovimiento = tipoMovimiento;
            movimiento.Cantidad = cantidad;
            movimiento.Fecha = DateTime.Now;
            movimiento.Referencia = txtMotivo.Text;

            MovimientosStockNegocio negocioMovimiento = new MovimientosStockNegocio();

            bool registrado = negocioMovimiento.Registrar(movimiento);

            if (registrado)
            {
                MessageBox.Show("Stock ajustado correctamente.");
                txtStockActual.Text = inventario.StockActual.ToString();
                txtCantidad.Clear();
            }
            else
            {
                MessageBox.Show("El stock se actualizó, pero no se pudo registrar el movimiento.");
            }
        }
    }
}