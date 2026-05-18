using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class LoginForm : AuthFormBase
    {
        private bool telaMontada = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            if (telaMontada)
                return;

            MontarTelaLogin();
            telaMontada = true;
        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            lblErro.Visible = false;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                lblErro.Text = "Informe usuário e senha para continuar.";
                lblErro.Visible = true;
                return;
            }

            bool loginValido;
            try
            {
                loginValido = AppState.ValidateLogin(txtUsuario.Text.Trim(), txtSenha.Text);
            }
            catch (Exception ex)
            {
                lblErro.Text = "Não foi possível conectar ao MySQL.";
                lblErro.Visible = true;
                MessageBox.Show("Verifique se o XAMPP/MySQL está aberto e se o banco sistema_tst foi criado.\n\n" + ex.Message, "Conexão MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!loginValido)
            {
                lblErro.Text = "Usuário ou senha não encontrados no banco.";
                lblErro.Visible = true;
                return;
            }

            AppNavigator.Show(new EmployeesForm());
        }

        private void LinkEsqueci_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new ForgotPasswordForm());
        }

        private void LinkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppNavigator.Show(new RegisterForm());
        }

        private void BtnMostrarSenha_Click(object sender, EventArgs e)
        {
            TogglePassword(txtSenha);
            btnMostrarSenha.Text = txtSenha.UseSystemPasswordChar ? "Ver" : "Ocultar";
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnEntrar_Click(sender, EventArgs.Empty);
            }
        }
    }
}
