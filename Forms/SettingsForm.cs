using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Configurações salvas.", "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Close();
            AppNavigator.Show(new LoginForm());
        }
    }
}
