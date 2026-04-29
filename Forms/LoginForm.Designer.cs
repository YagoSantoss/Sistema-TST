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
            CreateAuthCard(490, 126, 240, 300);

            CardPanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "Bem-vindo",
                    0,
                    26,
                    240,
                    30,
                    14F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            CardPanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "Acesse o Sistema TST - Largo Treze",
                    0,
                    58,
                    240,
                    18,
                    7.5F,
                    FontStyle.Regular,
                    Color.Gray
                )
            );

            CardPanel.Controls.Add(
                UiBuilder.Label(
                    "E-mail",
                    20,
                    92,
                    190,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            txtUsuario = UiBuilder.TextBox("Digite seu e-mail cadastrado", 20, 112, 200);
            CardPanel.Controls.Add(txtUsuario);

            CardPanel.Controls.Add(
                UiBuilder.Label(
                    "Senha",
                    20,
                    150,
                    190,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            txtSenha = UiBuilder.TextBox("Digite sua senha", 20, 170, 158);
            txtSenha.UseSystemPasswordChar = true;
            CardPanel.Controls.Add(txtSenha);

            btnMostrarSenha = UiBuilder.Button(
                "Ver",
                182,
                170,
                38,
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
                Location = new Point(20, 207),
                Size = new Size(110, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = UiColors.BodyText,
                BackColor = Color.Transparent
            };

            CardPanel.Controls.Add(chkLembrar);

            btnEntrar = UiBuilder.Button(
                "Entrar",
                20,
                237,
                200,
                30,
                UiColors.Orange,
                Color.White
            );

            btnEntrar.Click += BtnEntrar_Click;
            CardPanel.Controls.Add(btnEntrar);

            linkCadastro = UiBuilder.Link("Criar conta", 20, 268, 90, 20);
            linkCadastro.TextAlign = ContentAlignment.MiddleLeft;
            linkCadastro.LinkClicked += LinkCadastro_LinkClicked;
            CardPanel.Controls.Add(linkCadastro);

            linkEsqueci = UiBuilder.Link("Esqueci a senha", 116, 268, 104, 20);
            linkEsqueci.TextAlign = ContentAlignment.MiddleRight;
            linkEsqueci.LinkClicked += LinkEsqueci_LinkClicked;
            CardPanel.Controls.Add(linkEsqueci);

            lblErro = UiBuilder.CenterLabel(
                "",
                0,
                286,
                240,
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
