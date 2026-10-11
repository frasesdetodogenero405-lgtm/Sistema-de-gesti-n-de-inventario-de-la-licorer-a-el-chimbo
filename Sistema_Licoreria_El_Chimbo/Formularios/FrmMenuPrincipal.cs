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
    public partial class FrmMenuPrincipal : Form
    {
        public FrmMenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            FormProductos formulario = new FormProductos();
            formulario.ShowDialog();
        }

        private void btnMarcas_Click(object sender, EventArgs e)
        {
            FrmMarcas formulario = new FrmMarcas();
            formulario.ShowDialog();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
                Form1 formulario = new Form1();
                formulario.ShowDialog();
            }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            FrmClientes formulario = new FrmClientes();
            formulario.ShowDialog();
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            FrmInventario formulario = new FrmInventario();
            formulario.ShowDialog();
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            FrmCompras formulario = new FrmCompras();
            formulario.ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            FrmVentas formulario = new FrmVentas();
            formulario.ShowDialog();
        }

        private void btnMetodosPago_Click(object sender, EventArgs e)
        {
            FrmMetodosPago formulario = new FrmMetodosPago();
            formulario.ShowDialog();
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            btnProductos.Visible = PermisosSesion.Tiene("Gestionar productos", "consultar productos");
            btnMarcas.Visible = PermisosSesion.Tiene("Gestionar marcas");
            btnProveedores.Visible = PermisosSesion.Tiene("Gestionar proveedores");
            btnClientes.Visible = PermisosSesion.Tiene("Consultar clientes", "Registrar clientes", "Gestionar clientes");
            btnInventario.Visible = PermisosSesion.Tiene("Consultar inventario", "Gestionar inventario", "Ajustar stock", "Consultar movimientos de stock");
            btnCompras.Visible = PermisosSesion.Tiene("Gestionar compras");
            btnVentas.Visible = PermisosSesion.Tiene("Gestionar ventas", "Registrar ventas");
            btnMetodosPago.Visible = PermisosSesion.Tiene("Gestionar métodos de pago");
            btnRoles.Visible = PermisosSesion.Tiene("Gestionar roles");
            btnUsuarios.Visible = PermisosSesion.Tiene("Gestionar usuarios");
            btnPermisos.Visible = PermisosSesion.Tiene("Gestionar permisos");
            btnCategorias_productos.Visible = PermisosSesion.Tiene("Gestionar categorías");
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            FrmRoles formulario = new FrmRoles();
            formulario.ShowDialog();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            FrmUsuarios frm = new FrmUsuarios();
            frm.ShowDialog();
        }

        private void btnPermisos_Click(object sender, EventArgs e)
        {

            Permisos frm = new Permisos();
            frm.ShowDialog();
        }

        private void btnCategorias_productos_Click(object sender, EventArgs e)
        {

            FrmCategorias_productos formulario = new FrmCategorias_productos();
            formulario.ShowDialog();
        }
    }
    }

