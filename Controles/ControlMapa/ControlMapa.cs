using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.ControlMapa
{
    public partial class ControlMapa : UserControl
    {
        public delegate void CellClickedHandler(CeldaMapa celda, MouseButtons botones);
        public event CellClickedHandler clic_celda;
        public event Action<CeldaMapa, CeldaMapa> clic_celda_terminado;

        protected void OnCellClicked(CeldaMapa cell, MouseButtons buttons)
        {
            clic_celda?.Invoke(cell, buttons);
        }

        protected void OnCellOver(CeldaMapa cell, CeldaMapa last)
        {
            clic_celda_terminado?.Invoke(cell, last);
        }

        private int mapa_altura, mapa_anchura;
        private bool mapa_calidad_baja;
        private bool mapa_raton_abajo;
        private CeldaMapa mapa_celda_retenida;
        private CeldaMapa mapa_celda_abajo;

        public ControlMapa()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            MapaAltura = 17;
            MapaAnchura = 15;
            ModoDibujo = Modo_Dibujo.TODO;
            TraceOnOver = false;
            ColorCeldaInactiva = Color.DarkGray;
            ColorCeldaActiva = Color.Transparent;
            colores_Celda_Estado = new Dictionary<CeldaEstado, Color>
            {
                { CeldaEstado.CAMINABLE, Color.Gray},
                { CeldaEstado.NO_CAMINABLE, Color.Maroon},
                { CeldaEstado.PELEA_EQUIPO_AZUL, Color.DodgerBlue},
                { CeldaEstado.PELEA_EQUIPO_ROJO, Color.Red},
                { CeldaEstado.CELDA_TELEPORT, Color.Orange},
                { CeldaEstado.OBJETO_INTERACTIVO, Color.LightGoldenrodYellow}
            };
            set_Celda_Numero();
            dibujar_Mapa();
            InitializeComponent();
        }

        public int MapaAltura
        {
            get => mapa_altura;
            set
            {
                mapa_altura = value;
                set_Celda_Numero();
            }
        }

        public int MapaAnchura
        {
            get => mapa_anchura;
            set
            {
                mapa_anchura = value;
                set_Celda_Numero();
            }
        }

        [Browsable(false)]
        public int RealCellHeight { get; private set; }
        [Browsable(false)]
        public int RealCellWidth { get; private set; }
        public Color ColorCeldaInactiva { get; set; }
        public Color ColorCeldaActiva { get; set; }
        public Modo_Dibujo ModoDibujo { get; set; }
        public bool TraceOnOver { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public CeldaMapa CurrentCellOver { get; set; }
        public Color BorderColorOnOver { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Dictionary<CeldaEstado, Color> colores_Celda_Estado { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public CeldaMapa[] celdas { get; set; }

        public bool CalidadBaja
        {
            get => mapa_calidad_baja;
            set
            {
                mapa_calidad_baja = value;
                Invalidate();
            }
        }

        private void aplicar_Calidad_Mapa(Graphics g)
        {
            if (mapa_calidad_baja)
            {
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
            }
            else
            {
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void set_Celda_Numero()
        {
            celdas = new CeldaMapa[2 * MapaAltura * MapaAnchura];

            int cellId = 0;
            CeldaMapa celda;
            for (int y = 0; y < MapaAltura; y++)
            {
                for (int x = 0; x < MapaAnchura * 2; x++)
                {
                    celda = new CeldaMapa(cellId++);
                    celdas[celda.id] = celda;
                }
            }
        }

        private double get_Maximo_Escalado()
        {
            double celda_anchura = Width / (double)(MapaAnchura + 1);
            double cellHeight = Height / (double)(MapaAltura + 1);
            celda_anchura = Math.Min(cellHeight * 2, celda_anchura);

            return celda_anchura;
        }

        public void dibujar_Mapa()
        {
            int cellId = 0;
            double cellWidth = get_Maximo_Escalado();
            double cellHeight = Math.Ceiling(cellWidth / 2);

            int offsetX = Convert.ToInt32((Width - ((MapaAnchura + 0.5) * cellWidth)) / 2);
            var offsetY = Convert.ToInt32((Height - ((MapaAltura + 0.5) * cellHeight)) / 2);

            double midCellHeight = cellHeight / 2;
            double midCellWidth = cellWidth / 2;
            
            for (int y = 0; y <= (2 * MapaAltura) - 1; ++y)
            {
                if ((y % 2) == 0)
                {
                    for (int x = 0; x <= MapaAnchura - 1; x++)//dibuja los impares
                    {
                        Point left = new Point(Convert.ToInt32(offsetX + (x * cellWidth)), Convert.ToInt32(offsetY + (y * midCellHeight) + midCellHeight));
                        Point top = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + midCellWidth), Convert.ToInt32(offsetY + (y * midCellHeight)));
                        Point right = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + cellWidth), Convert.ToInt32(offsetY + (y * midCellHeight) + midCellHeight));
                        Point down = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + midCellWidth), Convert.ToInt32(offsetY + (y * midCellHeight) + cellHeight));
                        celdas[cellId++].Puntos = new[] { left, top, right, down };
                    }
                }
                else
                {
                    for (int x = 0; x <= MapaAnchura - 2; x++)//dibuja los pares
                    {
                        Point left = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + midCellWidth), Convert.ToInt32(offsetY + (y * midCellHeight) + midCellHeight));
                        Point top = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + cellWidth), Convert.ToInt32(offsetY + (y * midCellHeight)));
                        Point right = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + cellWidth + midCellWidth), Convert.ToInt32(offsetY + (y * midCellHeight) + midCellHeight));
                        Point down = new Point(Convert.ToInt32(offsetX + (x * cellWidth) + cellWidth), Convert.ToInt32(offsetY + (y * midCellHeight) + cellHeight));
                        celdas[cellId++].Puntos = new[] { left, top, right, down };
                    }
                }
            }
            RealCellHeight = (int)cellHeight;
            RealCellWidth = (int)cellWidth;
        }

        public void Draw(Graphics g)
        {
            aplicar_Calidad_Mapa(g);
            g.Clear(BackColor);
            Pen pen = new Pen(ForeColor);

            foreach(CeldaMapa celda in celdas)
            {
                if (celda.esta_En_Rectangulo(g.ClipBounds))
                {
                    celda.DrawBackground(this, g, ModoDibujo);
                    celda.DrawForeground(this, g);
                }

                if (celda.esta_En_Rectangulo(g.ClipBounds))
                    celda.DrawBorder(g, pen);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics);
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            dibujar_Mapa();
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mapa_raton_abajo)
            {
                CeldaMapa celda = get_Celda(e.Location);
                if (mapa_celda_retenida != null && mapa_celda_retenida != celda)
                {
                    OnCellClicked(mapa_celda_retenida, e.Button);
                    mapa_celda_retenida = celda;
                }
                if (celda != null)
                    OnCellClicked(celda, e.Button);
            }

            if (TraceOnOver)
            {
                var cell = get_Celda(e.Location);
                Rectangle rect = Rectangle.Empty;
                CeldaMapa last = null;

                if (CurrentCellOver != null && CurrentCellOver != cell)
                {
                    CurrentCellOver.MouseOverPen = null;

                    rect = CurrentCellOver.Rectangle;
                    last = CurrentCellOver;
                }

                if (cell != null)
                {
                    cell.MouseOverPen = new Pen(BorderColorOnOver, 1);
                    rect = rect != Rectangle.Empty ? Rectangle.Union(rect, cell.Rectangle) : cell.Rectangle;
                    CurrentCellOver = cell;
                }

                OnCellOver(cell, last);

                if (rect != Rectangle.Empty)
                {
                    Invalidate(rect);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            CeldaMapa cell = get_Celda(e.Location);

            if (cell != null)
            {
                mapa_celda_retenida = mapa_celda_abajo = cell;
            }
            mapa_raton_abajo = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mapa_raton_abajo = false;
            CeldaMapa cell = get_Celda(e.Location);

            if (mapa_celda_retenida != null)
            {
                OnCellClicked(mapa_celda_retenida, e.Button);
                mapa_celda_retenida = null;
            }
            base.OnMouseUp(e);
        }

        public CeldaMapa get_Celda(Point p) => celdas.FirstOrDefault(cell => cell.esta_En_Rectangulo(new Rectangle(p.X - RealCellWidth, p.Y - RealCellHeight, RealCellWidth, RealCellHeight)) && PointInPoly(p, cell.Puntos));
        public CeldaMapa get_Celda(int id) => celdas.FirstOrDefault(cell => cell.id == id);
        public void Invalidate(CeldaMapa celda) => Invalidate(celda.Rectangle);

        public void Invalidate(params CeldaMapa[] cells)
        {
            if (cells.Length == 0)
                base.Invalidate();
            else
                Invalidate(cells as IEnumerable<CeldaMapa>);
        }

        public void Invalidate(IEnumerable<CeldaMapa> celdas) => Invalidate(celdas.Select(entry => entry.Rectangle).Aggregate(Rectangle.Union));

        public static bool PointInPoly(Point p, Point[] poly)
        {
            int x_antiguo, y_antiguo, x_nuevo, y_nuevo, x1, y1, x2, y2;
            bool inside = false;

            if (poly.Length < 3)
                return false;

            x_antiguo = poly[poly.Length - 1].X;
            y_antiguo = poly[poly.Length - 1].Y;

            poly.ToList().ForEach(t =>
            {
                x_nuevo = t.X;
                y_nuevo = t.Y;

                if (x_nuevo > x_antiguo)
                {
                    x1 = x_antiguo;
                    x2 = x_nuevo;
                    y1 = y_antiguo;
                    y2 = y_nuevo;
                }
                else
                {
                    x1 = x_nuevo;
                    x2 = x_antiguo;
                    y1 = y_nuevo;
                    y2 = y_antiguo;
                }

                if ((x_nuevo < p.X) == (p.X <= x_antiguo) && (p.Y - (long)y1) * (x2 - x1) < (y2 - (long)y1) * (p.X - x1))
                {
                    inside = !inside;
                }
                x_antiguo = x_nuevo;
                y_antiguo = y_nuevo;
            });
            return inside;
        }
    }
}