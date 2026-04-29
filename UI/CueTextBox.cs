using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class CueTextBox : TextBox
    {
        private const int EmSetcuebanner = 0x1501;
        private const int WmPaint = 0x000F;
        private const int WmSetfocus = 0x0007;
        private const int WmKillfocus = 0x0008;
        private string cue = string.Empty;

        public int Radius { get; set; } = 8;
        public Color BorderColor { get; set; } = UiColors.Border;
        public Color FocusBorderColor { get; set; } = UiColors.AccentBlue;

        public CueTextBox()
        {
            AutoSize = false;
            BorderStyle = BorderStyle.None;
            BackColor = Color.White;
            ForeColor = UiColors.BodyText;
            Font = new Font("Segoe UI", 9F);
            Height = 32;
            Padding = new Padding(10, 6, 10, 6);
        }

        public string Cue
        {
            get { return cue; }
            set
            {
                cue = value ?? string.Empty;
                SetCue();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetCue();
            UpdateRegion();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WmPaint || m.Msg == WmSetfocus || m.Msg == WmKillfocus)
            {
                DrawRoundedBorder();
            }
        }

        private void SetCue()
        {
            if (IsHandleCreated)
            {
                SendMessage(Handle, EmSetcuebanner, (IntPtr)1, cue);
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (GraphicsPath path = RoundPanel.RoundedPath(new Rectangle(0, 0, Width, Height), Radius))
            {
                Region = new Region(path);
            }
        }

        private void DrawRoundedBorder()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (Graphics graphics = Graphics.FromHwnd(Handle))
            using (GraphicsPath path = RoundPanel.RoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
            using (Pen pen = new Pen(Focused ? FocusBorderColor : BorderColor, Focused ? 2 : 1))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawPath(pen, path);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
    }
}
