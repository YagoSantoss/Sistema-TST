using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
                MessageBox.Show("Não foi possível excluir no MySQL.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaTST", "Relatorios");
                Directory.CreateDirectory(pasta);
                string arquivo = Path.Combine(pasta, "empregados_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");

                StringBuilder csv = new StringBuilder();
                csv.AppendLine("Matricula;Nome;CPF;Setor;Cargo;Admissao;Vencimento ASO;Status ASO;Médico");

                foreach (EmpregadoRecord empregado in CadastrosRepository.GetEmpregados())
                {
                    csv.AppendLine(string.Join(";",
                        Csv(empregado.Matricula),
                        Csv(empregado.Nome),
                        Csv(empregado.Cpf),
                        Csv(empregado.Setor),
                        Csv(empregado.Cargo),
                        Csv(empregado.DataAdmissao),
                        Csv(empregado.DataVencimentoAso),
                        Csv(empregado.StatusAso),
                        Csv(empregado.MedicoNome)));
                }

                File.WriteAllText(arquivo, csv.ToString(), Encoding.UTF8);
                System.Diagnostics.Process.Start(arquivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível exportar os empregados.\n\n" + ex.Message, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFiltros_Click(object sender, EventArgs e)
        {
            BtnBuscarEmpregados_Click(sender, EventArgs.Empty);
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            _empregadosSelecionados.Clear();
            MontarConteudoEmpregados();
        }

        private void BtnImportar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Importar empregados do Excel/CSV";
                dialog.Filter = "CSV editado no Excel|*.csv|Todos os arquivos|*.*";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    List<EmpregadoRecord> empregados = LerEmpregadosCsv(dialog.FileName);
                    int total = CadastrosRepository.ImportEmpregados(empregados);
                    _empregadosSelecionados.Clear();
                    MontarConteudoEmpregados();
                    MessageBox.Show(total + " empregado(s) importado(s) ou atualizado(s).", "Importar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível importar a planilha.\n\n" + ex.Message, "Importar CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void SelecionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (_montandoConteudo)
                return;

            CheckBox check = sender as CheckBox;
            if (check == null)
                return;

            try
            {
                List<EmpregadoRecord> empregados = ObterEmpregadosFiltrados();

                foreach (EmpregadoRecord empregado in empregados)
                {
                    if (check.Checked)
                        _empregadosSelecionados.Add(empregado.Id);
                    else
                        _empregadosSelecionados.Remove(empregado.Id);
                }

                MontarConteudoEmpregados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível selecionar os empregados.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TodosEmpregadosFiltradosSelecionados()
        {
            try
            {
                List<EmpregadoRecord> empregados = ObterEmpregadosFiltrados();
                if (empregados.Count == 0)
                    return false;

                foreach (EmpregadoRecord empregado in empregados)
                {
                    if (!_empregadosSelecionados.Contains(empregado.Id))
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private int ObterPrimeiroSelecionado()
        {
            foreach (int id in _empregadosSelecionados)
                return id;

            return 0;
        }

        private string Csv(string value)
        {
            string text = value ?? string.Empty;
            if (text.Contains(";") || text.Contains("\"") || text.Contains("\n"))
                return "\"" + text.Replace("\"", "\"\"") + "\"";

            return text;
        }

        private List<EmpregadoRecord> LerEmpregadosCsv(string arquivo)
        {
            string[] linhas = File.ReadAllLines(arquivo, Encoding.UTF8);
            if (linhas.Length < 2)
                throw new InvalidOperationException("A planilha precisa ter cabecalho e pelo menos um empregado.");

            char separador = linhas[0].Contains(";") ? ';' : ',';
            List<string> cabecalho = ParseCsvLine(linhas[0], separador);
            Dictionary<string, int> colunas = new Dictionary<string, int>();

            for (int i = 0; i < cabecalho.Count; i++)
                colunas[NormalizarColuna(cabecalho[i])] = i;

            List<EmpregadoRecord> empregados = new List<EmpregadoRecord>();
            for (int i = 1; i < linhas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(linhas[i]))
                    continue;

                List<string> valores = ParseCsvLine(linhas[i], separador);
                EmpregadoRecord empregado = new EmpregadoRecord
                {
                    Matricula = ValorCsv(valores, colunas, "MATRICULA"),
                    Nome = ValorCsv(valores, colunas, "NOME"),
                    Cpf = ValorCsv(valores, colunas, "CPF"),
                    Setor = ValorCsv(valores, colunas, "SETOR"),
                    Cargo = ValorCsv(valores, colunas, "CARGO"),
                    DataAdmissao = ValorCsv(valores, colunas, "ADMISSAO"),
                    DataVencimentoAso = ValorCsv(valores, colunas, "VENCIMENTOASO"),
                    StatusAso = ValorCsv(valores, colunas, "STATUSASO")
                };

                if (!string.IsNullOrWhiteSpace(empregado.Matricula) ||
                    !string.IsNullOrWhiteSpace(empregado.Nome) ||
                    !string.IsNullOrWhiteSpace(empregado.Cpf))
                {
                    empregados.Add(empregado);
                }
            }

            return empregados;
        }

        private string ValorCsv(List<string> valores, Dictionary<string, int> colunas, string coluna)
        {
            int indice;
            if (!colunas.TryGetValue(coluna, out indice) || indice < 0 || indice >= valores.Count)
                return string.Empty;

            return valores[indice].Trim();
        }

        private string NormalizarColuna(string coluna)
        {
            return (coluna ?? string.Empty)
                .Replace(" ", string.Empty)
                .Replace(".", string.Empty)
                .Replace("_", string.Empty)
                .Replace("-", string.Empty)
                .ToUpperInvariant();
        }

        private List<string> ParseCsvLine(string linha, char separador)
        {
            List<string> valores = new List<string>();
            StringBuilder atual = new StringBuilder();
            bool aspas = false;

            for (int i = 0; i < linha.Length; i++)
            {
                char c = linha[i];
                if (c == '"')
                {
                    if (aspas && i + 1 < linha.Length && linha[i + 1] == '"')
                    {
                        atual.Append('"');
                        i++;
                    }
                    else
                    {
                        aspas = !aspas;
                    }
                }
                else if (c == separador && !aspas)
                {
                    valores.Add(atual.ToString());
                    atual.Clear();
                }
                else
                {
                    atual.Append(c);
                }
            }

            valores.Add(atual.ToString());
            return valores;
        }
    }
}
