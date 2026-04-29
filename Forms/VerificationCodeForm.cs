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
                lblErro.Text = "Codigo invalido.";
                lblErro.Visible = true;
                return;
            }

            AppNavigator.Show(new ResetPasswordForm());
        }

        private void LinkReenviar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Codigo reenviado para o e-mail cadastrado.", "Codigo de verificacao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
