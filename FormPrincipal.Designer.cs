namespace Bot_Dofus_1._29._1
{
    partial class FormPrincipal
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuSuperiorPrincipal = new System.Windows.Forms.MenuStrip();
            this.statusStripInferiorPrincipal = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // menuSuperiorPrincipal
            // 
            this.menuSuperiorPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuSuperiorPrincipal.Name = "menuSuperiorPrincipal";
            this.menuSuperiorPrincipal.Size = new System.Drawing.Size(800, 24);
            this.menuSuperiorPrincipal.TabIndex = 0;
            this.menuSuperiorPrincipal.Text = "menuSuperiorPrincipal";
            // 
            // statusStripInferiorPrincipal
            // 
            this.statusStripInferiorPrincipal.Location = new System.Drawing.Point(0, 428);
            this.statusStripInferiorPrincipal.Name = "statusStripInferiorPrincipal";
            this.statusStripInferiorPrincipal.Size = new System.Drawing.Size(800, 22);
            this.statusStripInferiorPrincipal.TabIndex = 1;
            this.statusStripInferiorPrincipal.Text = "statusStripInferiorPrincipal";
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStripInferiorPrincipal);
            this.Controls.Add(this.menuSuperiorPrincipal);
            this.MainMenuStrip = this.menuSuperiorPrincipal;
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuSuperiorPrincipal;
        private System.Windows.Forms.StatusStrip statusStripInferiorPrincipal;
    }
}

