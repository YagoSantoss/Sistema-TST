using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RiskFactorsForm : DashboardFormBase
    {
        private readonly int _id;
        private string _episSelecionados = string.Empty;
        private bool _alterandoEpi;

        public RiskFactorsForm()
            : this(0)
        {
        }

        public RiskFactorsForm(int id)
        {
            _id = id;
            InitializeComponent();
            CarregarRegistro();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarRegistro();
                MessageBox.Show("Fator de risco salvo com sucesso.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppNavigator.Show(new RiskFactorsListForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar no MySQL.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new RiskFactorsListForm());
        }

        private void BtnAdicionarAmbiente_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new WorkEnvironmentsForm());
        }

        private void SalvarRegistro()
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            ComboItem ambiente = cmbAmbiente.SelectedItem as ComboItem;

            if (empregado == null || empregado.Id <= 0)
                throw new InvalidOperationException("Selecione o empregado.");

            if (ambiente == null || ambiente.Id <= 0)
                throw new InvalidOperationException("Selecione o ambiente de trabalho.");

            if (string.IsNullOrWhiteSpace(txtDataAvaliacao.Text))
                throw new InvalidOperationException("Informe a data de avaliação.");

            if (string.IsNullOrWhiteSpace(txtInicioExposicao.Text))
                throw new InvalidOperationException("Informe o início da exposição.");

            if (!ValidationHelper.IsValidDate(txtDataAvaliacao.Text))
                throw new InvalidOperationException("Informe a data de avaliação no formato dd/mm/aaaa.");

            if (!ValidationHelper.IsValidDate(txtInicioExposicao.Text))
                throw new InvalidOperationException("Informe o início da exposição no formato dd/mm/aaaa.");

            if (!string.IsNullOrWhiteSpace(txtFimExposicao.Text) && !ValidationHelper.IsValidDate(txtFimExposicao.Text))
                throw new InvalidOperationException("Informe o fim da exposição no formato dd/mm/aaaa.");

            if (string.IsNullOrWhiteSpace(txtAgente.Text))
                throw new InvalidOperationException("Informe o agente de risco.");

            if (string.IsNullOrWhiteSpace(txtDescricaoAtividades.Text))
                throw new InvalidOperationException("Informe a descrição das atividades.");

            if (cbUsaEpi.Checked && string.IsNullOrWhiteSpace(_episSelecionados))
                throw new InvalidOperationException("Selecione pelo menos um EPI utilizado pelo trabalhador.");

            CadastrosRepository.SaveFatorRisco(new RiskFactorRecord
            {
                Id = _id,
                EmpregadoId = empregado.Id,
                AmbienteId = ambiente.Id,
                TipoFator = cmbTipoFator.Text.Trim(),
                Agente = txtAgente.Text.Trim(),
                Intensidade = txtIntensidade.Text.Trim(),
                TecnicaMedicao = txtTecnicaMedicao.Text.Trim(),
                DataAvaliacao = txtDataAvaliacao.Text.Trim(),
                InicioExposicao = txtInicioExposicao.Text.Trim(),
                FimExposicao = txtFimExposicao.Text.Trim(),
                DescricaoAtividades = txtDescricaoAtividades.Text.Trim(),
                UsaEpi = cbUsaEpi.Checked,
                EpiEficaz = cbUsaEpi.Checked && cbEpiEficaz.Checked,
                EpisSelecionados = cbUsaEpi.Checked ? _episSelecionados : string.Empty
            });
        }

        private void CarregarRegistro()
        {
            if (_id <= 0)
                return;

            try
            {
                RiskFactorRecord risco = CadastrosRepository.GetFatorRisco(_id);
                if (risco == null)
                    return;

                SelecionarComboPorId(cmbEmpregado, risco.EmpregadoId);
                SelecionarComboPorId(cmbAmbiente, risco.AmbienteId);
                cmbTipoFator.Text = risco.TipoFator;
                txtAgente.Text = risco.Agente;
                txtIntensidade.Text = risco.Intensidade;
                txtTecnicaMedicao.Text = risco.TecnicaMedicao;
                txtDataAvaliacao.Text = risco.DataAvaliacao;
                txtInicioExposicao.Text = risco.InicioExposicao;
                txtFimExposicao.Text = risco.FimExposicao;
                txtDescricaoAtividades.Text = risco.DescricaoAtividades;
                _episSelecionados = risco.EpisSelecionados ?? string.Empty;
                _alterandoEpi = true;
                cbUsaEpi.Checked = risco.UsaEpi;
                _alterandoEpi = false;
                cbEpiEficaz.Checked = risco.EpiEficaz;
                AtualizarResumoEpi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar o fator de risco.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelecionarComboPorId(ComboBox combo, int? id)
        {
            if (!id.HasValue || combo == null)
                return;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                ComboItem item = combo.Items[i] as ComboItem;
                if (item != null && item.Id == id.Value)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void CbUsaEpi_CheckedChanged(object sender, EventArgs e)
        {
            if (_alterandoEpi)
                return;

            if (!cbUsaEpi.Checked)
            {
                _episSelecionados = string.Empty;
                cbEpiEficaz.Checked = false;
                AtualizarResumoEpi();
                return;
            }

            using (EpiSelectionForm modal = new EpiSelectionForm(_episSelecionados))
            {
                if (modal.ShowDialog(this) == DialogResult.OK)
                {
                    _episSelecionados = modal.SelectedEpis;
                }
                else if (string.IsNullOrWhiteSpace(_episSelecionados))
                {
                    _alterandoEpi = true;
                    cbUsaEpi.Checked = false;
                    _alterandoEpi = false;
                }
            }

            AtualizarResumoEpi();
        }

        private void AtualizarResumoEpi()
        {
            if (lblEpisSelecionados == null)
                return;

            if (!cbUsaEpi.Checked || string.IsNullOrWhiteSpace(_episSelecionados))
                lblEpisSelecionados.Text = "EPIs selecionados: nenhum";
            else
                lblEpisSelecionados.Text = "EPIs selecionados: " + _episSelecionados;
        }
    }
}
