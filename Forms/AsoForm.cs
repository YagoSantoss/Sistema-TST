using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class AsoForm : DashboardFormBase
    {
        public AsoForm()
        {
            InitializeComponent();
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrarAso();
                MessageBox.Show("ASO registrado com sucesso.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MontarConteudoAso();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível registrar o ASO no MySQL.\n\n" + ex.Message, "ASO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHistorico_Click(object sender, EventArgs e)
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            if (empregado == null || empregado.Id <= 0)
            {
                MessageBox.Show("Selecione o empregado para consultar o histórico de ASO.", "Histórico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppNavigator.Show(new AsoHistoryForm(empregado.Id));
        }

        private void BtnAdicionarExame_Click(object sender, EventArgs e)
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            ComboItem medico = cmbMedico.SelectedItem as ComboItem;

            if (empregado == null || empregado.Id <= 0)
            {
                MessageBox.Show("Selecione o empregado antes de adicionar o exame.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (medico == null || medico.Id <= 0)
            {
                MessageBox.Show("Selecione o médico responsável antes de adicionar o exame.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (AsoExamLinkForm form = new AsoExamLinkForm(empregado.Id, empregado.Text, medico.Text))
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.Exame != null)
                {
                    ComboItem cat = cmbCat.SelectedItem as ComboItem;
                    int empregadoId = empregado.Id;
                    int medicoId = medico.Id;
                    int catId = cat == null ? 0 : cat.Id;
                    string dataAso = txtDataAso.Text;
                    string tipoExame = cmbTipoExame.Text;
                    string observacoes = txtObservacoes.Text;
                    string resultado = resultadoSelecionado;

                    _examesSelecionados.Add(form.Exame);
                    MontarConteudoAso();
                    RestaurarSelecoes(empregadoId, medicoId, catId, dataAso, tipoExame, observacoes, resultado);
                }
            }
        }

        private void BtnAdicionarMedico_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new DoctorsForm());
        }

        private void Empregado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            PopularCats(empregado == null ? 0 : empregado.Id);
        }

        private void Cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem catItem = cmbCat.SelectedItem as ComboItem;
            if (catItem == null || catItem.Id <= 0)
                return;

            try
            {
                CatRecord cat = CadastrosRepository.GetCat(catItem.Id);
                if (cat == null)
                    return;

                cmbTipoExame.Text = "Retorno ao trabalho - Tabela 27";
                txtObservacoes.Text = "ASO de retorno vinculado a CAT " + cat.Id + " - acidente em " + cat.DataAcidente + ". " + cat.Descricao;
            }
            catch
            {
                cmbTipoExame.Text = "Retorno ao trabalho - Tabela 27";
            }
        }

        private void ResultadoApto_Click(object sender, EventArgs e)
        {
            SelecionarResultado("Apto");
        }

        private void ResultadoInapto_Click(object sender, EventArgs e)
        {
            SelecionarResultado("Inapto");
        }

        private void ResultadoApto_MouseDown(object sender, MouseEventArgs e)
        {
            SelecionarResultado("Apto");
        }

        private void ResultadoInapto_MouseDown(object sender, MouseEventArgs e)
        {
            SelecionarResultado("Inapto");
        }

        private void SelecionarResultado(string resultado)
        {
            resultadoSelecionado = resultado;

            if (cardApto == null || cardInapto == null)
                return;

            bool apto = resultado == "Apto";

            cardApto.FillColor = apto ? Color.FromArgb(211, 250, 226) : Color.White;
            cardApto.BorderColor = apto ? UiColors.Green : Color.FromArgb(235, 239, 244);
            cardApto.BorderThickness = apto ? 2 : 1;

            cardInapto.FillColor = apto ? Color.White : Color.FromArgb(255, 233, 232);
            cardInapto.BorderColor = apto ? Color.FromArgb(235, 239, 244) : UiColors.Red;
            cardInapto.BorderThickness = apto ? 1 : 2;

            cardApto.Invalidate();
            cardInapto.Invalidate();
        }

        private void PopularCats(int empregadoId)
        {
            if (cmbCat == null)
                return;

            cmbCat.Items.Clear();
            cmbCat.Items.Add(new ComboItem(0, "Sem CAT vinculada"));

            if (empregadoId > 0)
            {
                try
                {
                    foreach (CatRecord cat in CadastrosRepository.GetCatsPorEmpregado(empregadoId))
                    {
                        cmbCat.Items.Add(new ComboItem(cat.Id, "CAT " + cat.Id + " - " + cat.DataAcidente + " - " + cat.Situacao));
                    }
                }
                catch
                {
                    cmbCat.Items.Add(new ComboItem(0, "MySQL indisponível"));
                }
            }

            cmbCat.SelectedIndex = 0;
        }

        private void RestaurarSelecoes(int empregadoId, int medicoId, int catId, string dataAso, string tipoExame, string observacoes, string resultado)
        {
            SelecionarComboPorId(cmbEmpregado, empregadoId);
            PopularCats(empregadoId);
            SelecionarComboPorId(cmbCat, catId);
            SelecionarComboPorId(cmbMedico, medicoId);
            txtDataAso.Text = dataAso;
            cmbTipoExame.Text = tipoExame;
            txtObservacoes.Text = observacoes;
            SelecionarResultado(resultado);
        }

        private void SelecionarComboPorId(ComboBox combo, int id)
        {
            if (combo == null)
                return;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                ComboItem item = combo.Items[i] as ComboItem;
                if (item != null && item.Id == id)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void RegistrarAso()
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            ComboItem medico = cmbMedico.SelectedItem as ComboItem;
            ComboItem cat = cmbCat.SelectedItem as ComboItem;

            if (empregado == null || empregado.Id <= 0)
                throw new InvalidOperationException("Selecione o empregado.");

            if (medico == null || medico.Id <= 0)
                throw new InvalidOperationException("Selecione o médico responsável.");

            if (string.IsNullOrWhiteSpace(txtDataAso.Text))
                throw new InvalidOperationException("Informe a data do ASO no formato dd/mm/yyyy.");

            if (!ValidationHelper.IsValidDate(txtDataAso.Text))
                throw new InvalidOperationException("Informe a data do ASO no formato dd/mm/aaaa.");

            bool temCatVinculada = cat != null && cat.Id > 0;
            bool retornoTrabalho = !string.IsNullOrWhiteSpace(cmbTipoExame.Text) &&
                                   cmbTipoExame.Text.IndexOf("retorno", StringComparison.OrdinalIgnoreCase) >= 0;

            if (temCatVinculada && !retornoTrabalho)
                throw new InvalidOperationException("Para avaliar uma CAT, selecione o tipo de exame Retorno ao trabalho.");

            int asoId = CadastrosRepository.SaveAso(new AsoRecord
            {
                EmpregadoId = empregado.Id,
                MedicoId = medico.Id,
                CatId = temCatVinculada ? (int?)cat.Id : null,
                DataAso = txtDataAso.Text.Trim(),
                TipoExame = cmbTipoExame.Text.Trim(),
                Resultado = resultadoSelecionado,
                Observacoes = txtObservacoes.Text.Trim()
            });

            if (_examesSelecionados.Count > 0)
            {
                CadastrosRepository.SaveAsoExames(asoId, _examesSelecionados);
                _examesSelecionados.Clear();
            }
        }
    }
}
