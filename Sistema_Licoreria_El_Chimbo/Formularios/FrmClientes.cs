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

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmClientes : Form
    {
        private ClienteNegocio objetoNegocio = new ClienteNegocio();
        private int idClienteSeleccionado = 0;
        public FrmClientes()
        {
            InitializeComponent();
            ListarClientes();
            var puedeRegistrar = PermisosSesion.Tiene("Registrar clientes", "Gestionar clientes");
            var puedeGestionar = PermisosSesion.Tiene("Gestionar clientes");
            btnRegistrar.Visible = puedeRegistrar;
            btnEditar.Visible = puedeGestionar;
            btnEliminar.Visible = puedeGestionar;
            btnLimpiar.Visible = puedeRegistrar;
            txtNombre.ReadOnly = !puedeRegistrar;
            txtTelefono.ReadOnly = !puedeRegistrar;
            txtDireccion.ReadOnly = !puedeRegistrar;
        }
        private void ListarClientes()
        {
            dgvClientes.DataSource = objetoNegocio.Listar();
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void lblNombre_Click(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!PermisosSesion.Tiene("Registrar clientes", "Gestionar clientes"))
            {
                MessageBox.Show("Tu rol no tiene permiso para registrar clientes.");
                return;
            }
            Cliente cliente = new Cliente();

            cliente.NombreCliente = txtNombre.Text;
            cliente.Telefono = txtTelefono.Text;
            cliente.Direccion = txtDireccion.Text;

            if (objetoNegocio.Registrar(cliente))
            {
                MessageBox.Show("Cliente registrado correctamente.");

                txtNombre.Clear();
                txtTelefono.Clear();
                txtDireccion.Clear();

                ListarClientes();
            }
            else
            {
                MessageBox.Show("No se pudo registrar el cliente.");
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idClienteSeleccionado = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells["IdCliente"].Value);

                txtNombre.Text = dgvClientes.Rows[e.RowIndex].Cells["NombreCliente"].Value.ToString();
                txtTelefono.Text = dgvClientes.Rows[e.RowIndex].Cells["Telefono"].Value.ToString();
                txtDireccion.Text = dgvClientes.Rows[e.RowIndex].Cells["Direccion"].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!PermisosSesion.Tiene("Gestionar clientes"))
            {
                MessageBox.Show("Tu rol no tiene permiso para editar clientes.");
                return;
            }
            if (idClienteSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un cliente para editar.");
                return;
            }

            Cliente cliente = new Cliente();

            cliente.IdCliente = idClienteSeleccionado;
            cliente.NombreCliente = txtNombre.Text;
            cliente.Telefono = txtTelefono.Text;
            cliente.Direccion = txtDireccion.Text;

            if (objetoNegocio.Editar(cliente))
            {
                MessageBox.Show("Cliente actualizado correctamente.");

                txtNombre.Clear();
                txtTelefono.Clear();
                txtDireccion.Clear();

                idClienteSeleccionado = 0;

                ListarClientes();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el cliente.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!PermisosSesion.Tiene("Gestionar clientes"))
            {
                MessageBox.Show("Tu rol no tiene permiso para eliminar clientes.");
                return;
            }
            if (idClienteSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un cliente para eliminar.");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                Cliente cliente = new Cliente();
                cliente.IdCliente = idClienteSeleccionado;

                if (objetoNegocio.Eliminar(cliente))
                {
                    MessageBox.Show("Cliente eliminado correctamente.");

                    txtNombre.Clear();
                    txtTelefono.Clear();
                    txtDireccion.Clear();

                    idClienteSeleccionado = 0;

                    ListarClientes();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el cliente.");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();

            dgvClientes.ClearSelection();

            txtNombre.Focus();
        }
    }
}
