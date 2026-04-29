using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RiskFactorsForm : DashboardFormBase
    {
        public RiskFactorsForm()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("S-2240 salvo e pronto para integração eSocial.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new EmployeesForm());
        }

        private void BtnAdicionarFator_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nova linha de fator de risco adicionada.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRemoverFator_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fator de risco removido.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnAdicionarAmbiente_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cadastro rápido de ambiente de trabalho.", "Ambiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
