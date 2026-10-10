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
    public partial class FrmMetodosPago : Form
    {
        private MetodosPagoNegocio objMetodosPagoNegocio = new MetodosPagoNegocio();
        private int idMetodoPago = 0;
        private void Listar()
        {
            dgvMetodosPago.DataSource = null;
            dgvMetodosPago.DataSource = objMetodosPagoNegocio.Listar();
        }

        public FrmMetodosPago()
        {
            InitializeComponent();
        }

        private void FrmMetodosPago_Load(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreMetodo.Text))
            {
                MessageBox.Show("Ingrese el método de pago.");
                return;
            }

            Metodos_Pago metodo = new Metodos_Pago
            {
                NombreMetodo = txtNombreMetodo.Text.Trim()
            };

            if (objMetodosPagoNegocio.Registrar(metodo))
            {
                MessageBox.Show("Método de pago registrado correctamente.");

                txtNombreMetodo.Clear();

                Listar();
            }
            else
            {
                MessageBox.Show("No se pudo registrar el método de pago.");
            }
        }

        private void dgvMetodosPago_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idMetodoPago = Convert.ToInt32(
                    dgvMetodosPago.Rows[e.RowIndex].Cells["IdMetodoPago"].Value
                );

                txtNombreMetodo.Text =
                    dgvMetodosPago.Rows[e.RowIndex].Cells["NombreMetodo"].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idMetodoPago == 0)
            {
                MessageBox.Show("Seleccione un método de pago.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreMetodo.Text))
            {
                MessageBox.Show("Ingrese el método de pago.");
                return;
            }

            Metodos_Pago metodo = new Metodos_Pago
            {
                IdMetodoPago = idMetodoPago,
                NombreMetodo = txtNombreMetodo.Text.Trim()
            };

            if (objMetodosPagoNegocio.Editar(metodo))
            {
                MessageBox.Show("Método de pago actualizado correctamente.");

                txtNombreMetodo.Clear();
                idMetodoPago = 0;

                Listar();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el método de pago.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idMetodoPago == 0)
            {
                MessageBox.Show("Seleccione un método de pago.");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de eliminar este método de pago?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                if (objMetodosPagoNegocio.Eliminar(idMetodoPago))
                {
                    MessageBox.Show("Método de pago eliminado correctamente.");

                    txtNombreMetodo.Clear();
                    idMetodoPago = 0;

                    Listar();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el método de pago.");
                }
            }
        }
    }
}