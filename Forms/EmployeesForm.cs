using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class EmployeesForm : DashboardFormBase
    {
        public EmployeesForm()
        {
            InitializeComponent();
        }

        private void BtnInserir_Click(object sender, EventArgs e)
        {
            using (EmployeeCadastroForm form = new EmployeeCadastroForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MontarConteudoEmpregados();
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edição do empregado selecionado.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Empregado removido da fila do protótipo.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Relatório exportado para planilha.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnFiltros_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Filtros avançados: setor, cargo, vencimento de ASO e status.", "Filtros", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRegistrarAso_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            int empregadoId = control != null && control.Tag is int ? (int)control.Tag : 0;

            if (empregadoId > 0)
            {
                AppNavigator.Show(new AsoHistoryForm(empregadoId));
                return;
            }

            AppNavigator.Show(new AsoForm());
        }

        private void BtnAgendar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Agendamento de ASO criado para o colaborador.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
