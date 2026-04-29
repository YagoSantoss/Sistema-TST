using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class RoundPanel : Panel
    {
        public int Radius { get; set; } = 10;
        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Transparent;
        public int BorderThickness { get; set; } = 1;
        public Color GradientColor { get; set; } = Color.Empty;

        public RoundPanel()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }

        protected override void OnResize(System.EventArgs eventargs)
        {
            base.OnResize(eventargs);
            using (GraphicsPath path = RoundedPath(ClientRectangle, Radius))
            {
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = RoundedPath(rect, Radius))
            {
                if (GradientColor != Color.Empty)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, FillColor, GradientColor, LinearGradientMode.Horizontal))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(FillColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }

                if (BorderColor != Color.Transparent && BorderThickness > 0)
                {
                    using (Pen pen = new Pen(BorderColor, BorderThickness))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }

        public static GraphicsPath RoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter <= 0)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
