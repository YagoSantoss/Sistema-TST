using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class AuthFormBase : PrototypeFormBase
    {
        protected RoundPanel StagePanel;
        protected RoundPanel CardPanel;
        private Label _footerLabel;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            WindowState = FormWindowState.Maximized;
        }

        protected void BuildAuthStage(int x, int y, int width, int height, bool gradient)
        {
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(900, 620);

            int stageWidth = ClientSize.Width > 0 ? ClientSize.Width : Screen.PrimaryScreen.WorkingArea.Width;
            int stageHeight = ClientSize.Height > 0 ? ClientSize.Height : Screen.PrimaryScreen.WorkingArea.Height;

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

            _footerLabel = UiBuilder.CenterLabel(
                "SENAC LARGO TREZE - 2026",
                0,
                stageHeight - 76,
                stageWidth,
                36,
                14F,
                FontStyle.Bold,
                Color.White
            );
            StagePanel.Controls.Add(_footerLabel);
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
            ReposicionarAuthStage();

            return CardPanel;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ReposicionarAuthStage();
        }

        protected void ReposicionarAuthStage()
        {
            if (StagePanel == null)
                return;

            StagePanel.Location = new Point(0, 0);
            StagePanel.Size = ClientSize;

            if (CardPanel != null)
            {
                int cardX = (StagePanel.Width - CardPanel.Width) / 2;
                int cardY = (StagePanel.Height - CardPanel.Height) / 2;

                if (cardX < 24)
                    cardX = 24;

                if (cardY < 96)
                    cardY = 96;

                CardPanel.Location = new Point(cardX, cardY);
            }

            if (_footerLabel != null)
            {
                _footerLabel.Location = new Point(0, StagePanel.Height - 76);
                _footerLabel.Size = new Size(StagePanel.Width, 36);
            }
        }

        protected static void TogglePassword(TextBox box)
        {
            box.UseSystemPasswordChar = !box.UseSystemPasswordChar;
        }
    }
}
