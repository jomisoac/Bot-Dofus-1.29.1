namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Oficios
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
            this.dataGridView_skills = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_minima = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_maxima = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tiempo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_oficios = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Experiencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.porcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_skills)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_oficios)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_skills
            // 
            this.dataGridView_skills.AllowUserToAddRows = false;
            this.dataGridView_skills.AllowUserToDeleteRows = false;
            this.dataGridView_skills.AllowUserToOrderColumns = true;
            this.dataGridView_skills.AllowUserToResizeColumns = false;
            this.dataGridView_skills.AllowUserToResizeRows = false;
            this.dataGridView_skills.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_skills.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_skills.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.cantidad_minima,
            this.cantidad_maxima,
            this.tiempo});
            this.dataGridView_skills.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_skills.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_skills.Location = new System.Drawing.Point(0, 261);
            this.dataGridView_skills.MultiSelect = false;
            this.dataGridView_skills.Name = "dataGridView_skills";
            this.dataGridView_skills.ReadOnly = true;
            this.dataGridView_skills.RowHeadersVisible = false;
            this.dataGridView_skills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_skills.Size = new System.Drawing.Size(790, 239);
            this.dataGridView_skills.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 180F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Nom";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 180;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 180;
            // 
            // cantidad_minima
            // 
            this.cantidad_minima.HeaderText = "Quantité minimal";
            this.cantidad_minima.MinimumWidth = 130;
            this.cantidad_minima.Name = "cantidad_minima";
            this.cantidad_minima.ReadOnly = true;
            this.cantidad_minima.Width = 130;
            // 
            // cantidad_maxima
            // 
            this.cantidad_maxima.HeaderText = "Quantité maximal";
            this.cantidad_maxima.MinimumWidth = 135;
            this.cantidad_maxima.Name = "cantidad_maxima";
            this.cantidad_maxima.ReadOnly = true;
            this.cantidad_maxima.Width = 135;
            // 
            // tiempo
            // 
            this.tiempo.HeaderText = "Temps/Pourcentage";
            this.tiempo.MinimumWidth = 130;
            this.tiempo.Name = "tiempo";
            this.tiempo.ReadOnly = true;
            this.tiempo.Width = 130;
            // 
            // dataGridView_oficios
            // 
            this.dataGridView_oficios.AllowUserToAddRows = false;
            this.dataGridView_oficios.AllowUserToDeleteRows = false;
            this.dataGridView_oficios.AllowUserToOrderColumns = true;
            this.dataGridView_oficios.AllowUserToResizeColumns = false;
            this.dataGridView_oficios.AllowUserToResizeRows = false;
            this.dataGridView_oficios.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_oficios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_oficios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Nombre,
            this.Nivel,
            this.Experiencia,
            this.porcentaje});
            this.dataGridView_oficios.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_oficios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_oficios.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_oficios.Name = "dataGridView_oficios";
            this.dataGridView_oficios.ReadOnly = true;
            this.dataGridView_oficios.RowHeadersVisible = false;
            this.dataGridView_oficios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_oficios.Size = new System.Drawing.Size(790, 261);
            this.dataGridView_oficios.TabIndex = 4;
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // Nombre
            // 
            this.Nombre.FillWeight = 200F;
            this.Nombre.HeaderText = "Nom";
            this.Nombre.MinimumWidth = 200;
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.Width = 200;
            // 
            // Nivel
            // 
            this.Nivel.HeaderText = "Nivel";
            this.Nivel.Name = "Nivel";
            this.Nivel.ReadOnly = true;
            // 
            // Experiencia
            // 
            this.Experiencia.FillWeight = 200F;
            this.Experiencia.HeaderText = "Expérience";
            this.Experiencia.MinimumWidth = 200;
            this.Experiencia.Name = "Experiencia";
            this.Experiencia.ReadOnly = true;
            this.Experiencia.Width = 200;
            // 
            // porcentaje
            // 
            this.porcentaje.HeaderText = "Pourcentage";
            this.porcentaje.Name = "porcentaje";
            this.porcentaje.ReadOnly = true;
            // 
            // UI_Oficios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView_oficios);
            this.Controls.Add(this.dataGridView_skills);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UI_Oficios";
            this.Size = new System.Drawing.Size(790, 500);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_skills)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_oficios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_skills;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_minima;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_maxima;
        private System.Windows.Forms.DataGridViewTextBoxColumn tiempo;
        private System.Windows.Forms.DataGridView dataGridView_oficios;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nivel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Experiencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn porcentaje;
    }
}
