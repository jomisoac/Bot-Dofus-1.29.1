using Bot_Dofus_1._29._1.Otros;

namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Mapa
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
            this.mapa = new Bot_Dofus_1._29._1.Controles.ControlMapa.ControlMapa();
            this.SuspendLayout();
            // 
            // mapa
            // 
            this.mapa.BorderColorOnOver = System.Drawing.Color.Empty;
            this.mapa.CalidadBaja = false;
            this.mapa.ColorCeldaActiva = System.Drawing.Color.Transparent;
            this.mapa.ColorCeldaInactiva = System.Drawing.Color.DarkGray;
            this.mapa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapa.Location = new System.Drawing.Point(0, 0);
            this.mapa.MapaAltura = 17;
            this.mapa.MapaAnchura = 15;
            this.mapa.ModoDibujo = ((Bot_Dofus_1._29._1.Controles.ControlMapa.Modo_Dibujo)((((Bot_Dofus_1._29._1.Controles.ControlMapa.Modo_Dibujo.MOVIMIENTOS | Bot_Dofus_1._29._1.Controles.ControlMapa.Modo_Dibujo.PELEAS) 
            | Bot_Dofus_1._29._1.Controles.ControlMapa.Modo_Dibujo.CELDAS_TELEPORT) 
            | Bot_Dofus_1._29._1.Controles.ControlMapa.Modo_Dibujo.OTROS)));
            this.mapa.Name = "mapa";
            this.mapa.Size = new System.Drawing.Size(811, 514);
            this.mapa.TabIndex = 0;
            this.mapa.TraceOnOver = false;
            // 
            // UI_Mapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapa);
            this.Name = "UI_Mapa";
            this.Size = new System.Drawing.Size(811, 514);
            this.ResumeLayout(false);

        }

        #endregion

        public Controles.ControlMapa.ControlMapa mapa;
    }
}
