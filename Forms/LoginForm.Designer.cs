using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class LoginForm
    {
        private CueTextBox txtUsuario;
        private CueTextBox txtSenha;
        private CheckBox chkLembrar;
        private RoundButton btnEntrar;
        private RoundButton btnMostrarSenha;
        private LinkLabel linkEsqueci;
        private LinkLabel linkCadastro;
        private Label lblErro;

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1342, 696);
            Name = "LoginForm";
            Text = "Login - Sistema TST Largo Treze";
            StartPosition = FormStartPosition.CenterScreen;

            ResumeLayout(false);
        }

        private void MontarTelaLogin()
        {
            KeyPreview = true;
            KeyDown += LoginForm_KeyDown;

            BuildAuthStage(76, 176, 1190, 520, false);
            CreateAuthCard(490, 126, 340, 390);

            CardPanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "Bem-vindo",
                    0,
                    30,
                    340,
                    34,
                    18F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            CardPanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "Acesse o Sistema TST - Largo Treze",
                    0,
                    68,
                    340,
                    18,
                    9F,
                    FontStyle.Regular,
                    Color.Gray
                )
            );

            CardPanel.Controls.Add(
                UiBuilder.Label(
                    "E-mail",
                    32,
                    112,
                    270,
                    18,
                    9F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            txtUsuario = UiBuilder.TextBox("Digite seu e-mail cadastrado", 32, 136, 276);
            CardPanel.Controls.Add(txtUsuario);

            CardPanel.Controls.Add(
                UiBuilder.Label(
                    "Senha",
                    32,
                    184,
                    270,
                    18,
                    9F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            txtSenha = UiBuilder.TextBox("Digite sua senha", 32, 208, 224);
            txtSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtSenha);

            btnMostrarSenha = UiBuilder.Button(
                "Ver",
                264,
                208,
                44,
                30,
                Color.White,
                UiColors.AccentBlue
            );

            btnMostrarSenha.BorderColor = UiColors.Border;
            btnMostrarSenha.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnMostrarSenha.Click += BtnMostrarSenha_Click;
            CardPanel.Controls.Add(btnMostrarSenha);

            chkLembrar = new CheckBox
            {
                Text = "Lembrar-me",
                Location = new Point(32, 248),
                Size = new Size(110, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = UiColors.BodyText,
                BackColor = Color.Transparent
            };

            CardPanel.Controls.Add(chkLembrar);

            btnEntrar = UiBuilder.Button(
                "Entrar",
                32,
                284,
                276,
                34,
                UiColors.Orange,
                Color.White
            );

            btnEntrar.Click += BtnEntrar_Click;
            CardPanel.Controls.Add(btnEntrar);

            linkCadastro = UiBuilder.Link("Criar conta", 32, 326, 120, 20);
            linkCadastro.TextAlign = ContentAlignment.MiddleLeft;
            linkCadastro.LinkClicked += LinkCadastro_LinkClicked;
            CardPanel.Controls.Add(linkCadastro);

            linkEsqueci = UiBuilder.Link("Esqueci a senha", 178, 326, 130, 20);
            linkEsqueci.TextAlign = ContentAlignment.MiddleRight;
            linkEsqueci.LinkClicked += LinkEsqueci_LinkClicked;
            CardPanel.Controls.Add(linkEsqueci);

            lblErro = UiBuilder.CenterLabel(
                "",
                0,
                352,
                340,
                16,
                7.5F,
                FontStyle.Bold,
                UiColors.Red
            );

            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);
        }
    }
}
