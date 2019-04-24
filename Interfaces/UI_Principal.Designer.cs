using Bot_Dofus_1._29._1.Controles.ColorCheckBox;

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
            this.ScriptTituloStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel_principal = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl_principal = new System.Windows.Forms.TabControl();
            this.tabPage_consola = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayout_Canales = new System.Windows.Forms.TableLayoutPanel();
            this.canal_incarnam = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_informaciones = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_comercio = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_alineamiento = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_reclutamiento = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_gremio = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_privado = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
            this.canal_general = new Bot_Dofus_1._29._1.Controles.ColorCheckBox.ColorCheckBox();
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
            this.label_kamas_principal = new System.Windows.Forms.Label();
            this.label_lider = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel_principal.SuspendLayout();
            this.tabControl_principal.SuspendLayout();
            this.tabPage_consola.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayout_Canales.SuspendLayout();
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
            this.ScriptTituloStripMenuItem,
            this.cargarScriptToolStripMenuItem,
            this.iniciarScriptToolStripMenuItem});
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
            // ScriptTituloStripMenuItem
            // 
            this.ScriptTituloStripMenuItem.Name = "ScriptTituloStripMenuItem";
            this.ScriptTituloStripMenuItem.Size = new System.Drawing.Size(24, 20);
            this.ScriptTituloStripMenuItem.Text = "-";
            // 
            // cargarScriptToolStripMenuItem
            // 
            this.cargarScriptToolStripMenuItem.Enabled = false;
            this.cargarScriptToolStripMenuItem.Image = global::Bot_Dofus_1._29._1.Properties.Resources.documento_azul;
            this.cargarScriptToolStripMenuItem.Name = "cargarScriptToolStripMenuItem";
            this.cargarScriptToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.cargarScriptToolStripMenuItem.Text = "Cargar script";
            this.cargarScriptToolStripMenuItem.Click += new System.EventHandler(this.cargarScriptToolStripMenuItem_Click);
            // 
            // iniciarScriptToolStripMenuItem
            // 
            this.iniciarScriptToolStripMenuItem.Enabled = false;
            this.iniciarScriptToolStripMenuItem.Image = global::Bot_Dofus_1._29._1.Properties.Resources.boton_play;
            this.iniciarScriptToolStripMenuItem.Name = "iniciarScriptToolStripMenuItem";
            this.iniciarScriptToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.iniciarScriptToolStripMenuItem.Click += new System.EventHandler(this.iniciarScriptToolStripMenuItem_Click);
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
            this.tableLayoutPanel2.Controls.Add(this.tableLayout_Canales, 1, 0);
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
            // tableLayout_Canales
            // 
            this.tableLayout_Canales.ColumnCount = 1;
            this.tableLayout_Canales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Canales.Controls.Add(this.canal_incarnam, 0, 7);
            this.tableLayout_Canales.Controls.Add(this.canal_informaciones, 0, 0);
            this.tableLayout_Canales.Controls.Add(this.canal_comercio, 0, 6);
            this.tableLayout_Canales.Controls.Add(this.canal_alineamiento, 0, 4);
            this.tableLayout_Canales.Controls.Add(this.canal_reclutamiento, 0, 5);
            this.tableLayout_Canales.Controls.Add(this.canal_gremio, 0, 3);
            this.tableLayout_Canales.Controls.Add(this.canal_privado, 0, 2);
            this.tableLayout_Canales.Controls.Add(this.canal_general, 0, 1);
            this.tableLayout_Canales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Canales.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayout_Canales.Location = new System.Drawing.Point(760, 3);
            this.tableLayout_Canales.Name = "tableLayout_Canales";
            this.tableLayout_Canales.RowCount = 8;
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayout_Canales.Size = new System.Drawing.Size(21, 459);
            this.tableLayout_Canales.TabIndex = 0;
            // 
            // canal_incarnam
            // 
            this.canal_incarnam.AutoSize = true;
            this.canal_incarnam.BackColor = System.Drawing.Color.Blue;
            this.canal_incarnam.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_incarnam.Enabled = false;
            this.canal_incarnam.ForeColor = System.Drawing.Color.Black;
            this.canal_incarnam.Location = new System.Drawing.Point(3, 442);
            this.canal_incarnam.Name = "canal_incarnam";
            this.canal_incarnam.Size = new System.Drawing.Size(15, 14);
            this.canal_incarnam.TabIndex = 7;
            this.canal_incarnam.UseVisualStyleBackColor = false;
            this.canal_incarnam.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_informaciones
            // 
            this.canal_informaciones.AutoSize = true;
            this.canal_informaciones.BackColor = System.Drawing.Color.Green;
            this.canal_informaciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_informaciones.Enabled = false;
            this.canal_informaciones.ForeColor = System.Drawing.Color.Black;
            this.canal_informaciones.Location = new System.Drawing.Point(3, 40);
            this.canal_informaciones.Name = "canal_informaciones";
            this.canal_informaciones.Size = new System.Drawing.Size(15, 14);
            this.canal_informaciones.TabIndex = 0;
            this.canal_informaciones.UseVisualStyleBackColor = false;
            this.canal_informaciones.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_comercio
            // 
            this.canal_comercio.AutoSize = true;
            this.canal_comercio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(62)))), ((int)(((byte)(28)))));
            this.canal_comercio.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_comercio.Enabled = false;
            this.canal_comercio.ForeColor = System.Drawing.Color.Black;
            this.canal_comercio.Location = new System.Drawing.Point(3, 382);
            this.canal_comercio.Name = "canal_comercio";
            this.canal_comercio.Size = new System.Drawing.Size(15, 14);
            this.canal_comercio.TabIndex = 6;
            this.canal_comercio.UseVisualStyleBackColor = false;
            this.canal_comercio.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_alineamiento
            // 
            this.canal_alineamiento.AutoSize = true;
            this.canal_alineamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(146)))), ((int)(((byte)(69)))));
            this.canal_alineamiento.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_alineamiento.Enabled = false;
            this.canal_alineamiento.ForeColor = System.Drawing.Color.Black;
            this.canal_alineamiento.Location = new System.Drawing.Point(3, 268);
            this.canal_alineamiento.Name = "canal_alineamiento";
            this.canal_alineamiento.Size = new System.Drawing.Size(15, 14);
            this.canal_alineamiento.TabIndex = 4;
            this.canal_alineamiento.UseVisualStyleBackColor = false;
            this.canal_alineamiento.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_reclutamiento
            // 
            this.canal_reclutamiento.AutoSize = true;
            this.canal_reclutamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(133)))), ((int)(((byte)(135)))));
            this.canal_reclutamiento.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_reclutamiento.Enabled = false;
            this.canal_reclutamiento.ForeColor = System.Drawing.Color.Black;
            this.canal_reclutamiento.Location = new System.Drawing.Point(3, 325);
            this.canal_reclutamiento.Name = "canal_reclutamiento";
            this.canal_reclutamiento.Size = new System.Drawing.Size(15, 14);
            this.canal_reclutamiento.TabIndex = 5;
            this.canal_reclutamiento.UseVisualStyleBackColor = false;
            this.canal_reclutamiento.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_gremio
            // 
            this.canal_gremio.AutoSize = true;
            this.canal_gremio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(48)))), ((int)(((byte)(160)))));
            this.canal_gremio.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_gremio.Enabled = false;
            this.canal_gremio.ForeColor = System.Drawing.Color.Black;
            this.canal_gremio.Location = new System.Drawing.Point(3, 211);
            this.canal_gremio.Name = "canal_gremio";
            this.canal_gremio.Size = new System.Drawing.Size(15, 14);
            this.canal_gremio.TabIndex = 3;
            this.canal_gremio.UseVisualStyleBackColor = false;
            this.canal_gremio.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_privado
            // 
            this.canal_privado.AutoSize = true;
            this.canal_privado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(112)))), ((int)(((byte)(196)))));
            this.canal_privado.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_privado.Enabled = false;
            this.canal_privado.ForeColor = System.Drawing.Color.Black;
            this.canal_privado.Location = new System.Drawing.Point(3, 154);
            this.canal_privado.Name = "canal_privado";
            this.canal_privado.Size = new System.Drawing.Size(15, 14);
            this.canal_privado.TabIndex = 2;
            this.canal_privado.UseVisualStyleBackColor = false;
            this.canal_privado.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
            // 
            // canal_general
            // 
            this.canal_general.AutoSize = true;
            this.canal_general.BackColor = System.Drawing.Color.Black;
            this.canal_general.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.canal_general.Enabled = false;
            this.canal_general.ForeColor = System.Drawing.Color.Black;
            this.canal_general.Location = new System.Drawing.Point(3, 97);
            this.canal_general.Name = "canal_general";
            this.canal_general.Size = new System.Drawing.Size(15, 14);
            this.canal_general.TabIndex = 1;
            this.canal_general.UseVisualStyleBackColor = false;
            this.canal_general.CheckedChanged += new System.EventHandler(this.canal_CheckedChanged);
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
            this.textBox_enviar_consola.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_enviar_consola_KeyDown);
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
            this.lista_imagenes.Images.SetKeyName(0, "terminal.png");
            this.lista_imagenes.Images.SetKeyName(1, "debugger.png");
            this.lista_imagenes.Images.SetKeyName(2, "personaje.png");
            this.lista_imagenes.Images.SetKeyName(3, "inventario.png");
            this.lista_imagenes.Images.SetKeyName(4, "mapa.png");
            this.lista_imagenes.Images.SetKeyName(5, "pelea.png");
            this.lista_imagenes.Images.SetKeyName(6, "bolsa_dinero.png");
            this.lista_imagenes.Images.SetKeyName(7, "ajustes.png");
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
            this.tableLayoutPanel4.Controls.Add(this.label_kamas_principal, 9, 0);
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
            this.progresBar_energia.tipos_Barra = Bot_Dofus_1._29._1.Controles.ProgresBar.TipoProgresBar.VALOR_MAXIMO;
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
            // label_kamas_principal
            // 
            this.label_kamas_principal.AutoSize = true;
            this.label_kamas_principal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_kamas_principal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_kamas_principal.Location = new System.Drawing.Point(684, 0);
            this.label_kamas_principal.Name = "label_kamas_principal";
            this.label_kamas_principal.Size = new System.Drawing.Size(117, 33);
            this.label_kamas_principal.TabIndex = 9;
            this.label_kamas_principal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_lider
            // 
            this.label_lider.AutoSize = true;
            this.label_lider.BackColor = System.Drawing.SystemColors.Control;
            this.label_lider.Location = new System.Drawing.Point(596, 6);
            this.label_lider.Name = "label_lider";
            this.label_lider.Size = new System.Drawing.Size(0, 17);
            this.label_lider.TabIndex = 2;
            // 
            // UI_Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_lider);
            this.Controls.Add(this.tableLayoutPanel_principal);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UI_Principal";
            this.Size = new System.Drawing.Size(804, 604);
            this.Load += new System.EventHandler(this.UI_Principal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel_principal.ResumeLayout(false);
            this.tabControl_principal.ResumeLayout(false);
            this.tabPage_consola.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayout_Canales.ResumeLayout(false);
            this.tableLayout_Canales.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem ScriptTituloStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_principal;
        private System.Windows.Forms.TabControl tabControl_principal;
        private System.Windows.Forms.TabPage tabPage_consola;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Canales;
        private ColorCheckBox canal_informaciones;
        private ColorCheckBox canal_comercio;
        private ColorCheckBox canal_alineamiento;
        private ColorCheckBox canal_reclutamiento;
        private ColorCheckBox canal_gremio;
        private ColorCheckBox canal_privado;
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
        private ColorCheckBox canal_incarnam;
        private ColorCheckBox canal_general;
        private System.Windows.Forms.Label label_kamas_principal;
        private System.Windows.Forms.ToolStripMenuItem cargarScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iniciarScriptToolStripMenuItem;
        private System.Windows.Forms.Label label_lider;
    }
}
