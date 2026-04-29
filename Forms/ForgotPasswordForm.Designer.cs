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
            CreateAuthCard(480, 126, 260, 225);

            CardPanel.Controls.Add(
                UiBuilder.CenterLabel(
                    "Esqueci a senha",
                    0,
                    28,
                    260,
                    32,
                    15F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            CardPanel.Controls.Add(
                UiBuilder.Label(
                    "E-mail",
                    25,
                    80,
                    210,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.Navy
                )
            );

            txtEmail = UiBuilder.TextBox(
                "Digite seu e-mail cadastrado",
                25,
                100,
                210
            );

            CardPanel.Controls.Add(txtEmail);

            lblErro = UiBuilder.CenterLabel(
                "",
                25,
                130,
                210,
                18,
                7.5F,
                FontStyle.Bold,
                UiColors.Red
            );

            lblErro.Visible = false;
            CardPanel.Controls.Add(lblErro);

            btnRecuperar = UiBuilder.Button(
                "Recuperar senha",
                25,
                155,
                210,
                32,
                UiColors.Orange,
                Color.White
            );

            btnRecuperar.Click += BtnRecuperar_Click;
            CardPanel.Controls.Add(btnRecuperar);

            linkLogin = UiBuilder.Link(
                "Voltar para o login",
                25,
                193,
                210,
                22
            );

            linkLogin.LinkClicked += LinkLogin_LinkClicked;
            CardPanel.Controls.Add(linkLogin);

            CardPanel.BringToFront();
        }
    }
}
