using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class VerificationCodeForm
    {
        private CueTextBox[] codeBoxes;
        private RoundButton btnVerificar;
        private LinkLabel linkReenviar;
        private Label lblErro;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildAuthStage(76, 176, 1190, 520, false);
            CreateAuthCard(490, 118, 250, 285);

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Código enviado para o", 0, 22, 250, 26, 14F, FontStyle.Bold, UiColors.Navy));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("e-mail", 0, 45, 250, 26, 14F, FontStyle.Bold, UiColors.Navy));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("Digite o código de 6 dígitos que enviamos", 20, 78, 210, 18, 8F, FontStyle.Regular, UiColors.BodyText));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("para exemplo@email.com.", 20, 94, 210, 18, 8F, FontStyle.Bold, UiColors.BodyText));
            CardPanel.Controls.Add(UiBuilder.Label("Código de verificação", 20, 130, 210, 18, 8F, FontStyle.Bold, UiColors.Navy));

            codeBoxes = new CueTextBox[6];
            for (int i = 0; i < 6; i++)
            {
                CueTextBox box = new CueTextBox
                {
                    Cue = "-",
                    Location = new Point(20 + i * 37, 154),
                    Size = new Size(30, 36),
                    Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Center,
                    MaxLength = 1
                };
                box.TextChanged += CodeBox_TextChanged;
                codeBoxes[i] = box;
                CardPanel.Controls.Add(box);
            }

            linkReenviar = UiBuilder.Link("Reenviar código", 20, 198, 210, 22);
            linkReenviar.LinkClicked += LinkReenviar_LinkClicked;
            CardPanel.Controls.Add(linkReenviar);

            btnVerificar = UiBuilder.Button("Verificar código", 20, 242, 210, 32, Color.FromArgb(55, 113, 184), Color.White);
            btnVerificar.Click += BtnVerificar_Click;
            CardPanel.Controls.Add(btnVerificar);

            lblErro = UiBuilder.CenterLabel("", 20, 220, 210, 18, 7.2F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            ResumeLayout(false);
        }
    }
}
