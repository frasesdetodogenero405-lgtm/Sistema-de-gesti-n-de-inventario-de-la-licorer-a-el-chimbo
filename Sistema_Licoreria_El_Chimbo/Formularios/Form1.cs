using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sistema_Licoreria_El_Chimbo
{
    public partial class Form1 : Form
    {
        private ProveedorNegocio proveedorNegocio = new ProveedorNegocio();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un proveedor.");
                return;
            }

            try
            {
                Proveedor proveedor = new Proveedor();

                proveedor.IdProveedor = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                proveedor.NombreProveedor = textBox1.Text;
                proveedor.Telefono = textBox2.Text;
                proveedor.Direccion = textBox3.Text;

                bool resultado = proveedorNegocio.Editar(proveedor);

                if (resultado)
                {
                    MessageBox.Show("Proveedor actualizado correctamente.");

                    dataGridView1.DataSource = proveedorNegocio.Listar();

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el proveedor.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar proveedor: " + ex.Message);
            }
        }
    




    private void btnListarProveedores_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = proveedorNegocio.Listar();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Proveedor proveedor = new Proveedor();

            proveedor.NombreProveedor = textBox1.Text;
            proveedor.Telefono = textBox2.Text;
            proveedor.Direccion = textBox3.Text;

            proveedorNegocio.Registrar(proveedor);

            MessageBox.Show("Proveedor registrado correctamente.");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["nombre_proveedor"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["telefono"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["direccion"].Value.ToString();
            }
    }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null ||
            dataGridView1.CurrentRow.Cells[0].Value == null ||
            dataGridView1.CurrentRow.Cells[0].Value == DBNull.Value)
            {
                MessageBox.Show("Seleccione un proveedor válido.");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un proveedor completo de la tabla.");
                return;
            }

            int idProveedor = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar este proveedor?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    bool eliminado = proveedorNegocio.Eliminar(idProveedor);

                    if (eliminado)
                    {
                        MessageBox.Show("Proveedor eliminado correctamente.");

                        dataGridView1.DataSource = proveedorNegocio.Listar();

                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el proveedor.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar el proveedor. " + ex.Message);
                }
            }
        }
    }
    }
