using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ExamTypesForm : DashboardFormBase
    {
        public ExamTypesForm()
        {
            InitializeComponent();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            AbrirCadastro(false, 0);
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            int id = control != null && control.Tag is int ? (int)control.Tag : 0;
            AbrirCadastro(true, id);
        }

        private void AbrirCadastro(bool edicao, int id)
        {
            using (CadastroBaseForm form = new CadastroBaseForm(CadastroBaseTipo.TipoExame, edicao, id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MontarConteudoTiposExame();
                }
            }
        }
    }
}
