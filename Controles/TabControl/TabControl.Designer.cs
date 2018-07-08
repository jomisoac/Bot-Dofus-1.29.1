using Bot_Dofus_1._29._1.Controles.LayoutPanel;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.TabControl
{
    partial class TabControl
    {
        private System.ComponentModel.IContainer components = null;

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
            this.panelCabezeraCuentas = new FlowLayoutPanelBuffered();
            this.panelContenidoCuenta = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelCabezeraCuentas
            // 
            this.panelCabezeraCuentas.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelCabezeraCuentas.Location = new System.Drawing.Point(0, 0);
            this.panelCabezeraCuentas.Name = "panelCabezeraCuentas";
            this.panelCabezeraCuentas.Size = new System.Drawing.Size(174, 540);
            this.panelCabezeraCuentas.TabIndex = 0;
            // 
            // panelContenidoCuenta
            // 
            this.panelContenidoCuenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenidoCuenta.Location = new System.Drawing.Point(174, 0);
            this.panelContenidoCuenta.Name = "panelContenidoCuenta";
            this.panelContenidoCuenta.Size = new System.Drawing.Size(734, 540);
            this.panelContenidoCuenta.TabIndex = 0;
            // 
            // TabControl_Horizontal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContenidoCuenta);
            this.Controls.Add(this.panelCabezeraCuentas);
            this.Name = "TabControl_Horizontal";
            this.Size = new System.Drawing.Size(908, 540);
            this.ResumeLayout(false);

        }

        private FlowLayoutPanelBuffered panelCabezeraCuentas;
        private Panel panelContenidoCuenta;
    }
}
