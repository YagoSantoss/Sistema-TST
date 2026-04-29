using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class SettingsForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnSair;
        private RoundButton btnLogout;

        private void InitializeComponent()
        {
            SuspendLayout();

            ClientSize = new Size(520, 320);
            Text = "Configurações";
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = new Size(484, 284),
                Radius = 10,
                FillColor = Color.White,
                BorderColor = UiColors.Border
            };

            Controls.Add(card);

            card.Controls.Add(
                UiBuilder.Label(
                    "Configurações do Sistema",
                    20,
                    18,
                    320,
                    24,
                    13F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            card.Controls.Add(
                UiBuilder.Label(
                    "Competência ativa",
                    20,
                    62,
                    180,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.BodyText
                )
            );

            CueTextBox txtCompetencia = UiBuilder.TextBox("03/2026", 20, 84, 180);
            card.Controls.Add(txtCompetencia);

            card.Controls.Add(
                UiBuilder.Label(
                    "Unidade",
                    230,
                    62,
                    200,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.BodyText
                )
            );

            CueTextBox txtUnidade = UiBuilder.TextBox("SENAC Largo Treze", 230, 84, 220);
            card.Controls.Add(txtUnidade);

            CheckBox chkNotificacoes = new CheckBox
            {
                Text = "Notificar vencimentos de ASO e pendências eSocial",
                Checked = true,
                Location = new Point(20, 132),
                Size = new Size(420, 24),
                BackColor = Color.Transparent,
                ForeColor = UiColors.BodyText,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular)
            };

            card.Controls.Add(chkNotificacoes);

            Panel line = new Panel
            {
                Location = new Point(0, 210),
                Size = new Size(484, 1),
                BackColor = UiColors.Border
            };

            card.Controls.Add(line);

            btnLogout = UiBuilder.SmallButton(
                "Sair da conta",
                20,
                232,
                105,
                UiColors.Red,
                Color.White
            );

            btnLogout.Click += BtnLogout_Click;
            card.Controls.Add(btnLogout);

            btnSair = UiBuilder.SmallButton(
                "Cancelar",
                310,
                232,
                75,
                Color.White,
                UiColors.BodyText
            );

            btnSair.BorderColor = UiColors.Border;
            btnSair.Click += BtnSair_Click;
            card.Controls.Add(btnSair);

            btnSalvar = UiBuilder.SmallButton(
                "Salvar",
                395,
                232,
                70,
                UiColors.AccentBlue,
                Color.White
            );

            btnSalvar.Click += BtnSalvar_Click;
            card.Controls.Add(btnSalvar);

            ResumeLayout(false);
        }
    }
}