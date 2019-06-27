using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.TabControl
{
    public class Cabezera : Control
    {
        public string cuenta, estado, grupo;
        public Image imagen;
        public bool esta_seleccionada;

        public string propiedad_Cuenta
        {
            get => cuenta;
            set
            {
                cuenta = value;
                Invalidate();
            }
        }

        public string propiedad_Estado
        {
            get => estado;
            set
            {
                estado = value;
                Invalidate();
            }
        }

        public string propiedad_Grupo
        {
            get => grupo;
            set
            {
                grupo = value;
                Invalidate();
            }
        }

        public Image propiedad_Imagen
        {
            get => imagen;
            set
            {
                imagen = value;
                Invalidate();
            }
        }

        public bool propiedad_Esta_Seleccionada
        {
            get => esta_seleccionada;
            set
            {
                esta_seleccionada = value;
                Invalidate();
            }
        }

        public Cabezera()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.FixedHeight, true);
            Cursor = Cursors.Hand;
            Size = new Size(150, 40);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.High;

            base.OnPaint(e);

            Rectangle limites = new Rectangle(0, 0, Width, Height);
            using (SolidBrush b = new SolidBrush(esta_seleccionada ? Color.FromArgb(217, 228, 244) : DefaultBackColor))
            {
                g.FillRectangle(b, limites);
            }
            g.DrawRectangle(Pens.Black, limites);

            if (imagen != null)
            {
                g.DrawImage(imagen, new Rectangle(4, 8, 28, 28));
                limites.X += 30;
            }
            Font fuente = new Font(Font.FontFamily, Font.Size - 1.6F);

            if (!string.IsNullOrEmpty(cuenta) && !string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(grupo))
            {
                SizeF titulo_tamano = g.MeasureString(cuenta, fuente);
                SizeF estado_tamano = g.MeasureString(estado, fuente);
                SizeF estado_grupo = g.MeasureString(grupo, fuente);

                g.DrawString(char.ToUpper(cuenta[0]) + cuenta.Substring(1), fuente, Brushes.Black, limites.X, 25 - ((titulo_tamano.Height + estado_tamano.Height + estado_grupo.Height) / 2));
                g.DrawString($"Estado: {estado}", fuente, Brushes.Black, limites.X, 20 - ((titulo_tamano.Height + estado_tamano.Height + estado_grupo.Height) / 2) + titulo_tamano.Height);
                g.DrawString($"Grupo: {grupo}", fuente, Brushes.Black, limites.X, 15 - ((titulo_tamano.Height + estado_tamano.Height + estado_grupo.Height) / 2) + titulo_tamano.Height + estado_tamano.Height);
            }
            else if (!string.IsNullOrEmpty(cuenta))
            {
                SizeF titulo_tamano = g.MeasureString(cuenta, fuente);
                g.DrawString(cuenta, fuente, Brushes.Black, limites.X, 25 - titulo_tamano.Height / 2);
            }
        }
    }
}
