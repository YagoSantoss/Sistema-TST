using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ForgotPasswordForm : AuthFormBase
    {
        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private void BtnRecuperar_Click(object sender, EventArgs e)
        {
            lblErro.Visible = false;
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                lblErro.Text = "Informe o e-mail cadastrado.";
                lblErro.Visible = true;
                return;
            }

            AppState.BeginPasswordRecovery(txtEmail.Text.Trim());
            MessageBox.Show("Se o e-mail estiver cadastrado, enviaremos as instrucoes de recuperacao.", "Recuperacao de senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppNavigator.Show(new VerificationCodeForm());
        }

        private void LinkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new LoginForm());
        }
    }
}
