using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ResetPasswordForm : AuthFormBase
    {
        public ResetPasswordForm()
        {
            InitializeComponent();
        }

        private void BtnRedefinir_Click(object sender, EventArgs e)
        {
            lblErro.Visible = false;

            if (string.IsNullOrWhiteSpace(txtNovaSenha.Text) || string.IsNullOrWhiteSpace(txtConfirmarSenha.Text))
            {
                ShowError("Informe e confirme a nova senha.");
                return;
            }

            if (txtNovaSenha.Text != txtConfirmarSenha.Text)
            {
                ShowError("As senhas não conferem.");
                return;
            }

            AppState.ResetPendingPassword(txtNovaSenha.Text);
            MessageBox.Show("Senha redefinida com sucesso.", "Redefinir senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppNavigator.Show(new LoginForm());
        }

        private void LinkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new LoginForm());
        }

        private void BtnMostrarNova_Click(object sender, EventArgs e)
        {
            TogglePassword(txtNovaSenha);
            btnMostrarNova.Text = txtNovaSenha.UseSystemPasswordChar ? "Ver" : "Ocultar";
        }

        private void BtnMostrarConfirmacao_Click(object sender, EventArgs e)
        {
            TogglePassword(txtConfirmarSenha);
            btnMostrarConfirmacao.Text = txtConfirmarSenha.UseSystemPasswordChar ? "Ver" : "Ocultar";
        }

        private void ShowError(string text)
        {
            lblErro.Text = text;
            lblErro.Visible = true;
        }
    }
}
