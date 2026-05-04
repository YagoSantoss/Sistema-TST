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
            CreateAuthCard(490, 118, 360, 360);

            string email = string.IsNullOrWhiteSpace(AppState.PendingRecoveryEmail) ? "seu e-mail" : AppState.PendingRecoveryEmail;

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Codigo enviado", 0, 30, 360, 30, 18F, FontStyle.Bold, UiColors.Navy));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("Digite o codigo de 6 digitos enviado para", 30, 72, 300, 18, 8.5F, FontStyle.Regular, UiColors.BodyText));
            CardPanel.Controls.Add(UiBuilder.CenterLabel(email, 30, 92, 300, 18, 8F, FontStyle.Bold, UiColors.BodyText));
            CardPanel.Controls.Add(UiBuilder.Label("Codigo de verificacao", 40, 132, 280, 18, 9F, FontStyle.Bold, UiColors.Navy));

            codeBoxes = new CueTextBox[6];
            for (int i = 0; i < 6; i++)
            {
                CueTextBox box = new CueTextBox
                {
                    Cue = "-",
                    Location = new Point(40 + i * 47, 160),
                    Size = new Size(36, 40),
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Center,
                    MaxLength = 1
                };
                box.TextChanged += CodeBox_TextChanged;
                codeBoxes[i] = box;
                CardPanel.Controls.Add(box);
            }

            lblErro = UiBuilder.CenterLabel("", 40, 214, 280, 18, 7.5F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            btnVerificar = UiBuilder.Button("Verificar codigo", 40, 244, 280, 34, Color.FromArgb(55, 113, 184), Color.White);
            btnVerificar.Click += BtnVerificar_Click;
            CardPanel.Controls.Add(btnVerificar);

            linkReenviar = UiBuilder.Link("Reenviar codigo", 40, 292, 280, 22);
            linkReenviar.LinkClicked += LinkReenviar_LinkClicked;
            CardPanel.Controls.Add(linkReenviar);

            ResumeLayout(false);
        }
    }
}
