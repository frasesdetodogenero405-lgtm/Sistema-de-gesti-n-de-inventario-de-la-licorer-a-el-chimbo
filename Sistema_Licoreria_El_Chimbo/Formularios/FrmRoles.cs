using System;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo
{
    public partial class FrmRoles : Form
    {
        private RolesNegocio objetoNegocio = new RolesNegocio();
        private int idRolSeleccionado = 0;

        public FrmRoles()
        {
            InitializeComponent();

            Load += FrmRoles_Load;
            btnRegistrar.Click += btnRegistrar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvRoles.CellClick += dgvRoles_CellClick;
        }

        private void FrmRoles_Load(object sender, EventArgs e)
        {
            ListarRoles();
        }

        private void ListarRoles()
        {
            dgvRoles.DataSource = null;
            dgvRoles.DataSource = objetoNegocio.Listar();

            dgvRoles.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.MultiSelect = false;
            dgvRoles.ReadOnly = true;
            dgvRoles.AllowUserToAddRows = false;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreRol.Text))
            {
                MessageBox.Show("Ingrese el nombre del rol.");
                return;
            }

            try
            {
                Roles rol = new Roles();
                rol.NombreRol = txtNombreRol.Text.Trim();

                if (objetoNegocio.Registrar(rol))
                {
                    MessageBox.Show("Rol registrado correctamente.");
                    ListarRoles();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el rol: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idRolSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un rol de la tabla.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreRol.Text))
            {
                MessageBox.Show("Ingrese el nombre del rol.");
                return;
            }

            try
            {
                Roles rol = new Roles();
                rol.IdRol = idRolSeleccionado;
                rol.NombreRol = txtNombreRol.Text.Trim();

                if (objetoNegocio.Editar(rol))
                {
                    MessageBox.Show("Rol actualizado correctamente.");
                    ListarRoles();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el rol: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idRolSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un rol de la tabla.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este rol?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                if (objetoNegocio.Eliminar(idRolSeleccionado))
                {
                    MessageBox.Show("Rol eliminado correctamente.");
                    ListarRoles();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo eliminar el rol. Puede estar relacionado " +
                    "con usuarios o permisos.\n" + ex.Message);
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            idRolSeleccionado = Convert.ToInt32(
                dgvRoles.Rows[e.RowIndex].Cells["IdRol"].Value);

            txtNombreRol.Text = Convert.ToString(
                dgvRoles.Rows[e.RowIndex].Cells["NombreRol"].Value);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtNombreRol.Clear();
            idRolSeleccionado = 0;
            dgvRoles.ClearSelection();
            txtNombreRol.Focus();
        }
    }
}
