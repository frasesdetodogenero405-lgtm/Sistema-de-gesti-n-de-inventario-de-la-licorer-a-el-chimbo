namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    partial class FrmMetodosPago
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
            this.lblMetodoPago = new System.Windows.Forms.Label();
            this.txtNombreMetodo = new System.Windows.Forms.TextBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvMetodosPago = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetodosPago)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMetodoPago
            // 
            this.lblMetodoPago.AutoSize = true;
            this.lblMetodoPago.Location = new System.Drawing.Point(218, 91);
            this.lblMetodoPago.Name = "lblMetodoPago";
            this.lblMetodoPago.Size = new System.Drawing.Size(86, 13);
            this.lblMetodoPago.TabIndex = 0;
            this.lblMetodoPago.Text = "Método de Pago";
            // 
            // txtNombreMetodo
            // 
            this.txtNombreMetodo.Location = new System.Drawing.Point(343, 84);
            this.txtNombreMetodo.Name = "txtNombreMetodo";
            this.txtNombreMetodo.Size = new System.Drawing.Size(100, 20);
            this.txtNombreMetodo.TabIndex = 1;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(221, 130);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(75, 23);
            this.btnRegistrar.TabIndex = 2;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(343, 130);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 23);
            this.btnEditar.TabIndex = 3;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(463, 130);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvMetodosPago
            // 
            this.dgvMetodosPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetodosPago.Location = new System.Drawing.Point(282, 196);
            this.dgvMetodosPago.Name = "dgvMetodosPago";
            this.dgvMetodosPago.Size = new System.Drawing.Size(243, 112);
            this.dgvMetodosPago.TabIndex = 5;
            this.dgvMetodosPago.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMetodosPago_CellClick);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(217, 36);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(342, 24);
            this.lblTitulo.TabIndex = 6;
            this.lblTitulo.Text = "GESTIÓN DE MÉTODOS DE PAGO";
            // 
            // FrmMetodosPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.dgvMetodosPago);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.txtNombreMetodo);
            this.Controls.Add(this.lblMetodoPago);
            this.Name = "FrmMetodosPago";
            this.Text = "Metodos de Pago";
            this.Load += new System.EventHandler(this.FrmMetodosPago_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetodosPago)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMetodoPago;
        private System.Windows.Forms.TextBox txtNombreMetodo;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvMetodosPago;
        private System.Windows.Forms.Label lblTitulo;
    }
}