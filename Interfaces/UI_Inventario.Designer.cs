namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Inventario
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Inventario));
            this.tabControl_mision = new System.Windows.Forms.TabControl();
            this.tabPage_equipamiento = new System.Windows.Forms.TabPage();
            this.dataGridView_equipamientos = new System.Windows.Forms.DataGridView();
            this.ID_Inventario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID_Modelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Posicion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Accion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Eliminar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_varios = new System.Windows.Forms.TabPage();
            this.tabPage_recursos = new System.Windows.Forms.TabPage();
            this.dataGridView_recursos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_mision = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl_mision.SuspendLayout();
            this.tabPage_equipamiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_equipamientos)).BeginInit();
            this.tabPage_recursos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_recursos)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_mision
            // 
            this.tabControl_mision.Controls.Add(this.tabPage_equipamiento);
            this.tabControl_mision.Controls.Add(this.tabPage_varios);
            this.tabControl_mision.Controls.Add(this.tabPage_recursos);
            this.tabControl_mision.Controls.Add(this.tabPage_mision);
            this.tabControl_mision.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl_mision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_mision.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tabControl_mision.ImageList = this.imageList1;
            this.tabControl_mision.ItemSize = new System.Drawing.Size(67, 26);
            this.tabControl_mision.Location = new System.Drawing.Point(0, 0);
            this.tabControl_mision.Name = "tabControl_mision";
            this.tabControl_mision.SelectedIndex = 0;
            this.tabControl_mision.Size = new System.Drawing.Size(790, 500);
            this.tabControl_mision.TabIndex = 0;
            // 
            // tabPage_equipamiento
            // 
            this.tabPage_equipamiento.Controls.Add(this.dataGridView_equipamientos);
            this.tabPage_equipamiento.ImageIndex = 0;
            this.tabPage_equipamiento.Location = new System.Drawing.Point(4, 30);
            this.tabPage_equipamiento.Name = "tabPage_equipamiento";
            this.tabPage_equipamiento.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_equipamiento.Size = new System.Drawing.Size(782, 466);
            this.tabPage_equipamiento.TabIndex = 0;
            this.tabPage_equipamiento.Text = "Equipamiento";
            this.tabPage_equipamiento.UseVisualStyleBackColor = true;
            // 
            // dataGridView_equipamientos
            // 
            this.dataGridView_equipamientos.AllowUserToAddRows = false;
            this.dataGridView_equipamientos.AllowUserToDeleteRows = false;
            this.dataGridView_equipamientos.AllowUserToOrderColumns = true;
            this.dataGridView_equipamientos.AllowUserToResizeColumns = false;
            this.dataGridView_equipamientos.AllowUserToResizeRows = false;
            this.dataGridView_equipamientos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_equipamientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_equipamientos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_Inventario,
            this.ID_Modelo,
            this.Nombre,
            this.Cantidad,
            this.Posicion,
            this.Accion,
            this.Eliminar});
            this.dataGridView_equipamientos.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_equipamientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_equipamientos.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_equipamientos.Name = "dataGridView_equipamientos";
            this.dataGridView_equipamientos.ReadOnly = true;
            this.dataGridView_equipamientos.RowHeadersVisible = false;
            this.dataGridView_equipamientos.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_equipamientos.TabIndex = 0;
            this.dataGridView_equipamientos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_equipamientos_CellContentClick);
            // 
            // ID_Inventario
            // 
            this.ID_Inventario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ID_Inventario.HeaderText = "ID Inventario";
            this.ID_Inventario.MinimumWidth = 110;
            this.ID_Inventario.Name = "ID_Inventario";
            this.ID_Inventario.ReadOnly = true;
            this.ID_Inventario.Width = 110;
            // 
            // ID_Modelo
            // 
            this.ID_Modelo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ID_Modelo.HeaderText = "ID Modelo";
            this.ID_Modelo.MinimumWidth = 100;
            this.ID_Modelo.Name = "ID_Modelo";
            this.ID_Modelo.ReadOnly = true;
            // 
            // Nombre
            // 
            this.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.MinimumWidth = 100;
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            // 
            // Cantidad
            // 
            this.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Cantidad.HeaderText = "Cantidad";
            this.Cantidad.MinimumWidth = 100;
            this.Cantidad.Name = "Cantidad";
            this.Cantidad.ReadOnly = true;
            // 
            // Posicion
            // 
            this.Posicion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Posicion.HeaderText = "Posición";
            this.Posicion.MinimumWidth = 100;
            this.Posicion.Name = "Posicion";
            this.Posicion.ReadOnly = true;
            // 
            // Accion
            // 
            this.Accion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Accion.HeaderText = "Acción";
            this.Accion.MinimumWidth = 100;
            this.Accion.Name = "Accion";
            this.Accion.ReadOnly = true;
            // 
            // Eliminar
            // 
            this.Eliminar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Eliminar.HeaderText = "Eliminar";
            this.Eliminar.MinimumWidth = 100;
            this.Eliminar.Name = "Eliminar";
            this.Eliminar.ReadOnly = true;
            // 
            // tabPage_varios
            // 
            this.tabPage_varios.ImageIndex = 1;
            this.tabPage_varios.Location = new System.Drawing.Point(4, 30);
            this.tabPage_varios.Name = "tabPage_varios";
            this.tabPage_varios.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_varios.Size = new System.Drawing.Size(782, 466);
            this.tabPage_varios.TabIndex = 1;
            this.tabPage_varios.Text = "Varios";
            this.tabPage_varios.UseVisualStyleBackColor = true;
            // 
            // tabPage_recursos
            // 
            this.tabPage_recursos.Controls.Add(this.dataGridView_recursos);
            this.tabPage_recursos.ImageIndex = 2;
            this.tabPage_recursos.Location = new System.Drawing.Point(4, 30);
            this.tabPage_recursos.Name = "tabPage_recursos";
            this.tabPage_recursos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_recursos.Size = new System.Drawing.Size(782, 466);
            this.tabPage_recursos.TabIndex = 2;
            this.tabPage_recursos.Text = "Recursos";
            this.tabPage_recursos.UseVisualStyleBackColor = true;
            // 
            // dataGridView_recursos
            // 
            this.dataGridView_recursos.AllowUserToAddRows = false;
            this.dataGridView_recursos.AllowUserToDeleteRows = false;
            this.dataGridView_recursos.AllowUserToOrderColumns = true;
            this.dataGridView_recursos.AllowUserToResizeColumns = false;
            this.dataGridView_recursos.AllowUserToResizeRows = false;
            this.dataGridView_recursos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_recursos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_recursos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn7});
            this.dataGridView_recursos.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_recursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_recursos.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_recursos.Name = "dataGridView_recursos";
            this.dataGridView_recursos.ReadOnly = true;
            this.dataGridView_recursos.RowHeadersVisible = false;
            this.dataGridView_recursos.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_recursos.TabIndex = 1;
            this.dataGridView_recursos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_recursos_CellContentClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "ID Inventario";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 110;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 110;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID Modelo";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn4.HeaderText = "Cantidad";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn7.HeaderText = "Eliminar";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // tabPage_mision
            // 
            this.tabPage_mision.ImageIndex = 3;
            this.tabPage_mision.Location = new System.Drawing.Point(4, 30);
            this.tabPage_mision.Name = "tabPage_mision";
            this.tabPage_mision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_mision.Size = new System.Drawing.Size(782, 466);
            this.tabPage_mision.TabIndex = 3;
            this.tabPage_mision.Text = "Objetos de misión";
            this.tabPage_mision.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "espada.png");
            this.imageList1.Images.SetKeyName(1, "pocion.png");
            this.imageList1.Images.SetKeyName(2, "recursos.png");
            this.imageList1.Images.SetKeyName(3, "llave.png");
            // 
            // UI_Inventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_mision);
            this.Name = "UI_Inventario";
            this.Size = new System.Drawing.Size(790, 500);
            this.tabControl_mision.ResumeLayout(false);
            this.tabPage_equipamiento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_equipamientos)).EndInit();
            this.tabPage_recursos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_recursos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_mision;
        private System.Windows.Forms.TabPage tabPage_equipamiento;
        private System.Windows.Forms.TabPage tabPage_varios;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage_recursos;
        private System.Windows.Forms.TabPage tabPage_mision;
        private System.Windows.Forms.DataGridView dataGridView_equipamientos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_Inventario;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_Modelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Posicion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Accion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Eliminar;
        private System.Windows.Forms.DataGridView dataGridView_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}
