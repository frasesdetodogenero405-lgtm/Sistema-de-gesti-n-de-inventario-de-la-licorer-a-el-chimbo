
using System;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmCategorias_productos : Form
    {
        private CategoriaProductoNegocio negocio = new CategoriaProductoNegocio();
        private int idCategoriaSeleccionada = 0;

        public FrmCategorias_productos()
        {
            InitializeComponent();

            btnRegistrar.Click += btnRegistrar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvCategorias.CellClick += dgvCategorias_CellClick;

            CargarCategorias();
        }

        private void CargarCategorias()
        {
            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = negocio.Listar();

            if (dgvCategorias.Columns["IdCategoria"] != null)
                dgvCategorias.Columns["IdCategoria"].HeaderText = "ID";

            if (dgvCategorias.Columns["NombreCategoria"] != null)
                dgvCategorias.Columns["NombreCategoria"].HeaderText = "Nombre de categoría";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.");
                return;
            }

            Categorias_productos categoria = new Categorias_productos();
            categoria.NombreCategoria = txtNombreCategoria.Text.Trim();

            negocio.Registrar(categoria);

            MessageBox.Show("Categoría registrada correctamente.");
            Limpiar();
            CargarCategorias();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idCategoriaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MessageBox.Show("Ingrese el nombre de la categoría.");
                return;
            }

            Categorias_productos categoria = new Categorias_productos();
            categoria.IdCategoria = idCategoriaSeleccionada;
            categoria.NombreCategoria = txtNombreCategoria.Text.Trim();

            negocio.Editar(categoria);

            MessageBox.Show("Categoría actualizada.");
            Limpiar();
            CargarCategorias();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idCategoriaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }

            if (MessageBox.Show("¿Desea eliminar esta categoría?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                negocio.Eliminar(idCategoriaSeleccionada);

                MessageBox.Show("Categoría eliminada.");
                Limpiar();
                CargarCategorias();
            }
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idCategoriaSeleccionada = Convert.ToInt32(
                    dgvCategorias.Rows[e.RowIndex].Cells["IdCategoria"].Value);

                txtNombreCategoria.Text = Convert.ToString(
                    dgvCategorias.Rows[e.RowIndex].Cells["NombreCategoria"].Value);
            }
        }

        private void Limpiar()
        {
            idCategoriaSeleccionada = 0;
            txtNombreCategoria.Clear();
        }
    }
}