using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatBasicForm : DashboardFormBase
    {
        private readonly int _catId;

        public CatBasicForm()
            : this(0)
        {
        }

        public CatBasicForm(int catId)
        {
            _catId = catId;
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarCat();
                MessageBox.Show("CAT salva com sucesso.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppNavigator.Show(new CatListForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel salvar a CAT no MySQL.\n\n" + ex.Message, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatListForm());
        }

        private void NovoEmpregado_Click(object sender, EventArgs e)
        {
            using (EmployeeCadastroForm form = new EmployeeCadastroForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MontarConteudoCat();
                }
            }
        }

        private void TabDados_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatBasicForm());
        }

        private void TabTestemunhas_Click(object sender, EventArgs e)
        {
            string mensagem;
            if (!ValidarObrigatorios(out mensagem))
            {
                MessageBox.Show(mensagem, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SalvarRascunho();
            AppNavigator.Show(new CatWitnessesForm());
        }

        private void TabComplementares_Click(object sender, EventArgs e)
        {
            string mensagem;
            if (!ValidarObrigatorios(out mensagem))
            {
                MessageBox.Show(mensagem, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SalvarRascunho();
            AppNavigator.Show(new CatMedicalForm(_catId));
        }
    }
}
