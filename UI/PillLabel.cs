using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class PillLabel : Label
    {
        public int Radius { get; set; } = 12;
        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Transparent;

        public PillLabel()
        {
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = RoundPanel.RoundedPath(rect, Radius))
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                e.Graphics.FillPath(brush, path);
                if (BorderColor != Color.Transparent)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                rect,
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
