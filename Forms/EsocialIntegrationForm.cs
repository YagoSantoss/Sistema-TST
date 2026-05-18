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
            MessageBox.Show("Indicadores atualizados com os dados do banco.", "Controle eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MontarConteudoEsocial();
        }

        private void BtnEnviarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                int total = CadastrosRepository.RegistrarTransmissoesEsocial();
                MessageBox.Show(total == 0 ? "Não há novos eventos SST para registrar. Os eventos já transmitidos continuam no log." : total + " novo(s) evento(s) registrados no log interno.", "Controle eSocial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MontarConteudoEsocial();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível registrar os eventos no log interno.\n\n" + ex.Message, "Controle eSocial", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
