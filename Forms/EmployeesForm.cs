using System;
using System.Collections.Generic;
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
            if (_empregadosSelecionados.Count == 0)
            {
                MessageBox.Show("Selecione um empregado para editar.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_empregadosSelecionados.Count > 1)
            {
                MessageBox.Show("Para editar, selecione apenas um empregado por vez.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int empregadoId = ObterPrimeiroSelecionado();
            using (EmployeeCadastroForm form = new EmployeeCadastroForm(empregadoId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _empregadosSelecionados.Clear();
                    MontarConteudoEmpregados();
                }
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (_empregadosSelecionados.Count == 0)
            {
                MessageBox.Show("Selecione um ou mais empregados para excluir.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacao = MessageBox.Show(
                "Deseja excluir os empregados selecionados?",
                "Empregados",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacao != DialogResult.Yes)
                return;

            try
            {
                CadastrosRepository.DeleteEmpregados(new List<int>(_empregadosSelecionados));
                _empregadosSelecionados.Clear();
                MontarConteudoEmpregados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel excluir no MySQL.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Relatorio exportado para planilha.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnFiltros_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Filtros avancados: setor, cargo, vencimento de ASO e status.", "Filtros", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnBuscarEmpregados_Click(object sender, EventArgs e)
        {
            _termoBuscaEmpregados = txtBuscaEmpregados == null ? string.Empty : txtBuscaEmpregados.Text.Trim();
            _empregadosSelecionados.Clear();
            MontarConteudoEmpregados();
        }

        private void TxtBuscaEmpregados_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            BtnBuscarEmpregados_Click(sender, EventArgs.Empty);
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

        private void EmployeeCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check == null || !(check.Tag is int))
                return;

            int empregadoId = (int)check.Tag;

            if (check.Checked)
                _empregadosSelecionados.Add(empregadoId);
            else
                _empregadosSelecionados.Remove(empregadoId);

            if (check.Parent != null)
                check.Parent.BackColor = check.Checked ? System.Drawing.Color.FromArgb(255, 244, 229) : System.Drawing.Color.White;
        }

        private int ObterPrimeiroSelecionado()
        {
            foreach (int id in _empregadosSelecionados)
                return id;

            return 0;
        }
    }
}
