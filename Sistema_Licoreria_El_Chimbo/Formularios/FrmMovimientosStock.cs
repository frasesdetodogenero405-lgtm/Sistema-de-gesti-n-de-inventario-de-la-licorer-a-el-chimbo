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

namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    public partial class FrmMovimientosStock : Form
    {
        public FrmMovimientosStock()
        {
            InitializeComponent();

            dgvMovimientosStock.ReadOnly = true;
            dgvMovimientosStock.AllowUserToAddRows = false;
            dgvMovimientosStock.AllowUserToDeleteRows = false;
        }

        private void FrmMovimientosStock_Load(object sender, EventArgs e)
        {
            MovimientosStockNegocio negocio = new MovimientosStockNegocio();

            dgvMovimientosStock.DataSource = negocio.Listar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            MovimientosStockNegocio negocio = new MovimientosStockNegocio();

            dgvMovimientosStock.DataSource = negocio.Listar();
        }

        private void dgvMovimientosStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
