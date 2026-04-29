using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class RoundButton : Button
    {
        public int Radius { get; set; } = 8;
        public Color BorderColor { get; set; } = Color.Transparent;
        public Color HoverColor { get; set; } = Color.Empty;
        private bool hovering;

        public RoundButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ForeColor = Color.White;
            BackColor = UiColors.AccentBlue;
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            hovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            hovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(ResolveParentBackColor());

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fill = hovering && HoverColor != Color.Empty ? HoverColor : BackColor;
            using (GraphicsPath path = RoundPanel.RoundedPath(rect, Radius))
            using (SolidBrush brush = new SolidBrush(fill))
            {
                pevent.Graphics.FillPath(brush, path);

                if (BorderColor != Color.Transparent)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        pevent.Graphics.DrawPath(pen, path);
                    }
                }
            }

            TextRenderer.DrawText(
                pevent.Graphics,
                Text,
                Font,
                rect,
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private Color ResolveParentBackColor()
        {
            Control current = Parent;
            while (current != null)
            {
                if (current.BackColor != Color.Transparent)
                {
                    return current.BackColor;
                }

                current = current.Parent;
            }

            return Color.White;
        }
    }
}
