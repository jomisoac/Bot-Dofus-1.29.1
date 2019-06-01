namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Hechizos
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView_hechizos = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_hechizos)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_hechizos
            // 
            this.dataGridView_hechizos.AllowUserToAddRows = false;
            this.dataGridView_hechizos.AllowUserToDeleteRows = false;
            this.dataGridView_hechizos.AllowUserToOrderColumns = true;
            this.dataGridView_hechizos.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_hechizos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_hechizos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Nombre,
            this.Nivel,
            this.Subir});
            this.dataGridView_hechizos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_hechizos.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_hechizos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView_hechizos.MultiSelect = false;
            this.dataGridView_hechizos.Name = "dataGridView_hechizos";
            this.dataGridView_hechizos.ReadOnly = true;
            this.dataGridView_hechizos.RowHeadersVisible = false;
            this.dataGridView_hechizos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_hechizos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_hechizos.Size = new System.Drawing.Size(790, 500);
            this.dataGridView_hechizos.TabIndex = 0;
            // 
            // Id
            // 
            this.Id.HeaderText = "id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // Nombre
            // 
            this.Nombre.HeaderText = "nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            // 
            // Nivel
            // 
            this.Nivel.HeaderText = "nivel";
            this.Nivel.Name = "Nivel";
            this.Nivel.ReadOnly = true;
            // 
            // Subir
            // 
            this.Subir.HeaderText = "subir";
            this.Subir.Name = "Subir";
            this.Subir.ReadOnly = true;
            // 
            // UI_Hechizos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView_hechizos);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UI_Hechizos";
            this.Size = new System.Drawing.Size(790, 500);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_hechizos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_hechizos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nivel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subir;
    }
}
