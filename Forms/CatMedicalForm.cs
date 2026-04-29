using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatMedicalForm : DashboardFormBase
    {
        public CatMedicalForm()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CAT salva com dados médicos complementares.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatListForm());
        }

        private void BtnAdicionarMedico_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new DoctorsForm());
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
