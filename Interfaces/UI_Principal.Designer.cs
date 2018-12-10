namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Principal
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
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Principal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.desconectarOconectarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel_principal = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl_principal = new System.Windows.Forms.TabControl();
            this.tabPage_consola = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.colorCheckBox_canal_informaciomes = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_canal_comercio = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_canal_alineamiento = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_canal_reclutamiento = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_canal_gremio = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_mensajes_privados = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.colorCheckBox_canal_general = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.textbox_logs = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_enviar_consola = new System.Windows.Forms.TextBox();
            this.button_limpiar_consola = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.debugger = new Bot_Dofus_1._29._1.Interfaces.UI_Debugger();
            this.lista_imagenes = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.progresBar_vitalidad = new Bot_Dofus_1._29._1.Controles.ProgresBar.ProgresBar();
            this.progresBar_energia = new Bot_Dofus_1._29._1.Controles.ProgresBar.ProgresBar();
            this.progresBar_experiencia = new Bot_Dofus_1._29._1.Controles.ProgresBar.ProgresBar();
            this.progresBar_pods = new Bot_Dofus_1._29._1.Controles.ProgresBar.ProgresBar();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel_principal.SuspendLayout();
            this.tabControl_principal.SuspendLayout();
            this.tabPage_consola.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.desconectarOconectarToolStripMenuItem,
            this.eliminarToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(804, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // desconectarOconectarToolStripMenuItem
            // 
            this.desconectarOconectarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("desconectarOconectarToolStripMenuItem.Image")));
            this.desconectarOconectarToolStripMenuItem.Name = "desconectarOconectarToolStripMenuItem";
            this.desconectarOconectarToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.desconectarOconectarToolStripMenuItem.Text = "Desconectar";
            this.desconectarOconectarToolStripMenuItem.Click += new System.EventHandler(this.desconectarToolStripMenuItem_Click);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("eliminarToolStripMenuItem.Image")));
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.eliminarToolStripMenuItem.Text = "Eliminar";
            this.eliminarToolStripMenuItem.Click += new System.EventHandler(this.eliminarToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "-";
            // 
            // tableLayoutPanel_principal
            // 
            this.tableLayoutPanel_principal.ColumnCount = 1;
            this.tableLayoutPanel_principal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_principal.Controls.Add(this.tabControl_principal, 0, 0);
            this.tableLayoutPanel_principal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_principal.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanel_principal.Name = "tableLayoutPanel_principal";
            this.tableLayoutPanel_principal.RowCount = 1;
            this.tableLayoutPanel_principal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel_principal.Size = new System.Drawing.Size(804, 545);
            this.tableLayoutPanel_principal.TabIndex = 1;
            // 
            // tabControl_principal
            // 
            this.tabControl_principal.Controls.Add(this.tabPage_consola);
            this.tabControl_principal.Controls.Add(this.tabPage2);
            this.tabControl_principal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_principal.ImageList = this.lista_imagenes;
            this.tabControl_principal.ItemSize = new System.Drawing.Size(67, 26);
            this.tabControl_principal.Location = new System.Drawing.Point(3, 3);
            this.tabControl_principal.Name = "tabControl_principal";
            this.tabControl_principal.SelectedIndex = 0;
            this.tabControl_principal.Size = new System.Drawing.Size(798, 539);
            this.tabControl_principal.TabIndex = 0;
            // 
            // tabPage_consola
            // 
            this.tabPage_consola.Controls.Add(this.tableLayoutPanel2);
            this.tabPage_consola.Controls.Add(this.tableLayoutPanel1);
            this.tabPage_consola.ImageIndex = 0;
            this.tabPage_consola.Location = new System.Drawing.Point(4, 30);
            this.tabPage_consola.Name = "tabPage_consola";
            this.tabPage_consola.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_consola.Size = new System.Drawing.Size(790, 505);
            this.tabPage_consola.TabIndex = 0;
            this.tabPage_consola.Text = "Consola";
            this.tabPage_consola.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textbox_logs, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.64935F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 465);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_informaciomes, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_comercio, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_alineamiento, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_reclutamiento, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_gremio, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_mensajes_privados, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.colorCheckBox_canal_general, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(760, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(21, 459);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // colorCheckBox_canal_informaciomes
            // 
            this.colorCheckBox_canal_informaciomes.AutoSize = true;
            this.colorCheckBox_canal_informaciomes.BackColor = System.Drawing.Color.Green;
            this.colorCheckBox_canal_informaciomes.Checked = true;
            this.colorCheckBox_canal_informaciomes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_informaciomes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_informaciomes.Enabled = false;
            this.colorCheckBox_canal_informaciomes.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_informaciomes.Location = new System.Drawing.Point(3, 48);
            this.colorCheckBox_canal_informaciomes.Name = "colorCheckBox_canal_informaciomes";
            this.colorCheckBox_canal_informaciomes.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_informaciomes.TabIndex = 0;
            this.colorCheckBox_canal_informaciomes.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_canal_comercio
            // 
            this.colorCheckBox_canal_comercio.AutoSize = true;
            this.colorCheckBox_canal_comercio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(62)))), ((int)(((byte)(28)))));
            this.colorCheckBox_canal_comercio.Checked = true;
            this.colorCheckBox_canal_comercio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_comercio.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_comercio.Enabled = false;
            this.colorCheckBox_canal_comercio.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_comercio.Location = new System.Drawing.Point(3, 442);
            this.colorCheckBox_canal_comercio.Name = "colorCheckBox_canal_comercio";
            this.colorCheckBox_canal_comercio.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_comercio.TabIndex = 6;
            this.colorCheckBox_canal_comercio.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_canal_alineamiento
            // 
            this.colorCheckBox_canal_alineamiento.AutoSize = true;
            this.colorCheckBox_canal_alineamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(146)))), ((int)(((byte)(69)))));
            this.colorCheckBox_canal_alineamiento.Checked = true;
            this.colorCheckBox_canal_alineamiento.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_alineamiento.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_alineamiento.Enabled = false;
            this.colorCheckBox_canal_alineamiento.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_alineamiento.Location = new System.Drawing.Point(3, 308);
            this.colorCheckBox_canal_alineamiento.Name = "colorCheckBox_canal_alineamiento";
            this.colorCheckBox_canal_alineamiento.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_alineamiento.TabIndex = 4;
            this.colorCheckBox_canal_alineamiento.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_canal_reclutamiento
            // 
            this.colorCheckBox_canal_reclutamiento.AutoSize = true;
            this.colorCheckBox_canal_reclutamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(133)))), ((int)(((byte)(135)))));
            this.colorCheckBox_canal_reclutamiento.Checked = true;
            this.colorCheckBox_canal_reclutamiento.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_reclutamiento.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_reclutamiento.Enabled = false;
            this.colorCheckBox_canal_reclutamiento.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_reclutamiento.Location = new System.Drawing.Point(3, 373);
            this.colorCheckBox_canal_reclutamiento.Name = "colorCheckBox_canal_reclutamiento";
            this.colorCheckBox_canal_reclutamiento.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_reclutamiento.TabIndex = 5;
            this.colorCheckBox_canal_reclutamiento.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_canal_gremio
            // 
            this.colorCheckBox_canal_gremio.AutoSize = true;
            this.colorCheckBox_canal_gremio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(48)))), ((int)(((byte)(160)))));
            this.colorCheckBox_canal_gremio.Checked = true;
            this.colorCheckBox_canal_gremio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_gremio.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_gremio.Enabled = false;
            this.colorCheckBox_canal_gremio.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_gremio.Location = new System.Drawing.Point(3, 243);
            this.colorCheckBox_canal_gremio.Name = "colorCheckBox_canal_gremio";
            this.colorCheckBox_canal_gremio.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_gremio.TabIndex = 3;
            this.colorCheckBox_canal_gremio.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_mensajes_privados
            // 
            this.colorCheckBox_mensajes_privados.AutoSize = true;
            this.colorCheckBox_mensajes_privados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(112)))), ((int)(((byte)(196)))));
            this.colorCheckBox_mensajes_privados.Checked = true;
            this.colorCheckBox_mensajes_privados.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_mensajes_privados.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_mensajes_privados.Enabled = false;
            this.colorCheckBox_mensajes_privados.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_mensajes_privados.Location = new System.Drawing.Point(3, 178);
            this.colorCheckBox_mensajes_privados.Name = "colorCheckBox_mensajes_privados";
            this.colorCheckBox_mensajes_privados.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_mensajes_privados.TabIndex = 2;
            this.colorCheckBox_mensajes_privados.UseVisualStyleBackColor = false;
            // 
            // colorCheckBox_canal_general
            // 
            this.colorCheckBox_canal_general.AutoSize = true;
            this.colorCheckBox_canal_general.BackColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_general.Checked = true;
            this.colorCheckBox_canal_general.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorCheckBox_canal_general.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.colorCheckBox_canal_general.Enabled = false;
            this.colorCheckBox_canal_general.ForeColor = System.Drawing.Color.Black;
            this.colorCheckBox_canal_general.Location = new System.Drawing.Point(3, 113);
            this.colorCheckBox_canal_general.Name = "colorCheckBox_canal_general";
            this.colorCheckBox_canal_general.Size = new System.Drawing.Size(15, 14);
            this.colorCheckBox_canal_general.TabIndex = 1;
            this.colorCheckBox_canal_general.UseVisualStyleBackColor = false;
            // 
            // textbox_logs
            // 
            this.textbox_logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textbox_logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textbox_logs.Location = new System.Drawing.Point(3, 3);
            this.textbox_logs.MaxLength = 200;
            this.textbox_logs.Name = "textbox_logs";
            this.textbox_logs.Size = new System.Drawing.Size(751, 459);
            this.textbox_logs.TabIndex = 5;
            this.textbox_logs.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Controls.Add(this.textBox_enviar_consola, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_limpiar_consola, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 468);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 34);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBox_enviar_consola
            // 
            this.textBox_enviar_consola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_enviar_consola.Enabled = false;
            this.textBox_enviar_consola.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox_enviar_consola.Location = new System.Drawing.Point(3, 3);
            this.textBox_enviar_consola.MaxLength = 50;
            this.textBox_enviar_consola.Name = "textBox_enviar_consola";
            this.textBox_enviar_consola.Size = new System.Drawing.Size(743, 25);
            this.textBox_enviar_consola.TabIndex = 0;
            // 
            // button_limpiar_consola
            // 
            this.button_limpiar_consola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_limpiar_consola.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_limpiar_consola.Image = ((System.Drawing.Image)(resources.GetObject("button_limpiar_consola.Image")));
            this.button_limpiar_consola.Location = new System.Drawing.Point(752, 3);
            this.button_limpiar_consola.Name = "button_limpiar_consola";
            this.button_limpiar_consola.Size = new System.Drawing.Size(29, 28);
            this.button_limpiar_consola.TabIndex = 1;
            this.button_limpiar_consola.UseVisualStyleBackColor = true;
            this.button_limpiar_consola.Click += new System.EventHandler(this.button_limpiar_consola_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.debugger);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(790, 505);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Debugger";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // debugger
            // 
            this.debugger.Cursor = System.Windows.Forms.Cursors.Default;
            this.debugger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugger.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.debugger.Location = new System.Drawing.Point(3, 3);
            this.debugger.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.debugger.MinimumSize = new System.Drawing.Size(790, 500);
            this.debugger.Name = "debugger";
            this.debugger.Size = new System.Drawing.Size(790, 500);
            this.debugger.TabIndex = 0;
            // 
            // lista_imagenes
            // 
            this.lista_imagenes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lista_imagenes.ImageStream")));
            this.lista_imagenes.TransparentColor = System.Drawing.Color.Transparent;
            this.lista_imagenes.Images.SetKeyName(0, "application32.png");
            this.lista_imagenes.Images.SetKeyName(1, "f-bug_256-24.png");
            this.lista_imagenes.Images.SetKeyName(2, "user-blue32.png");
            this.lista_imagenes.Images.SetKeyName(3, "briefcase48.png");
            this.lista_imagenes.Images.SetKeyName(4, "678074-map-24.png");
            this.lista_imagenes.Images.SetKeyName(5, "Iron_Sword32.png");
            this.lista_imagenes.Images.SetKeyName(6, "bank_silver32.png");
            this.lista_imagenes.Images.SetKeyName(7, "cog-icon-2-48x48 (1).png");
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 10;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.19867F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.80491F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.197404F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8017F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.197404F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8017F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.197404F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8017F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.197404F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8017F));
            this.tableLayoutPanel4.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.pictureBox2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.pictureBox3, 6, 0);
            this.tableLayoutPanel4.Controls.Add(this.pictureBox4, 8, 0);
            this.tableLayoutPanel4.Controls.Add(this.pictureBox5, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.progresBar_vitalidad, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.progresBar_energia, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.progresBar_experiencia, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.progresBar_pods, 7, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 571);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(804, 33);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(163, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 27);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(483, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(35, 27);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(643, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(35, 27);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox5.Image = global::Bot_Dofus_1._29._1.Properties.Resources.experiencia;
            this.pictureBox5.Location = new System.Drawing.Point(323, 3);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(35, 27);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox5.TabIndex = 4;
            this.pictureBox5.TabStop = false;
            // 
            // progresBar_vitalidad
            // 
            this.progresBar_vitalidad.color_Barra = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(150)))), ((int)(((byte)(232)))));
            this.progresBar_vitalidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progresBar_vitalidad.Location = new System.Drawing.Point(44, 3);
            this.progresBar_vitalidad.Name = "progresBar_vitalidad";
            this.progresBar_vitalidad.Size = new System.Drawing.Size(113, 27);
            this.progresBar_vitalidad.TabIndex = 5;
            this.progresBar_vitalidad.tipos_Barra = Bot_Dofus_1._29._1.Controles.ProgresBar.TipoProgresBar.VALOR_MAXIMO_PORCENTAJE;
            this.progresBar_vitalidad.Valor = 0;
            this.progresBar_vitalidad.valor_Maximo = 100;
            // 
            // progresBar_energia
            // 
            this.progresBar_energia.color_Barra = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(150)))), ((int)(((byte)(232)))));
            this.progresBar_energia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progresBar_energia.Location = new System.Drawing.Point(204, 3);
            this.progresBar_energia.Name = "progresBar_energia";
            this.progresBar_energia.Size = new System.Drawing.Size(113, 27);
            this.progresBar_energia.TabIndex = 6;
            this.progresBar_energia.tipos_Barra = Bot_Dofus_1._29._1.Controles.ProgresBar.TipoProgresBar.VALOR_PORCENTAJE;
            this.progresBar_energia.Valor = 0;
            this.progresBar_energia.valor_Maximo = 10000;
            // 
            // progresBar_experiencia
            // 
            this.progresBar_experiencia.color_Barra = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(150)))), ((int)(((byte)(232)))));
            this.progresBar_experiencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progresBar_experiencia.Location = new System.Drawing.Point(364, 3);
            this.progresBar_experiencia.Name = "progresBar_experiencia";
            this.progresBar_experiencia.Size = new System.Drawing.Size(113, 27);
            this.progresBar_experiencia.TabIndex = 7;
            this.progresBar_experiencia.Text = "0";
            this.progresBar_experiencia.tipos_Barra = Bot_Dofus_1._29._1.Controles.ProgresBar.TipoProgresBar.TEXTO_PORCENTAJE;
            this.progresBar_experiencia.Valor = 0;
            this.progresBar_experiencia.valor_Maximo = 100;
            // 
            // progresBar_pods
            // 
            this.progresBar_pods.color_Barra = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(150)))), ((int)(((byte)(232)))));
            this.progresBar_pods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progresBar_pods.Location = new System.Drawing.Point(524, 3);
            this.progresBar_pods.Name = "progresBar_pods";
            this.progresBar_pods.Size = new System.Drawing.Size(113, 27);
            this.progresBar_pods.TabIndex = 8;
            this.progresBar_pods.tipos_Barra = Bot_Dofus_1._29._1.Controles.ProgresBar.TipoProgresBar.VALOR_MAXIMO_PORCENTAJE;
            this.progresBar_pods.Valor = 0;
            this.progresBar_pods.valor_Maximo = 100;
            // 
            // UI_Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel_principal);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UI_Principal";
            this.Size = new System.Drawing.Size(804, 604);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel_principal.ResumeLayout(false);
            this.tabControl_principal.ResumeLayout(false);
            this.tabPage_consola.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem desconectarOconectarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_principal;
        private System.Windows.Forms.TabControl tabControl_principal;
        private System.Windows.Forms.TabPage tabPage_consola;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_informaciomes;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_comercio;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_alineamiento;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_reclutamiento;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_gremio;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_mensajes_privados;
        private Controles.ColorCheckBox.ColorCheckBox colorCheckBox_canal_general;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBox_enviar_consola;
        private System.Windows.Forms.Button button_limpiar_consola;
        private System.Windows.Forms.RichTextBox textbox_logs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private Controles.ProgresBar.ProgresBar progresBar_vitalidad;
        private Controles.ProgresBar.ProgresBar progresBar_energia;
        private Controles.ProgresBar.ProgresBar progresBar_experiencia;
        private Controles.ProgresBar.ProgresBar progresBar_pods;
        private UI_Debugger debugger;
        private System.Windows.Forms.ImageList lista_imagenes;
    }
}
