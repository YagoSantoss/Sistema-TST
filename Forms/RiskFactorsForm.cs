using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RiskFactorsForm : DashboardFormBase
    {
        private readonly int _id;

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
                MessageBox.Show("Nao foi possivel salvar no MySQL.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                throw new InvalidOperationException("Informe a data de avaliacao.");

            if (string.IsNullOrWhiteSpace(txtInicioExposicao.Text))
                throw new InvalidOperationException("Informe o inicio da exposicao.");

            if (!ValidationHelper.IsValidDate(txtDataAvaliacao.Text))
                throw new InvalidOperationException("Informe a data de avaliacao no formato dd/mm/aaaa.");

            if (!ValidationHelper.IsValidDate(txtInicioExposicao.Text))
                throw new InvalidOperationException("Informe o inicio da exposicao no formato dd/mm/aaaa.");

            if (!string.IsNullOrWhiteSpace(txtFimExposicao.Text) && !ValidationHelper.IsValidDate(txtFimExposicao.Text))
                throw new InvalidOperationException("Informe o fim da exposicao no formato dd/mm/aaaa.");

            if (string.IsNullOrWhiteSpace(txtAgente.Text))
                throw new InvalidOperationException("Informe o agente de risco.");

            if (string.IsNullOrWhiteSpace(txtDescricaoAtividades.Text))
                throw new InvalidOperationException("Informe a descricao das atividades.");

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
                EpiEficaz = cbEpiEficaz.Checked
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
                cbUsaEpi.Checked = risco.UsaEpi;
                cbEpiEficaz.Checked = risco.EpiEficaz;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel carregar o fator de risco.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
