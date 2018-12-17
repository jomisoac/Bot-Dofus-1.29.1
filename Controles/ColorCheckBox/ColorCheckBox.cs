using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Controles.ColorCheckBox
{
    public class ColorCheckBox : CheckBox
    {
        public ColorCheckBox() => SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.High;

            base.OnPaint(e);

            using (SolidBrush pincel = new SolidBrush(BackColor))
            {
                g.FillRectangle(pincel, new Rectangle(0, 0, Width - 2, Height));
            }

            if (Checked)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLines(new Point[]
                    {
                        new Point(2, Height / 2),
                        new Point(Width / 3, Height - 3),
                        new Point(Width - 2, Height / 3)
                    });

                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            if (!Enabled)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(120, Color.Gray)))
                {
                    g.FillRectangle(brush, 0, 0, Width, Height);
                }
            }
        }
    }
}
