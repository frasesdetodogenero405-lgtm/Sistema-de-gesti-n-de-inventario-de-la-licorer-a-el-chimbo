using CapaEntidad;
using CapaNegocio;
using System;
using System.Windows.Forms;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmMarcas : Form
    {
        private MarcaNegocio negocio = new MarcaNegocio();

        public FrmMarcas()
        {
            InitializeComponent();
        }

        private void FrmMarcas_Load(object sender, EventArgs e)
        {
            ListarMarcas();
        }

        private void ListarMarcas()
        {
            dgvMarcas.DataSource = null;
            dgvMarcas.DataSource = negocio.Listar();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MessageBox.Show("Ingrese el nombre de la marca.");
                return;
            }

            Marca marca = new Marca
            {
                NombreMarca = txtNombreMarca.Text.Trim()
            };

            if (negocio.Registrar(marca))
            {
                MessageBox.Show("Marca registrada correctamente.");
                txtNombreMarca.Clear();
                ListarMarcas();
            }
            else
            {
                MessageBox.Show("No se pudo registrar la marca.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MessageBox.Show("Ingrese el nombre de la marca.");
                return;
            }

            int idMarca = Convert.ToInt32(
                dgvMarcas.CurrentRow.Cells["IdMarca"].Value
            );

            Marca marca = new Marca
            {
                IdMarca = idMarca,
                NombreMarca = txtNombreMarca.Text.Trim()
            };

            if (negocio.Editar(marca))
            {
                MessageBox.Show("Marca editada correctamente.");
                txtNombreMarca.Clear();
                ListarMarcas();
            }
            else
            {
                MessageBox.Show("No se pudo editar la marca.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una marca.");
                return;
            }

            int idMarca = Convert.ToInt32(
                dgvMarcas.CurrentRow.Cells["IdMarca"].Value
            );

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de eliminar esta marca?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                if (negocio.Eliminar(idMarca))
                {
                    MessageBox.Show("Marca eliminada correctamente.");
                    txtNombreMarca.Clear();
                    ListarMarcas();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar la marca.");
                }
            }
        }

        private void dgvMarcas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreMarca.Text =
                    dgvMarcas.Rows[e.RowIndex].Cells["NombreMarca"].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            txtNombreMarca.Clear();
            dgvMarcas.ClearSelection();
            txtNombreMarca.Focus();
        }
    }
}