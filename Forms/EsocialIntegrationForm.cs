using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class EsocialIntegrationForm : DashboardFormBase
    {
        public EsocialIntegrationForm()
        {
            InitializeComponent();
        }

        private void BtnSincronizar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sincronização com eSocial concluída.", "Integração eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEnviarTodos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Eventos pendentes enviados para o eSocial.", "Integração eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnGerarS2240_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new RiskFactorsForm());
        }

        private void BtnGerarS2210_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatBasicForm());
        }

        private void BtnGerarS2220_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new AsoForm());
        }
    }
}
