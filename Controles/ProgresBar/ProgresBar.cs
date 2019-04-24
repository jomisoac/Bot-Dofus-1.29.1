using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.ProgresBar
{
    [DefaultEvent("ValueChanged")]
    class ProgresBar : Control
    {
        private Color color;
        private int valor_maximo, valor;
        private TipoProgresBar tipo_barra;
        public event EventHandler valor_cambiado;

        public ProgresBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = true;
            Size = new Size(100, 24);
            color = Color.FromArgb(102, 150, 232);
            valor_maximo = 100;
            valor = 0;
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public Color color_Barra
        {
            get => color;
            set
            {
                if (color == value) return;
                color = value;
                Invalidate();
            }
        }

        public int valor_Maximo
        {
            get => valor_maximo;
            set
            {
                if (valor_maximo == value) return;
                valor_maximo = value;
                if (valor > valor_maximo)
                    valor = valor_maximo;

                Invalidate();
            }
        }

        public int Valor
        {
            get => valor;
            set
            {
                if (valor == value) return;
                valor = value;
                if (valor > valor_maximo)
                    valor = valor_maximo;
                else if (valor < 0)
                    valor = 0;

                Invalidate();
                valor_cambiado?.Invoke(this, EventArgs.Empty);
            }
        }

        public TipoProgresBar tipos_Barra
        {
            get => tipo_barra;
            set
            {
                tipo_barra = value;
                Invalidate();
            }
        }

        public int porcentaje => (int)((double)Valor / valor_Maximo * 100);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            
            g.Clear(BackColor);
            
            using (SolidBrush brush = new SolidBrush(color_Barra))
            {
                double progressWidth = Width * porcentaje / 100;
                g.FillRectangle(brush, 0, 0, (int)progressWidth, Height);
            }
            
            using (Pen p = new Pen(Color.Black))
            {
                g.DrawLines(p, new Point[]
                {
                    new Point(0, 0),
                    new Point(0, Height),
                    new Point(Width, Height),
                    new Point(Width, 0),
                    new Point(0, 0)
                });
            }

            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                SizeF textSize = g.MeasureString(get_Texto_Barra(), Font);
                g.DrawString(get_Texto_Barra(), Font, brush, Width / 2 - textSize.Width / 2, Height / 2 - textSize.Height / 2);
            }
            base.OnPaint(e);
        }

        private string get_Texto_Barra()
        {
            switch (tipo_barra)
            {
                case TipoProgresBar.VALOR_MAXIMO_PORCENTAJE:
                    return $"{valor}/{valor_maximo} ({porcentaje}%)";
                case TipoProgresBar.VALOR_MAXIMO:
                    return $"{valor}/{valor_maximo}";
                case TipoProgresBar.VALOR_PORCENTAJE:
                    return $"{valor} ({porcentaje}%)";
                case TipoProgresBar.TEXTO_PORCENTAJE:
                    return $"{Text} ({porcentaje}%)";
                default:
                    return $"{porcentaje}%";
            }
        }
    }
}
