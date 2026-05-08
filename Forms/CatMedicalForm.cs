using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatMedicalForm : DashboardFormBase
    {
        private readonly int _catId;

        public CatMedicalForm()
            : this(0)
        {
        }

        public CatMedicalForm(int catId)
        {
            _catId = catId;
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                CatRecord cat = _catId > 0 ? CadastrosRepository.GetCat(_catId) : CriarCatDoRascunho();
                if (cat == null)
                    throw new InvalidOperationException("Preencha os dados cadastrais da CAT antes dos dados complementares.");

                cat.ParteCorpoAtingida = cmbParteCorpo.Text.Trim();
                cat.Lateralidade = cmbLateralidade.Text.Trim();
                cat.AgenteCausador = txtAgenteCausador.Text.Trim();
                cat.Cid10 = txtCid10.Text.Trim();
                cat.NaturezaLesao = txtNaturezaLesao.Text.Trim();
                cat.DuracaoTratamento = txtDuracaoTratamento.Text.Trim();
                cat.MedicoAssistente = txtMedicoAssistente.Text.Trim();
                cat.ObservacaoMedica = txtObservacaoMedica.Text.Trim();

                CadastrosRepository.SaveCat(cat);
                CatDraftState.Clear();
                MessageBox.Show("CAT salva com dados complementares.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppNavigator.Show(new CatListForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel salvar a CAT.\n\n" + ex.Message, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private CatRecord CriarCatDoRascunho()
        {
            CatDraft draft = CatDraftState.Current;
            if (draft == null || draft.EmpregadoId <= 0)
                return null;

            return new CatRecord
            {
                EmpregadoId = draft.EmpregadoId,
                DataAcidente = draft.DataAcidente,
                HoraAcidente = draft.HoraAcidente,
                DataComunicacao = string.IsNullOrWhiteSpace(draft.DataComunicacao) ? DateTime.Today.ToString("dd/MM/yyyy") : draft.DataComunicacao,
                DataObito = draft.DataObito,
                LocalAcidente = draft.LocalAcidente,
                Descricao = draft.Descricao,
                TipoCat = draft.TipoCat,
                Aposentado = draft.Aposentado,
                Area = draft.Area,
                FiliacaoPrevSocial = draft.FiliacaoPrevSocial,
                Emitente = draft.Emitente,
                TipoAcidente = draft.TipoAcidente,
                HorasTrabalhadasAntes = draft.HorasTrabalhadasAntes,
                HouveObito = draft.HouveObito,
                HouveAfastamento = draft.HouveAfastamento,
                RegistroPolicia = draft.RegistroPolicia,
                UltimoDiaTrabalho = draft.UltimoDiaTrabalho,
                CodificacaoAcidente = draft.CodificacaoAcidente,
                SituacaoGeradora = draft.SituacaoGeradora,
                CatEmitidaPor = draft.CatEmitidaPor,
                EspecificacaoLocal = draft.EspecificacaoLocal,
                TipoLogradouro = draft.TipoLogradouro,
                Numero = draft.Numero,
                TipoInscricao = draft.TipoInscricao,
                InscricaoEstabelecimento = draft.InscricaoEstabelecimento,
                Logradouro = draft.Logradouro,
                Municipio = draft.Municipio,
                Uf = draft.Uf,
                Bairro = draft.Bairro,
                Complemento = draft.Complemento,
                Cep = draft.Cep,
                CodigoPostal = draft.CodigoPostal,
                ObservacaoCat = draft.ObservacaoCat,
                Situacao = "Aberta",
                ResultadoAso = "Aguardando ASO"
            };
        }

        private void CarregarDadosComplementares()
        {
            if (_catId <= 0)
                return;

            CatRecord cat = CadastrosRepository.GetCat(_catId);
            if (cat == null)
                return;

            cmbParteCorpo.Text = cat.ParteCorpoAtingida;
            cmbLateralidade.Text = string.IsNullOrWhiteSpace(cat.Lateralidade) ? "Nao aplicavel" : cat.Lateralidade;
            txtAgenteCausador.Text = cat.AgenteCausador;
            txtCid10.Text = cat.Cid10;
            txtNaturezaLesao.Text = cat.NaturezaLesao;
            txtDuracaoTratamento.Text = cat.DuracaoTratamento;
            txtMedicoAssistente.Text = cat.MedicoAssistente;
            txtObservacaoMedica.Text = cat.ObservacaoMedica;
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
            AppNavigator.Show(new CatBasicForm(_catId));
        }

        private void TabTestemunhas_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatWitnessesForm());
        }

        private void TabComplementares_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatMedicalForm(_catId));
        }
    }
}
