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
            if (!ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                lblErro.Text = "Informe o e-mail cadastrado.";
                lblErro.Visible = true;
                return;
            }

            try
            {
                AppState.BeginPasswordRecovery(txtEmail.Text.Trim());
                MessageBox.Show("Enviamos um codigo de recuperacao para o e-mail informado.", "Recuperacao de senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppNavigator.Show(new VerificationCodeForm());
            }
            catch (Exception ex)
            {
                lblErro.Text = "Nao foi possivel enviar o codigo.";
                lblErro.Visible = true;
                MessageBox.Show(ex.Message, "Recuperacao de senha", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new LoginForm());
        }
    }
}
