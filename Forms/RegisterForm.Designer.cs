using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RegisterForm
    {
        private CueTextBox txtNome;
        private CueTextBox txtEmail;
        private CueTextBox txtSenha;
        private CueTextBox txtConfirmarSenha;
        private RoundButton btnCadastrar;
        private RoundButton btnMostrarSenha;
        private RoundButton btnMostrarConfirmacao;
        private LinkLabel linkLogin;
        private Label lblErro;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildAuthStage(76, 176, 1190, 520, true);
            CreateAuthCard(472, 75, 380, 520);

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Crie sua conta", 0, 30, 380, 36, 19F, FontStyle.Bold, UiColors.Navy));

            CardPanel.Controls.Add(UiBuilder.Label("Nome completo", 40, 92, 300, 18, 9F, FontStyle.Bold, UiColors.Navy));
            txtNome = UiBuilder.TextBox("Digite seu nome completo", 40, 116, 300);
            CardPanel.Controls.Add(txtNome);

            CardPanel.Controls.Add(UiBuilder.Label("E-mail", 40, 164, 300, 18, 9F, FontStyle.Bold, UiColors.Navy));
            txtEmail = UiBuilder.TextBox("Digite seu e-mail", 40, 188, 300);
            CardPanel.Controls.Add(txtEmail);

            CardPanel.Controls.Add(UiBuilder.Label("Senha", 40, 236, 300, 18, 9F, FontStyle.Bold, UiColors.Navy));
            txtSenha = UiBuilder.TextBox("Crie sua senha", 40, 260, 246);
            txtSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtSenha);

            btnMostrarSenha = UiBuilder.Button("Ver", 296, 260, 44, 30, Color.White, UiColors.AccentBlue);
            btnMostrarSenha.BorderColor = UiColors.Border;
            btnMostrarSenha.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarSenha.Click += BtnMostrarSenha_Click;
            CardPanel.Controls.Add(btnMostrarSenha);

            CardPanel.Controls.Add(UiBuilder.Label("Confirmar senha", 40, 308, 300, 18, 9F, FontStyle.Bold, UiColors.Navy));
            txtConfirmarSenha = UiBuilder.TextBox("Confirme sua senha", 40, 332, 246);
            txtConfirmarSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtConfirmarSenha);

            btnMostrarConfirmacao = UiBuilder.Button("Ver", 296, 332, 44, 30, Color.White, UiColors.AccentBlue);
            btnMostrarConfirmacao.BorderColor = UiColors.Border;
            btnMostrarConfirmacao.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarConfirmacao.Click += BtnMostrarConfirmacao_Click;
            CardPanel.Controls.Add(btnMostrarConfirmacao);

            btnCadastrar = UiBuilder.Button("Cadastrar", 40, 404, 300, 34, UiColors.Orange, Color.White);
            btnCadastrar.Click += BtnCadastrar_Click;
            CardPanel.Controls.Add(btnCadastrar);

            linkLogin = UiBuilder.Link("Ja tem uma conta? Faca login", 40, 452, 300, 22);
            linkLogin.LinkClicked += LinkLogin_LinkClicked;
            CardPanel.Controls.Add(linkLogin);

            lblErro = UiBuilder.CenterLabel("", 40, 70, 300, 18, 7.5F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            CardPanel.BringToFront();

            ResumeLayout(false);
        }
    }
}
