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
            MessageBox.Show("Indicadores atualizados com os dados do banco.", "Painel de Gestao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MontarConteudoEsocial();
        }

        private void BtnEnviarTodos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Acompanhe as CATs geradas e seus resultados no painel.", "Painel de Gestao", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
