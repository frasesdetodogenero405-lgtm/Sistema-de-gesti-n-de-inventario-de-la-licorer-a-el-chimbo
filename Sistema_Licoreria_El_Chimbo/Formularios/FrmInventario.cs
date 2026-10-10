using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmInventario : Form
    {
        int stockMinimoOriginal;
        int stockMaximoOriginal;

        private InventarioNegocio objetoNegocio =
            new InventarioNegocio();

        public FrmInventario()
        {
            InitializeComponent();

            CargarProductos();
            ListarInventario();
        }

        private void CargarProductos()
        {
            ProductoNegocio productoNegocio =
                new ProductoNegocio();

            cmbProducto.DataSource =
                productoNegocio.Listar();

            cmbProducto.DisplayMember =
                "NombreProducto";

            cmbProducto.ValueMember =
                "IdProducto";
        }

        private void ListarInventario()
        {
            dgvInventario.DataSource = null;

            List<Inventario> lista =
                objetoNegocio.Listar();

            dgvInventario.DataSource = lista;
        }

        private void dgvInventario_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cmbProducto.SelectedValue =
                    dgvInventario.Rows[e.RowIndex]
                    .Cells["IdProducto"].Value;

                txtStockMinimo.Text =
                    dgvInventario.Rows[e.RowIndex]
                    .Cells["StockMinimo"].Value.ToString();

                txtStockMaximo.Text =
                    dgvInventario.Rows[e.RowIndex]
                    .Cells["StockMaximo"].Value.ToString();

                txtStockActual.Text =
                    dgvInventario.Rows[e.RowIndex]
                    .Cells["StockActual"].Value.ToString();

                stockMinimoOriginal =
                    Convert.ToInt32(
                        dgvInventario.Rows[e.RowIndex]
                        .Cells["StockMinimo"].Value);

                stockMaximoOriginal =
                    Convert.ToInt32(
                        dgvInventario.Rows[e.RowIndex]
                        .Cells["StockMaximo"].Value);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show(
                    "Seleccione un producto.");
                return;
            }

            if (!int.TryParse(
                txtStockMinimo.Text,
                out int stockMinimo))
            {
                MessageBox.Show(
                    "Ingrese un stock mínimo válido.");
                return;
            }

            if (!int.TryParse(
                txtStockMaximo.Text,
                out int stockMaximo))
            {
                MessageBox.Show(
                    "Ingrese un stock máximo válido.");
                return;
            }

            if (!int.TryParse(
                txtStockActual.Text,
                out int stockActual))
            {
                MessageBox.Show(
                    "Ingrese un stock actual válido.");
                return;
            }

            if (stockMinimo < 0 ||
                stockMaximo < 0 ||
                stockActual < 0)
            {
                MessageBox.Show(
                    "Los valores de stock no pueden ser negativos.");
                return;
            }

            if (stockMaximo < stockMinimo)
            {
                MessageBox.Show(
                    "El stock máximo no puede ser menor " +
                    "que el stock mínimo.");
                return;
            }

            int idProducto =
                Convert.ToInt32(
                    cmbProducto.SelectedValue);

            List<Inventario> listaInventario =
                objetoNegocio.Listar();

            Inventario existente =
                listaInventario.Find(
                    x => x.IdProducto == idProducto);

            if (existente != null)
            {
                MessageBox.Show(
                    "Este producto ya tiene un registro " +
                    "en inventario. Utilice Editar Inventario " +
                    "para modificar el stock mínimo o máximo.");
                return;
            }

            Inventario inventario =
                new Inventario();

            inventario.IdProducto =
                idProducto;

            inventario.StockMinimo =
                stockMinimo;

            inventario.StockMaximo =
                stockMaximo;

            inventario.StockActual =
                stockActual;

            bool respuesta =
                objetoNegocio.Registrar(
                    inventario);

            if (respuesta)
            {
                MessageBox.Show(
                    "Inventario registrado correctamente.");

                ListarInventario();

                txtStockMinimo.Clear();
                txtStockMaximo.Clear();
                txtStockActual.Clear();
            }
            else
            {
                MessageBox.Show(
                    "No se pudo registrar el inventario.");
            }
        }

        private void dgvInventario_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
        }

        private void btnEditar_Click(
            object sender,
            EventArgs e)
        {
            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione un registro.");
                return;
            }

            try
            {
                int idInventario =
                    Convert.ToInt32(
                        dgvInventario.CurrentRow
                        .Cells["IdInventario"].Value);

                int idProducto =
                    Convert.ToInt32(
                        cmbProducto.SelectedValue);

                int stockMinimoAnterior =
                    Convert.ToInt32(
                        dgvInventario.CurrentRow
                        .Cells["StockMinimo"].Value);

                int stockMaximoAnterior =
                    Convert.ToInt32(
                        dgvInventario.CurrentRow
                        .Cells["StockMaximo"].Value);

                int stockActualOriginal =
                    Convert.ToInt32(
                        dgvInventario.CurrentRow
                        .Cells["StockActual"].Value);

                if (!int.TryParse(
                    txtStockMinimo.Text,
                    out int stockMinimoIngresado))
                {
                    MessageBox.Show(
                        "Ingrese un stock mínimo válido.");
                    return;
                }

                if (!int.TryParse(
                    txtStockMaximo.Text,
                    out int stockMaximoIngresado))
                {
                    MessageBox.Show(
                        "Ingrese un stock máximo válido.");
                    return;
                }

                if (!int.TryParse(
                    txtStockActual.Text,
                    out int stockActualIngresado))
                {
                    MessageBox.Show(
                        "Ingrese un stock actual válido.");
                    return;
                }

                // El stock actual no se modifica con Editar
                if (stockActualIngresado != stockActualOriginal)
                {
                    MessageBox.Show(
                        "El stock actual no se puede modificar " +
                        "con el botón Editar. Utilice Ajustar Stock.");
                    return;
                }

                if (stockMinimoIngresado < 0 ||
                    stockMaximoIngresado < 0)
                {
                    MessageBox.Show(
                        "El stock mínimo y máximo no pueden ser negativos.");
                    return;
                }

                if (stockMaximoIngresado <
                    stockMinimoIngresado)
                {
                    MessageBox.Show(
                        "El stock máximo no puede ser menor " +
                        "que el stock mínimo.");
                    return;
                }

                if (stockMinimoIngresado ==
                        stockMinimoAnterior &&
                    stockMaximoIngresado ==
                        stockMaximoAnterior)
                {
                    MessageBox.Show(
                        "No se realizaron cambios " +
                        "en el inventario.");
                    return;
                }

                Inventario inventario =
                    new Inventario();

                inventario.IdInventario =
                    idInventario;

                inventario.IdProducto =
                    idProducto;

                inventario.StockMinimo =
                    stockMinimoIngresado;

                inventario.StockMaximo =
                    stockMaximoIngresado;

                inventario.StockActual =
                    stockActualOriginal;

                bool respuesta =
                    objetoNegocio.Editar(
                        inventario);

                if (respuesta)
                {
                    MessageBox.Show(
                        "Inventario actualizado correctamente.");

                    // Volver a cargar incluyendo Estado
                    ListarInventario();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo actualizar el inventario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error: " +
                    ex.Message);
            }
        }

        private void btnEliminar_Click(
            object sender,
            EventArgs e)
        {
            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione un registro para eliminar.");
                return;
            }

            int idInventario =
                Convert.ToInt32(
                    dgvInventario.CurrentRow
                    .Cells["IdInventario"].Value);

            DialogResult resultado =
                MessageBox.Show(
                    "¿Está seguro de eliminar este registro?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                InventarioNegocio negocio =
                    new InventarioNegocio();

                if (negocio.Eliminar(idInventario))
                {
                    MessageBox.Show(
                        "Registro eliminado correctamente.");

                    ListarInventario();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo eliminar el registro.");
                }
            }
        }

        private void btnAjustarStock_Click(
            object sender,
            EventArgs e)
        {
            FrmAjustarStock formulario =
                new FrmAjustarStock();

            formulario.ShowDialog();

            // Recargar inventario y recalcular Estado
            ListarInventario();
        }

        private void btnHistorialStock_Click(
            object sender,
            EventArgs e)
        {
            FrmMovimientosStock formulario =
                new FrmMovimientosStock();

            formulario.ShowDialog();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbProducto.SelectedIndex = -1;
            txtStockMinimo.Clear();
            txtStockMaximo.Clear();
            txtStockActual.Clear();
           

            dgvInventario.ClearSelection();
            dgvInventario.CurrentCell = null;
        }
    }
}