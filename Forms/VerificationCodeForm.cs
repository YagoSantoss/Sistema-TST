using System;
using System.Linq;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class VerificationCodeForm : AuthFormBase
    {
        public VerificationCodeForm()
        {
            InitializeComponent();
        }

        private void CodeBox_TextChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text.Length > 1)
            {
                box.Text = box.Text.Substring(0, 1);
                box.SelectionStart = 1;
            }

            if (box.Text.Length == 1)
            {
                SelectNextControl(box, true, true, true, true);
            }
        }

        private void BtnVerificar_Click(object sender, EventArgs e)
        {
            string code = string.Concat(codeBoxes.Select(b => b.Text.Trim()));
            lblErro.Visible = false;

            if (!AppState.CheckVerificationCode(code))
            {
                lblErro.Text = "Código inválido.";
                lblErro.Visible = true;
                return;
            }

            AppNavigator.Show(new ResetPasswordForm());
        }

        private void LinkReenviar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblErro.Visible = false;

            try
            {
                AppState.ResendPasswordRecoveryCode();
                MessageBox.Show("Código reenviado para o e-mail cadastrado.", "Código de verificação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblErro.Text = "Não foi possível reenviar o código.";
                lblErro.Visible = true;
                MessageBox.Show(ex.Message, "Código de verificação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkEmailErrado_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new ForgotPasswordForm());
        }
    }
}
