using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class AuthFormBase : PrototypeFormBase
    {
        protected RoundPanel StagePanel;
        protected RoundPanel CardPanel;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            WindowState = FormWindowState.Maximized;
        }

        protected void BuildAuthStage(int x, int y, int width, int height, bool gradient)
        {
            WindowState = FormWindowState.Maximized;

            int stageWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int stageHeight = Screen.PrimaryScreen.WorkingArea.Height;

            StagePanel = new RoundPanel
            {
                Location = new Point(0, 0),
                Size = new Size(stageWidth, stageHeight),
                Radius = 0,
                FillColor = UiColors.PanelBlue,
                GradientColor = gradient ? Color.FromArgb(0, 76, 124) : Color.Empty,
                BorderColor = Color.Transparent
            };

            Controls.Add(StagePanel);

            LogoControl logo = new LogoControl
            {
                Location = new Point(32, 31),
                Size = new Size(40, 40)
            };

            StagePanel.Controls.Add(logo);

            StagePanel.Controls.Add(
                UiBuilder.Label("SISTEMA TST", 90, 30, 210, 27, 14F, FontStyle.Bold, Color.White)
            );

            StagePanel.Controls.Add(
                UiBuilder.Label("LARGO TREZE", 90, 58, 220, 27, 14F, FontStyle.Bold, Color.White)
            );

            StagePanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "SENAC LARGO TREZE - 2026",
                    0,
                    stageHeight - 76,
                    stageWidth,
                    36,
                    14F,
                    FontStyle.Bold,
                    Color.White
                )
            );
        }

        protected RoundPanel CreateAuthCard(int x, int y, int width, int height)
        {
            int stageWidth = StagePanel.Width;
            int stageHeight = StagePanel.Height;

            int cardX = (stageWidth - width) / 2;
            int cardY = (stageHeight - height) / 2;

            CardPanel = new RoundPanel
            {
                Location = new Point(cardX, cardY),
                Size = new Size(width, height),
                Radius = 10,
                FillColor = Color.White,
                BorderColor = Color.Transparent
            };

            StagePanel.Controls.Add(CardPanel);

            return CardPanel;
        }

        protected static void TogglePassword(TextBox box)
        {
            box.UseSystemPasswordChar = !box.UseSystemPasswordChar;
        }
    }
}