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
        private const int EmSetmargins = 0x00D3;
        private const int EcLeftmargin = 0x0001;
        private const int EcRightmargin = 0x0002;
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
            Height = 34;
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
            SetInnerMargins();
            UpdateRegion();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetInnerMargins();
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

        private void SetInnerMargins()
        {
            if (!IsHandleCreated)
                return;

            int left = 10;
            int right = 10;
            int value = (right << 16) | left;
            SendMessage(Handle, EmSetmargins, (IntPtr)(EcLeftmargin | EcRightmargin), (IntPtr)value);
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

            int borderWidth = Focused ? 2 : 1;
            int inset = borderWidth;
            Rectangle rect = new Rectangle(inset, inset, Width - (inset * 2) - 1, Height - (inset * 2) - 1);

            using (Graphics graphics = Graphics.FromHwnd(Handle))
            using (GraphicsPath path = RoundPanel.RoundedPath(rect, Radius))
            using (Pen pen = new Pen(Focused ? FocusBorderColor : BorderColor, borderWidth))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawPath(pen, path);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
