using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatWitnessesForm : DashboardFormBase
    {
        private readonly int _catId;

        public CatWitnessesForm()
            : this(0)
        {
        }

        public CatWitnessesForm(int catId)
        {
            _catId = catId;
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            List<CatTestemunhaRecord> testemunhas = ColetarTestemunhas();
            SalvarTestemunhasNoRascunho(testemunhas);

            if (_catId > 0)
                CadastrosRepository.SaveCatTestemunhas(_catId, testemunhas);

            MessageBox.Show("Testemunhas salvas. Continue para os dados complementares da CAT.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AppNavigator.Show(new CatMedicalForm(_catId));
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new CatListForm());
        }

        private void BtnAdicionarTestemunha_Click(object sender, EventArgs e)
        {
            _testemunhas = ColetarTestemunhas();
            while (_testemunhas.Count < _witnessInputs.Count)
                _testemunhas.Add(new CatTestemunhaRecord());

            if (_testemunhas.Count >= 2)
            {
                MessageBox.Show("A CAT permite cadastrar ate duas testemunhas.", "Testemunhas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _testemunhas.Add(new CatTestemunhaRecord());
            MontarConteudoTestemunhas();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            SalvarTestemunhasNoRascunho(ColetarTestemunhas());
            AppNavigator.Show(new CatBasicForm(_catId));
        }

        private void TabDados_Click(object sender, EventArgs e)
        {
            SalvarTestemunhasNoRascunho(ColetarTestemunhas());
            AppNavigator.Show(new CatBasicForm(_catId));
        }

        private void TabTestemunhas_Click(object sender, EventArgs e)
        {
            SalvarTestemunhasNoRascunho(ColetarTestemunhas());
            AppNavigator.Show(new CatWitnessesForm(_catId));
        }

        private void TabComplementares_Click(object sender, EventArgs e)
        {
            SalvarTestemunhasNoRascunho(ColetarTestemunhas());
            AppNavigator.Show(new CatMedicalForm(_catId));
        }

        private void CarregarTestemunhasIniciais()
        {
            if (_catId > 0)
                _testemunhas = CadastrosRepository.GetCatTestemunhas(_catId);
            else if (CatDraftState.Current != null && CatDraftState.Current.Testemunhas != null)
                _testemunhas = new List<CatTestemunhaRecord>(CatDraftState.Current.Testemunhas);

            if (_testemunhas.Count == 0)
                _testemunhas.Add(new CatTestemunhaRecord());
        }

        private List<CatTestemunhaRecord> ColetarTestemunhas()
        {
            List<CatTestemunhaRecord> testemunhas = new List<CatTestemunhaRecord>();

            foreach (WitnessInputs input in _witnessInputs)
            {
                CatTestemunhaRecord testemunha = new CatTestemunhaRecord
                {
                    Nome = input.Nome.Text.Trim(),
                    Cpf = input.Cpf.Text.Trim(),
                    Telefone = input.Telefone.Text.Trim(),
                    Endereco = input.Endereco.Text.Trim()
                };

                if (!string.IsNullOrWhiteSpace(testemunha.Nome) ||
                    !string.IsNullOrWhiteSpace(testemunha.Cpf) ||
                    !string.IsNullOrWhiteSpace(testemunha.Telefone) ||
                    !string.IsNullOrWhiteSpace(testemunha.Endereco))
                {
                    testemunhas.Add(testemunha);
                }
            }

            return testemunhas;
        }

        private void SalvarTestemunhasNoRascunho(List<CatTestemunhaRecord> testemunhas)
        {
            if (_catId > 0)
                return;

            if (CatDraftState.Current == null)
                CatDraftState.Current = new CatDraft();

            CatDraftState.Current.Testemunhas = testemunhas;
        }

        private sealed class WitnessInputs
        {
            public CueTextBox Nome { get; set; }
            public CueTextBox Cpf { get; set; }
            public CueTextBox Telefone { get; set; }
            public CueTextBox Endereco { get; set; }
        }
    }
}
