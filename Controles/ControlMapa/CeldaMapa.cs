using System;
using System.Drawing;
using System.Linq;

namespace Bot_Dofus_1._29._1.Controles.ControlMapa
{
    public class CeldaMapa
    {
        public static CeldaEstado HighestState = Enum.GetValues(typeof(CeldaEstado)).Cast<CeldaEstado>().Max();
        public int id;
        private Point[] mapa_puntos;

        public CeldaMapa(int _id)
        {
            id = _id;
            activo = true;
        }

        public Point[] Puntos
        {
            get => mapa_puntos;
            set
            {
                mapa_puntos = value;
                RefreshBounds();
            }
        }

        public bool activo { get; set; }
        public CeldaEstado Celda_Estado { get; set; }
        public Brush CustomBrush { get; set; }
        public Pen CustomBorderPen { get; set; }
        public Pen MouseOverPen { get; set; }

        public Brush TextBrush { get; set; }
        public Point Center => new Point((Puntos[0].X + Puntos[2].X) / 2, (Puntos[1].Y + Puntos[3].Y) / 2);
        public int Altura => Puntos[3].Y - Puntos[1].Y;
        public int Anchura => Puntos[2].X - Puntos[0].X;
        public Rectangle Rectangle { get; private set; }

        public void RefreshBounds()
        {
            int x = Puntos.Min(entry => entry.X);
            int y = Puntos.Min(entry => entry.Y);

            int width = Puntos.Max(entry => entry.X) - x;
            int height = Puntos.Max(entry => entry.Y) - y;

            Rectangle = new Rectangle(x, y, width, height);
        }

        public virtual void DrawBorder(Graphics g, Pen pen)
        {
            if (Puntos != null)
            {
                g.DrawPolygon(MouseOverPen ?? CustomBorderPen ?? pen, Puntos);
            }
        }

        public virtual void DrawBackground(ControlMapa parent, Graphics g, Modo_Dibujo mode)
        {
            Brush brush = GetDefaultBrush(parent);

            if (!activo)
            {
                brush = new SolidBrush(parent.ColorCeldaInactiva);
            }
            else if (CustomBrush != null)
            {
                brush = CustomBrush;
            }
            else
            {
                for (CeldaEstado state = HighestState; state > CeldaEstado.NINGUNO; state = (CeldaEstado)((int)state >> 1))
                {
                    if (Celda_Estado.HasFlag(state) && IsStateValid(state, mode) && parent.colores_Celda_Estado.ContainsKey(state))
                    {
                        if(Celda_Estado == CeldaEstado.CAMINABLE)
                        {
                            if ((id % 2) == 0)
                                brush = new SolidBrush(Color.DimGray);
                            else
                                brush = new SolidBrush(Color.Teal);
                        }
                        else
                            brush = new SolidBrush(parent.colores_Celda_Estado[state]);
                    }
                }
            }
            if (Puntos != null)
                g.FillPolygon(brush, Puntos);
        }

        public virtual void DrawForeground(ControlMapa parent, Graphics g)
        {
            StringFormat formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(id.ToString(), parent.Font, TextBrush ?? Brushes.Black, new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height), formato);
        }

        public void dibujar_Redonda(Graphics g)
        {
            g.DrawEllipse(new Pen(Color.Black, 3), new RectangleF(0.0F, 0.0F, Rectangle.Width, Rectangle.Height));
        }

        protected virtual bool IsStateValid(CeldaEstado state, Modo_Dibujo mode)
        {
            if (mode == Modo_Dibujo.NINGUNO)
                return false;

            if (mode == Modo_Dibujo.TODO)
                return true;

            if ((state.HasFlag(CeldaEstado.CAMINABLE) || state.HasFlag(CeldaEstado.NO_CAMINABLE)) && mode.HasFlag(Modo_Dibujo.MOVIMIENTOS))
                return true;

            if ((state.HasFlag(CeldaEstado.PELEA_EQUIPO_AZUL) || state.HasFlag(CeldaEstado.PELEA_EQUIPO_ROJO)) && mode.HasFlag(Modo_Dibujo.PELEAS))
                return true;

            if (state.HasFlag(CeldaEstado.CELDA_TELEPORT) && mode.HasFlag(Modo_Dibujo.CELDAS_TELEPORT))
                return true;

            if (state.HasFlag(CeldaEstado.OBJETO_INTERACTIVO) && mode.HasFlag(Modo_Dibujo.OTROS))
                return true;

            return false;
        }

        public virtual Brush GetDefaultBrush(ControlMapa parent) => new SolidBrush(activo ? parent.ColorCeldaActiva : parent.ColorCeldaInactiva);
        public bool esta_En_Rectangulo(Rectangle rectangulo) => Rectangle.IntersectsWith(rectangulo);
        public bool esta_En_Rectangulo(RectangleF rectangulo) => Rectangle.IntersectsWith(Rectangle.Ceiling(rectangulo));
    }
}
