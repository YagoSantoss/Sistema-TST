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

            CreateAuthCard(472, 75, 290, 420);

            CardPanel.Controls.Add(UiBuilder.CenterLabel("Crie sua conta", 0, 28, 290, 34, 16F, FontStyle.Bold, UiColors.Navy));

            CardPanel.Controls.Add(UiBuilder.Label("Nome completo", 25, 80, 230, 18, 8F, FontStyle.Bold, UiColors.Navy));
            txtNome = UiBuilder.TextBox("Digite seu nome completo", 25, 100, 240);
            CardPanel.Controls.Add(txtNome);

            CardPanel.Controls.Add(UiBuilder.Label("E-mail", 25, 143, 230, 18, 8F, FontStyle.Bold, UiColors.Navy));
            txtEmail = UiBuilder.TextBox("Digite seu e-mail", 25, 163, 240);
            CardPanel.Controls.Add(txtEmail);

            CardPanel.Controls.Add(UiBuilder.Label("Senha", 25, 206, 230, 18, 8F, FontStyle.Bold, UiColors.Navy));
            txtSenha = UiBuilder.TextBox("Crie sua senha", 25, 226, 198);
            txtSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtSenha);

            btnMostrarSenha = UiBuilder.Button("Ver", 227, 226, 38, 30, Color.White, UiColors.AccentBlue);
            btnMostrarSenha.BorderColor = UiColors.Border;
            btnMostrarSenha.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarSenha.Click += BtnMostrarSenha_Click;
            CardPanel.Controls.Add(btnMostrarSenha);

            CardPanel.Controls.Add(UiBuilder.Label("Confirmar senha", 25, 269, 230, 18, 8F, FontStyle.Bold, UiColors.Navy));
            txtConfirmarSenha = UiBuilder.TextBox("Confirme sua senha", 25, 289, 198);
            txtConfirmarSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtConfirmarSenha);

            btnMostrarConfirmacao = UiBuilder.Button("Ver", 227, 289, 38, 30, Color.White, UiColors.AccentBlue);
            btnMostrarConfirmacao.BorderColor = UiColors.Border;
            btnMostrarConfirmacao.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarConfirmacao.Click += BtnMostrarConfirmacao_Click;
            CardPanel.Controls.Add(btnMostrarConfirmacao);

            btnCadastrar = UiBuilder.Button("Cadastrar", 25, 345, 240, 32, UiColors.Orange, Color.White);
            btnCadastrar.Click += BtnCadastrar_Click;
            CardPanel.Controls.Add(btnCadastrar);

            linkLogin = UiBuilder.Link("Já tem uma conta? Faça login", 25, 383, 240, 22);
            linkLogin.LinkClicked += LinkLogin_LinkClicked;
            CardPanel.Controls.Add(linkLogin);

            lblErro = UiBuilder.CenterLabel("", 25, 62, 240, 18, 7.5F, FontStyle.Bold, UiColors.Red);
            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            CardPanel.BringToFront();

            ResumeLayout(false);
        }
    }
}