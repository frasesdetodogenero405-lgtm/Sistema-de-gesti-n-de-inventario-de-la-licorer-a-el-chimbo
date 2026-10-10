
using System;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo
{
    public partial class FrmUsuarios : Form
    {
        private UsuariosNegocio objetoNegocio = new UsuariosNegocio();
        private RolesNegocio objetoRoles = new RolesNegocio();
        private int idUsuarioSeleccionado = 0;

        public FrmUsuarios()
        {
            InitializeComponent();

            Load += FrmUsuarios_Load;
            btnRegistrar.Click += btnRegistrar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            txtContrasena.UseSystemPasswordChar = true;

            cmbRol.DataSource = objetoRoles.Listar();
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "IdRol";
            cmbRol.SelectedIndex = -1;

            ListarUsuarios();
        }

        private void ListarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = objetoNegocio.Listar();

            dgvUsuarios.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;

            if (dgvUsuarios.Columns["Contrasena"] != null)
                dgvUsuarios.Columns["Contrasena"].Visible = false;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Text) ||
                cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            try
            {
                Usuarios usuario = new Usuarios();
                usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                usuario.Contrasena = txtContrasena.Text;
                usuario.IdRol = Convert.ToInt32(cmbRol.SelectedValue);

                if (objetoNegocio.Registrar(usuario))
                {
                    MessageBox.Show("Usuario registrado correctamente.");
                    ListarUsuarios();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un usuario de la tabla.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Text) ||
                cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            try
            {
                Usuarios usuario = new Usuarios();
                usuario.IdUsuario = idUsuarioSeleccionado;
                usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                usuario.Contrasena = txtContrasena.Text;
                usuario.IdRol = Convert.ToInt32(cmbRol.SelectedValue);

                if (objetoNegocio.Editar(usuario))
                {
                    MessageBox.Show("Usuario actualizado correctamente.");
                    ListarUsuarios();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un usuario de la tabla.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                if (objetoNegocio.Eliminar(idUsuarioSeleccionado))
                {
                    MessageBox.Show("Usuario eliminado correctamente.");
                    ListarUsuarios();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar: " + ex.Message);
            }
        }

        private void dgvUsuarios_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            idUsuarioSeleccionado = Convert.ToInt32(
                dgvUsuarios.Rows[e.RowIndex].Cells["IdUsuario"].Value);

            txtNombreUsuario.Text = Convert.ToString(
                dgvUsuarios.Rows[e.RowIndex].Cells["NombreUsuario"].Value);

            txtContrasena.Clear();

            object idRol = dgvUsuarios.Rows[e.RowIndex].Cells["IdRol"].Value;
            cmbRol.SelectedValue = idRol;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtNombreUsuario.Clear();
            txtContrasena.Clear();
            cmbRol.SelectedIndex = -1;
            idUsuarioSeleccionado = 0;
            dgvUsuarios.ClearSelection();
            txtNombreUsuario.Focus();
        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {

        }
    }
}
