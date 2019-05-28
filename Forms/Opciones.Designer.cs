namespace Bot_Dofus_1._29._1.Forms
{
    partial class Opciones
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_mensajes_debug = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.boton_opciones_guardar = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_puerto_servidor = new System.Windows.Forms.TextBox();
            this.label_puerto_servidor = new System.Windows.Forms.Label();
            this.textBox_ip_servidor = new System.Windows.Forms.TextBox();
            this.label_ip_conexion = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 272);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.checkBox_mensajes_debug, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(372, 106);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // checkBox_mensajes_debug
            // 
            this.checkBox_mensajes_debug.AutoSize = true;
            this.checkBox_mensajes_debug.Checked = true;
            this.checkBox_mensajes_debug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_mensajes_debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_mensajes_debug.Location = new System.Drawing.Point(3, 3);
            this.checkBox_mensajes_debug.Name = "checkBox_mensajes_debug";
            this.checkBox_mensajes_debug.Size = new System.Drawing.Size(366, 20);
            this.checkBox_mensajes_debug.TabIndex = 0;
            this.checkBox_mensajes_debug.Text = "Mostrar los mensajes debug";
            this.checkBox_mensajes_debug.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 130);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Otros";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.boton_opciones_guardar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 35);
            this.panel1.TabIndex = 1;
            // 
            // boton_opciones_guardar
            // 
            this.boton_opciones_guardar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boton_opciones_guardar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boton_opciones_guardar.Location = new System.Drawing.Point(0, 0);
            this.boton_opciones_guardar.Name = "boton_opciones_guardar";
            this.boton_opciones_guardar.Size = new System.Drawing.Size(372, 35);
            this.boton_opciones_guardar.TabIndex = 0;
            this.boton_opciones_guardar.Text = "Guardar";
            this.boton_opciones_guardar.UseVisualStyleBackColor = true;
            this.boton_opciones_guardar.Click += new System.EventHandler(this.boton_opciones_guardar_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.62366F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.37634F));
            this.tableLayoutPanel3.Controls.Add(this.textBox_puerto_servidor, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label_puerto_servidor, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBox_ip_servidor, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label_ip_conexion, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.6129F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.3871F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(372, 71);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // textBox_puerto_servidor
            // 
            this.textBox_puerto_servidor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_puerto_servidor.Location = new System.Drawing.Point(169, 35);
            this.textBox_puerto_servidor.MaxLength = 5;
            this.textBox_puerto_servidor.Name = "textBox_puerto_servidor";
            this.textBox_puerto_servidor.Size = new System.Drawing.Size(200, 25);
            this.textBox_puerto_servidor.TabIndex = 3;
            // 
            // label_puerto_servidor
            // 
            this.label_puerto_servidor.AutoSize = true;
            this.label_puerto_servidor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_puerto_servidor.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label_puerto_servidor.Location = new System.Drawing.Point(3, 32);
            this.label_puerto_servidor.Name = "label_puerto_servidor";
            this.label_puerto_servidor.Size = new System.Drawing.Size(160, 30);
            this.label_puerto_servidor.TabIndex = 2;
            this.label_puerto_servidor.Text = "Puerto servidor conexión:";
            this.label_puerto_servidor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_ip_servidor
            // 
            this.textBox_ip_servidor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_ip_servidor.Location = new System.Drawing.Point(169, 3);
            this.textBox_ip_servidor.MaxLength = 35;
            this.textBox_ip_servidor.Name = "textBox_ip_servidor";
            this.textBox_ip_servidor.Size = new System.Drawing.Size(200, 25);
            this.textBox_ip_servidor.TabIndex = 1;
            // 
            // label_ip_conexion
            // 
            this.label_ip_conexion.AutoSize = true;
            this.label_ip_conexion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_ip_conexion.Location = new System.Drawing.Point(3, 0);
            this.label_ip_conexion.Name = "label_ip_conexion";
            this.label_ip_conexion.Size = new System.Drawing.Size(160, 32);
            this.label_ip_conexion.TabIndex = 0;
            this.label_ip_conexion.Text = "IP servidor conexión:";
            this.label_ip_conexion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Opciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 272);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 311);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 311);
            this.Name = "Opciones";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajustes";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBox_mensajes_debug;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button boton_opciones_guardar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label_ip_conexion;
        private System.Windows.Forms.TextBox textBox_ip_servidor;
        private System.Windows.Forms.Label label_puerto_servidor;
        private System.Windows.Forms.TextBox textBox_puerto_servidor;
    }
}