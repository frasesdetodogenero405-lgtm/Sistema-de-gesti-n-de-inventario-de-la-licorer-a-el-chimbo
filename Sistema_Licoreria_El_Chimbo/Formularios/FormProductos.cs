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
using Sistema_Licoreria_El_Chimbo.Formularios;

namespace Sistema_Licoreria_El_Chimbo
{
    public partial class FormProductos : Form
    {
        ProductoNegocio negocio = new ProductoNegocio();
        public FormProductos()
        {
            InitializeComponent();
            CargarCategorias();
            CargarMarcas();
            ListarProductos();
            var puedeGestionar = PermisosSesion.Tiene("Gestionar productos");
            button1.Visible = puedeGestionar;
            btnEditar.Visible = puedeGestionar;
            btnEliminar.Visible = puedeGestionar;
            btnLimpiar.Visible = puedeGestionar;
            txtNombreProducto.ReadOnly = !puedeGestionar;
            txtPrecioVenta.ReadOnly = !puedeGestionar;
            cmbCategoria.Enabled = puedeGestionar;
            cmbMarca.Enabled = puedeGestionar;
        }
        private void CargarCategorias()
        {
            CategoriaProductoNegocio negocioCategoria = new CategoriaProductoNegocio();

            cmbCategoria.DataSource = negocioCategoria.Listar();
            cmbCategoria.DisplayMember = "NombreCategoria";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.SelectedIndex = -1;
        }
        private void CargarMarcas()
        {
            MarcaNegocio negocioMarca = new MarcaNegocio();

            cmbMarca.DataSource = negocioMarca.Listar();
            cmbMarca.DisplayMember = "NombreMarca";
            cmbMarca.ValueMember = "IdMarca";
            cmbMarca.SelectedIndex = -1;
        }
        private void ListarProductos()
        {
            dgvProductos.DataSource = negocio.Listar();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!PermisosSesion.Tiene("Gestionar productos"))
            {
                MessageBox.Show("Tu rol no tiene permiso para registrar productos.");
                return;
            }
            Producto producto = new Producto();

            producto.NombreProducto = txtNombreProducto.Text;
            decimal precio;

            if (!decimal.TryParse(txtPrecioVenta.Text, out precio))
            {
                MessageBox.Show("Ingrese un precio válido.");
                return;
            }

            producto.PrecioVenta = precio;
            producto.IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
            producto.IdMarca = Convert.ToInt32(cmbMarca.SelectedValue);

            ProductoNegocio negocio = new ProductoNegocio();

            bool respuesta = negocio.Registrar(producto);

            if (respuesta)
            {
                MessageBox.Show("Producto registrado correctamente");

                txtNombreProducto.Clear();
                txtPrecioVenta.Clear();
                cmbCategoria.SelectedIndex = -1;
                cmbMarca.SelectedIndex = -1;

                ListarProductos();

            }
            else
            {
                MessageBox.Show("No se pudo registrar el producto");
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
  
        {
            if (!PermisosSesion.Tiene("Gestionar productos"))
            {
                MessageBox.Show("Tu rol no tiene permiso para eliminar productos.");
                return;
            }
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            int idProducto = Convert.ToInt32(
             dgvProductos.CurrentRow.Cells["IdProducto"].Value

            );

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de eliminar este producto?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                bool respuesta = negocio.Eliminar(idProducto);

                if (respuesta)
                {
                    MessageBox.Show("Producto eliminado correctamente.");
                    ListarProductos();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto.");
                }
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtNombreProducto.Text = fila.Cells["NombreProducto"].Value.ToString();
                txtPrecioVenta.Text = fila.Cells["PrecioVenta"].Value.ToString();
                cmbCategoria.SelectedValue = fila.Cells["IdCategoria"].Value;
                cmbMarca.SelectedValue = fila.Cells["IdMarca"].Value;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!PermisosSesion.Tiene("Gestionar productos"))
            {
                MessageBox.Show("Tu rol no tiene permiso para editar productos.");
                return;
            }
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un producto para editar.");
                return;
            }

            int idProducto = Convert.ToInt32(
                dgvProductos.CurrentRow.Cells["IdProducto"].Value
            );

            Producto producto = new Producto();

            producto.IdProducto = idProducto;
            producto.NombreProducto = txtNombreProducto.Text;

            decimal precio;

            if (!decimal.TryParse(txtPrecioVenta.Text, out precio))
            {
                MessageBox.Show("Ingrese un precio válido.");
                return;
            }

            producto.PrecioVenta = precio;
            producto.IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
            producto.IdMarca = Convert.ToInt32(cmbMarca.SelectedValue);

            bool respuesta = negocio.Editar(producto);

            if (respuesta)
            {
                MessageBox.Show("Producto actualizado correctamente.");

                txtNombreProducto.Clear();
                txtPrecioVenta.Clear();
                cmbCategoria.SelectedIndex = -1;
                cmbMarca.SelectedIndex = -1;

                ListarProductos();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el producto.");
            }
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombreProducto.Clear();
            txtPrecioVenta.Clear();

            cmbCategoria.SelectedIndex = -1;
            cmbMarca.SelectedIndex = -1;

            dgvProductos.ClearSelection();
            txtNombreProducto.Focus();
        }
    }
}
