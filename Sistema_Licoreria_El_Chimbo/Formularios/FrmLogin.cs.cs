
using System;
using System.Linq;
using System.Windows.Forms;
using CapaNegocio;

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmLogin : Form
    {
        private UsuariosNegocio usuariosNegocio = new UsuariosNegocio();

        public static int IdRolActual { get; private set; }
        public static int IdUsuarioActual { get; private set; }
        public static string NombreUsuarioActual { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();

            txtContrasena.UseSystemPasswordChar = true;
            btnIngresar.Click += btnIngresar_Click;
            btnSalir.Click += btnSalir_Click;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Ingresa el usuario y la contraseña.");
                return;
            }

            try
            {
                var usuario = usuariosNegocio.Listar().FirstOrDefault(u =>
                    u.NombreUsuario == txtUsuario.Text.Trim() &&
                    u.Contrasena == txtContrasena.Text);

                if (usuario == null)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                    return;
                }

                IdUsuarioActual = usuario.IdUsuario;
                IdRolActual = usuario.IdRol;
                NombreUsuarioActual = usuario.NombreUsuario;

                MessageBox.Show("Bienvenido, " + usuario.NombreUsuario);

                FrmMenuPrincipal menu = new FrmMenuPrincipal();

                menu.FormClosed += (s, args) => this.Close();

                menu.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo iniciar sesión: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
