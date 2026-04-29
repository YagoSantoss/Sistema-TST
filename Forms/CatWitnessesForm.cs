using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatWitnessesForm : DashboardFormBase
    {
        public CatWitnessesForm()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CAT salva com testemunhas.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatListForm());
        }

        private void BtnAdicionarTestemunha_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nova testemunha adicionada.", "Testemunhas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TabDados_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatBasicForm());
        }

        private void TabTestemunhas_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatWitnessesForm());
        }

        private void TabComplementares_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatMedicalForm());
        }
    }
}
