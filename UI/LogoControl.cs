using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class LogoControl : Control
    {
        private static readonly Image logoImage = LoadLogoImage();

        public LogoControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(42, 42);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle outer = new Rectangle(1, 1, Width - 3, Height - 3);

            if (logoImage != null)
            {
                using (GraphicsPath clip = new GraphicsPath())
                {
                    clip.AddEllipse(outer);
                    GraphicsState state = e.Graphics.Save();
                    e.Graphics.SetClip(clip);
                    e.Graphics.DrawImage(logoImage, outer);
                    e.Graphics.Restore(state);
                }

                using (Pen orange = new Pen(UiColors.Orange, 3))
                using (Pen white = new Pen(Color.White, 1))
                {
                    e.Graphics.DrawEllipse(orange, outer);
                    e.Graphics.DrawEllipse(white, new Rectangle(4, 4, Width - 9, Height - 9));
                }

                return;
            }

            using (SolidBrush brush = new SolidBrush(UiColors.Orange))
            {
                e.Graphics.FillEllipse(brush, outer);
            }

            using (SolidBrush brush = new SolidBrush(UiColors.Navy))
            {
                e.Graphics.FillEllipse(brush, new Rectangle(5, 5, Width - 11, Height - 11));
            }

            using (Pen pen = new Pen(Color.White, 2))
            {
                e.Graphics.DrawEllipse(pen, outer);
            }

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(252, 190, 74)))
            {
                e.Graphics.FillPie(brush, new Rectangle(10, 9, 22, 18), 200, 140);
            }

            using (SolidBrush brush = new SolidBrush(Color.White))
            using (Font font = new Font("Segoe UI", 7F, FontStyle.Bold))
            {
                StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString("TST", font, brush, new RectangleF(0, 18, Width, 17), format);
            }
        }

        private static Image LoadLogoImage()
        {
            string[] candidates =
            {
                Path.Combine(Directory.GetCurrentDirectory(), "Assets", "logo.png"),
                Path.Combine(Application.StartupPath, "Assets", "logo.png"),
                Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.png"),
                Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Assets", "logo.png")
            };

            foreach (string candidate in candidates)
            {
                try
                {
                    string fullPath = Path.GetFullPath(candidate);
                    if (File.Exists(fullPath))
                    {
                        using (Image image = Image.FromFile(fullPath))
                        {
                            return new Bitmap(image);
                        }
                    }
                }
                catch
                {
                    // If the designer cannot load the asset, the drawn fallback logo is used.
                }
            }

            return null;
        }
    }
}
