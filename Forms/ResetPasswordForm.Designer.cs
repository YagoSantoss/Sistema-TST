using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ResetPasswordForm
    {
        private CueTextBox txtNovaSenha;
        private CueTextBox txtConfirmarSenha;
        private RoundButton btnMostrarNova;
        private RoundButton btnMostrarConfirmacao;
        private RoundButton btnRedefinir;
        private LinkLabel linkLogin;
        private Label lblErro;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildAuthStage(76, 176, 1135, 495, false);
            CreateAuthCard(480, 126, 250, 270);

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Crie sua nova senha", 0, 24, 250, 28, 13F, FontStyle.Bold, UiColors.Navy));
            CardPanel.Controls.Add(UiBuilder.CenterLabel("Insira sua nova senha abaixo.", 0, 54, 250, 18, 7.5F, FontStyle.Regular, Color.Gray));

            CardPanel.Controls.Add(UiBuilder.Label("Nova Senha", 25, 88, 200, 18, 8F, FontStyle.Bold, UiColors.BodyText));
            txtNovaSenha = UiBuilder.TextBox("Crie sua nova senha", 25, 108, 160);
            txtNovaSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtNovaSenha);
            btnMostrarNova = UiBuilder.Button("Ver", 189, 108, 36, 30, Color.White, UiColors.AccentBlue);
            btnMostrarNova.BorderColor = UiColors.Border;
            btnMostrarNova.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarNova.Click += BtnMostrarNova_Click;
            CardPanel.Controls.Add(btnMostrarNova);

            CardPanel.Controls.Add(UiBuilder.Label("Confirmar Nova Senha", 25, 148, 200, 18, 8F, FontStyle.Bold, UiColors.BodyText));
            txtConfirmarSenha = UiBuilder.TextBox("Digite a nova senha novamente", 25, 168, 160);
            txtConfirmarSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtConfirmarSenha);
            btnMostrarConfirmacao = UiBuilder.Button("Ver", 189, 168, 36, 30, Color.White, UiColors.AccentBlue);
            btnMostrarConfirmacao.BorderColor = UiColors.Border;
            btnMostrarConfirmacao.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarConfirmacao.Click += BtnMostrarConfirmacao_Click;
            CardPanel.Controls.Add(btnMostrarConfirmacao);

            btnRedefinir = UiBuilder.Button("Redefinir Senha", 25, 220, 200, 32, UiColors.Orange, Color.White);
            btnRedefinir.Click += BtnRedefinir_Click;
            CardPanel.Controls.Add(btnRedefinir);

            linkLogin = UiBuilder.Link("Voltar ao Login", 25, 248, 200, 22);
            linkLogin.TextAlign = ContentAlignment.MiddleRight;
            linkLogin.LinkClicked += LinkLogin_LinkClicked;
            CardPanel.Controls.Add(linkLogin);

            lblErro = UiBuilder.CenterLabel("", 25, 198, 200, 18, 7.5F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            ResumeLayout(false);
        }
    }
}
