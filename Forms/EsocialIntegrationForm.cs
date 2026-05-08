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
            MessageBox.Show("Indicadores atualizados com os dados do banco.", "Integracao eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MontarConteudoEsocial();
        }

        private void BtnEnviarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                int total = CadastrosRepository.RegistrarTransmissoesEsocial();
                MessageBox.Show(total == 0 ? "Nao ha eventos SST cadastrados para transmitir." : total + " transmissao(oes) registradas no log.", "Integracao eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MontarConteudoEsocial();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel registrar as transmissoes.\n\n" + ex.Message, "Integracao eSocial", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
