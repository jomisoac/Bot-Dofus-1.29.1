using Bot_Dofus_1._29._1.Controles.ControlMapa.Celdas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Controles.ControlMapa.Animaciones
{
    class MovimientoAnimacion : IDisposable
    {
        public int entidad_id { get; private set; }
        public List<CeldaMapa> path { get; private set; }
        public PointF actual_punto { get; private set; }
        public TipoAnimaciones tipo_animacion { get; private set; }
        private int index_frame;
        private int tiempo_por_frame;
        private Timer timer;
        private List<PointF> frames;

        public event Action<MovimientoAnimacion> finalizado;

        public MovimientoAnimacion(int _entidad_id, IEnumerable<CeldaMapa> _path, int duration, TipoAnimaciones _tipo_animacion)
        {
            entidad_id = _entidad_id;
            path = new List<CeldaMapa>(_path);
            tipo_animacion = _tipo_animacion;
            timer = new Timer(realizar_Animacion, null, Timeout.Infinite, Timeout.Infinite);

            iniciar_Frames();
            tiempo_por_frame = duration / frames.Count;
            index_frame = 0;
        }

        private void iniciar_Frames()
        {
            frames = new List<PointF>();

            for (int i = 0; i < path.Count - 1; i++)
                frames.AddRange(get_Punto_Entre_Dos(path[i].Centro, path[i + 1].Centro, 3));
        }

        public void iniciar()
        {
            actual_punto = frames[index_frame];
            timer.Change(tiempo_por_frame, tiempo_por_frame);
        }

        private PointF[] get_Punto_Entre_Dos(PointF p1, PointF p2, int cantidad)
        {
            PointF[] puntos = new PointF[cantidad];
            float y_diferencia = p2.Y - p1.Y, xdiff = p2.X - p1.X;
            double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double x, y;

            cantidad--;

            for (double i = 0; i < cantidad; i++)
            {
                y = slope == 0 ? 0 : y_diferencia * (i / cantidad);
                x = slope == 0 ? xdiff * (i / cantidad) : y / slope;
                puntos[(int)i] = new PointF((float)Math.Round(x) + p1.X, (float)Math.Round(y) + p1.Y);
            }

            puntos[cantidad] = p2;
            return puntos;
        }

        private void realizar_Animacion(object state)
        {
            index_frame++;
            actual_punto = frames[index_frame];

            if (index_frame == frames.Count - 1)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                finalizado?.Invoke(this);
            }
        }

        public void Dispose()
        {
            path.Clear();
            timer.Dispose();

            path = null;
            timer = null;
        }
    }
}
