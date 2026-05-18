using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RegisterForm : AuthFormBase
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            lblErro.Visible = false;

            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmarSenha.Text))
            {
                ShowError("Preencha todos os campos obrigatórios.");
                return;
            }

            if (!ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                ShowError("Informe um e-mail válido.");
                return;
            }

            if (txtSenha.Text != txtConfirmarSenha.Text)
            {
                ShowError("As senhas não conferem.");
                return;
            }

            try
            {
                AppState.Register(txtNome.Text.Trim(), txtEmail.Text.Trim(), txtSenha.Text);
            }
            catch (Exception ex)
            {
                ShowError("Não foi possível salvar no MySQL.");
                MessageBox.Show("Verifique se o XAMPP/MySQL está aberto e se o banco sistema_tst foi criado.\n\n" + ex.Message, "Conexão MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Conta cadastrada com sucesso. Faça login para continuar.", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppNavigator.Show(new LoginForm());
        }

        private void LinkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new LoginForm());
        }

        private void BtnMostrarSenha_Click(object sender, EventArgs e)
        {
            TogglePassword(txtSenha);
            btnMostrarSenha.Text = txtSenha.UseSystemPasswordChar ? "Ver" : "Ocultar";
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
