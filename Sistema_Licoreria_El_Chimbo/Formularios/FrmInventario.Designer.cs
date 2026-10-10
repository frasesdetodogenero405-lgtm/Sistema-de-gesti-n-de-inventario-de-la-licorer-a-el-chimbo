namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    partial class FrmInventario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblProducto = new System.Windows.Forms.Label();
            this.lblStockMinimo = new System.Windows.Forms.Label();
            this.lblStockMaximo = new System.Windows.Forms.Label();
            this.lblStockActual = new System.Windows.Forms.Label();
            this.txtStockActual = new System.Windows.Forms.TextBox();
            this.txtStockMinimo = new System.Windows.Forms.TextBox();
            this.txtStockMaximo = new System.Windows.Forms.TextBox();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvInventario = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnAjustarStock = new System.Windows.Forms.Button();
            this.btnHistorialStock = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(221, 60);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(50, 13);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "Producto";
            // 
            // lblStockMinimo
            // 
            this.lblStockMinimo.AutoSize = true;
            this.lblStockMinimo.Location = new System.Drawing.Point(224, 97);
            this.lblStockMinimo.Name = "lblStockMinimo";
            this.lblStockMinimo.Size = new System.Drawing.Size(72, 13);
            this.lblStockMinimo.TabIndex = 1;
            this.lblStockMinimo.Text = "Stock mínimo";
            // 
            // lblStockMaximo
            // 
            this.lblStockMaximo.AutoSize = true;
            this.lblStockMaximo.Location = new System.Drawing.Point(227, 134);
            this.lblStockMaximo.Name = "lblStockMaximo";
            this.lblStockMaximo.Size = new System.Drawing.Size(73, 13);
            this.lblStockMaximo.TabIndex = 2;
            this.lblStockMaximo.Text = "Stock máximo";
            // 
            // lblStockActual
            // 
            this.lblStockActual.AutoSize = true;
            this.lblStockActual.Location = new System.Drawing.Point(230, 172);
            this.lblStockActual.Name = "lblStockActual";
            this.lblStockActual.Size = new System.Drawing.Size(67, 13);
            this.lblStockActual.TabIndex = 3;
            this.lblStockActual.Text = "Stock actual";
            // 
            // txtStockActual
            // 
            this.txtStockActual.Location = new System.Drawing.Point(357, 169);
            this.txtStockActual.Name = "txtStockActual";
            this.txtStockActual.Size = new System.Drawing.Size(100, 20);
            this.txtStockActual.TabIndex = 4;
            // 
            // txtStockMinimo
            // 
            this.txtStockMinimo.Location = new System.Drawing.Point(357, 94);
            this.txtStockMinimo.Name = "txtStockMinimo";
            this.txtStockMinimo.Size = new System.Drawing.Size(100, 20);
            this.txtStockMinimo.TabIndex = 5;
            // 
            // txtStockMaximo
            // 
            this.txtStockMaximo.Location = new System.Drawing.Point(357, 134);
            this.txtStockMaximo.Name = "txtStockMaximo";
            this.txtStockMaximo.Size = new System.Drawing.Size(100, 20);
            this.txtStockMaximo.TabIndex = 6;
            // 
            // cmbProducto
            // 
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(357, 52);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(121, 21);
            this.cmbProducto.TabIndex = 7;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(99, 230);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(209, 230);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(103, 23);
            this.btnEditar.TabIndex = 9;
            this.btnEditar.Text = "Editar inventario";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(344, 230);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 10;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvInventario
            // 
            this.dgvInventario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventario.Location = new System.Drawing.Point(55, 273);
            this.dgvInventario.Name = "dgvInventario";
            this.dgvInventario.Size = new System.Drawing.Size(720, 266);
            this.dgvInventario.TabIndex = 11;
            this.dgvInventario.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInventario_CellClick);
            this.dgvInventario.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInventario_CellContentClick);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(339, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(148, 25);
            this.lblTitulo.TabIndex = 12;
            this.lblTitulo.Text = "INVENTARIO";
            // 
            // btnAjustarStock
            // 
            this.btnAjustarStock.Location = new System.Drawing.Point(450, 230);
            this.btnAjustarStock.Name = "btnAjustarStock";
            this.btnAjustarStock.Size = new System.Drawing.Size(81, 23);
            this.btnAjustarStock.TabIndex = 13;
            this.btnAjustarStock.Text = "Ajustar stock";
            this.btnAjustarStock.UseVisualStyleBackColor = true;
            this.btnAjustarStock.Click += new System.EventHandler(this.btnAjustarStock_Click);
            // 
            // btnHistorialStock
            // 
            this.btnHistorialStock.Location = new System.Drawing.Point(550, 230);
            this.btnHistorialStock.Name = "btnHistorialStock";
            this.btnHistorialStock.Size = new System.Drawing.Size(101, 23);
            this.btnHistorialStock.TabIndex = 14;
            this.btnHistorialStock.Text = "Historial de Stock";
            this.btnHistorialStock.UseVisualStyleBackColor = true;
            this.btnHistorialStock.Click += new System.EventHandler(this.btnHistorialStock_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(668, 230);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiar.TabIndex = 15;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // FrmInventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 551);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnHistorialStock);
            this.Controls.Add(this.btnAjustarStock);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.dgvInventario);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.cmbProducto);
            this.Controls.Add(this.txtStockMaximo);
            this.Controls.Add(this.txtStockMinimo);
            this.Controls.Add(this.txtStockActual);
            this.Controls.Add(this.lblStockActual);
            this.Controls.Add(this.lblStockMaximo);
            this.Controls.Add(this.lblStockMinimo);
            this.Controls.Add(this.lblProducto);
            this.Name = "FrmInventario";
            this.Text = "Inventario";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label lblStockMinimo;
        private System.Windows.Forms.Label lblStockMaximo;
        private System.Windows.Forms.Label lblStockActual;
        private System.Windows.Forms.TextBox txtStockActual;
        private System.Windows.Forms.TextBox txtStockMinimo;
        private System.Windows.Forms.TextBox txtStockMaximo;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvInventario;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnAjustarStock;
        private System.Windows.Forms.Button btnHistorialStock;
        private System.Windows.Forms.Button btnLimpiar;
    }
}