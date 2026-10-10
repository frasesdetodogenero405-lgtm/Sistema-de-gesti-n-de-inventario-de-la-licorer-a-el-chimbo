
using System;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class Permisos : Form
    {
        private PermisosNegocio permisosNegocio = new PermisosNegocio();
        private RolesNegocio rolesNegocio = new RolesNegocio();
        private int idPermisoSeleccionado = 0;

        public Permisos()
        {
            InitializeComponent();

            dgvPermisos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPermisos.MultiSelect = false;
            dgvPermisos.ReadOnly = true;
            dgvPermisos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            CargarRoles();
            CargarPermisos();

            btnRegistrar.Click += btnRegistrar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            dgvPermisos.CellClick += dgvPermisos_CellClick;
        }

        private void CargarRoles()
        {
            cmbRol.DataSource = rolesNegocio.Listar();
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "IdRol";
            cmbRol.SelectedIndex = -1;
        }

        private void CargarPermisos()
        {
            dgvPermisos.DataSource = null;
            dgvPermisos.DataSource = permisosNegocio.Listar();

            if (dgvPermisos.Columns["IdPermiso"] != null)
                dgvPermisos.Columns["IdPermiso"].HeaderText = "ID Permiso";

            if (dgvPermisos.Columns["IdRol"] != null)
                dgvPermisos.Columns["IdRol"].HeaderText = "ID Rol";

            if (dgvPermisos.Columns["Accion"] != null)
                dgvPermisos.Columns["Accion"].HeaderText = "Permiso";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbRol.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtAccion.Text))
            {
                MessageBox.Show("Selecciona un rol e ingresa el permiso.");
                return;
            }

            CapaEntidad.Permisos permiso = new CapaEntidad.Permisos
            {
                IdRol = Convert.ToInt32(cmbRol.SelectedValue),
                Accion = txtAccion.Text.Trim()
            };

            if (permisosNegocio.Registrar(permiso))
            {
                MessageBox.Show("Permiso registrado correctamente.");
                CargarPermisos();
                Limpiar();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idPermisoSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un permiso de la tabla.");
                return;
            }

            if (cmbRol.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtAccion.Text))
            {
                MessageBox.Show("Selecciona un rol e ingresa el permiso.");
                return;
            }
            CapaEntidad.Permisos permiso = new CapaEntidad.Permisos
            {
                IdPermiso = idPermisoSeleccionado,
                IdRol = Convert.ToInt32(cmbRol.SelectedValue),
                Accion = txtAccion.Text.Trim()
            };

            if (permisosNegocio.Editar(permiso))
            {
                MessageBox.Show("Permiso actualizado correctamente.");
                CargarPermisos();
                Limpiar();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPermisoSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un permiso de la tabla.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas eliminar este permiso?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                if (permisosNegocio.Eliminar(idPermisoSeleccionado))
                {
                    MessageBox.Show("Permiso eliminado correctamente.");
                    CargarPermisos();
                    Limpiar();
                }
            }
        }

        private void dgvPermisos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow fila = dgvPermisos.Rows[e.RowIndex];

            idPermisoSeleccionado =
                Convert.ToInt32(fila.Cells["IdPermiso"].Value);

            txtAccion.Text = fila.Cells["Accion"].Value.ToString();
            cmbRol.SelectedValue =
                Convert.ToInt32(fila.Cells["IdRol"].Value);
        }

        private void Limpiar()
        {
            idPermisoSeleccionado = 0;
            cmbRol.SelectedIndex = -1;
            txtAccion.Clear();
            dgvPermisos.ClearSelection();
        }
    }
}