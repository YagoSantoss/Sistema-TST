using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ForgotPasswordForm
    {
        private CueTextBox txtEmail;
        private RoundButton btnRecuperar;
        private LinkLabel linkLogin;
        private Label lblErro;

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 520);
            Name = "ForgotPasswordForm";
            Text = "Recuperar Senha - Sistema TST Largo Treze";
            StartPosition = FormStartPosition.CenterScreen;

            MontarTelaRecuperarSenha();

            ResumeLayout(false);
        }

        private void MontarTelaRecuperarSenha()
        {
            BuildAuthStage(76, 176, 1190, 520, false);
            CreateAuthCard(480, 126, 360, 300);

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Esqueci a senha", 0, 32, 360, 34, 18F, FontStyle.Bold, UiColors.Navy));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("Informe seu e-mail para receber o codigo", 30, 72, 300, 18, 8.5F, FontStyle.Regular, UiColors.BodyText));

            CardPanel.Controls.Add(UiBuilder.Label("E-mail", 40, 112, 280, 18, 9F, FontStyle.Bold, UiColors.Navy));
            txtEmail = UiBuilder.TextBox("Digite seu e-mail cadastrado", 40, 136, 280);
            CardPanel.Controls.Add(txtEmail);

            lblErro = UiBuilder.CenterLabel("", 40, 170, 280, 18, 7.5F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            btnRecuperar = UiBuilder.Button("Enviar codigo", 40, 200, 280, 34, UiColors.Orange, Color.White);
            btnRecuperar.Click += BtnRecuperar_Click;
            CardPanel.Controls.Add(btnRecuperar);

            linkLogin = UiBuilder.Link("Voltar para o login", 40, 246, 280, 22);
            linkLogin.LinkClicked += LinkLogin_LinkClicked;
            CardPanel.Controls.Add(linkLogin);

            CardPanel.BringToFront();
        }
    }
}
