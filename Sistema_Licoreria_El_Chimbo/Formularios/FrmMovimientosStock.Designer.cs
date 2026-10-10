namespace Sistema_Licoreria_El_Chimbo.Formularios
{
    partial class FrmMovimientosStock
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
            this.dgvMovimientosStock = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnActualizar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientosStock)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMovimientosStock
            // 
            this.dgvMovimientosStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientosStock.Location = new System.Drawing.Point(44, 155);
            this.dgvMovimientosStock.Name = "dgvMovimientosStock";
            this.dgvMovimientosStock.Size = new System.Drawing.Size(727, 388);
            this.dgvMovimientosStock.TabIndex = 0;
            this.dgvMovimientosStock.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMovimientosStock_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(214, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "HISTORIAL DE STOCK ACTUAL";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(75, 105);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(109, 23);
            this.btnActualizar.TabIndex = 2;
            this.btnActualizar.Text = "Actualizar Historial";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // FrmMovimientosStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 555);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMovimientosStock);
            this.Name = "FrmMovimientosStock";
            this.Text = "Movimientos Stock";
            this.Load += new System.EventHandler(this.FrmMovimientosStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientosStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMovimientosStock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnActualizar;
    }
}