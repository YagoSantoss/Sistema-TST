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

        private void BuscarCep_Click(object sender, EventArgs e)
        {
            try
            {
                CepLookupResult endereco = BrazilLookupApi.ConsultarCep(txtCep.Text);
                AplicarEndereco(endereco);
                MessageBox.Show("Endereco preenchido pelo ViaCEP.", "CEP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel consultar o CEP.\n\n" + ex.Message, "CEP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarCnpj_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.Equals(cmbTipoInscricao.Text, "CNPJ", StringComparison.OrdinalIgnoreCase))
                    cmbTipoInscricao.Text = "CNPJ";

                CnpjLookupResult empresa = BrazilLookupApi.ConsultarCnpj(txtInscricaoEstabelecimento.Text);

                txtInscricaoEstabelecimento.Text = empresa.Cnpj;
                txtCep.Text = empresa.Cep;
                txtLogradouro.Text = empresa.Logradouro;
                txtNumero.Text = empresa.Numero;
                txtComplemento.Text = empresa.Complemento;
                txtBairro.Text = empresa.Bairro;
                txtMunicipio.Text = empresa.Municipio;
                txtUf.Text = empresa.Uf;

                MessageBox.Show(
                    "CNPJ encontrado.\n\nRazao social: " + empresa.RazaoSocial +
                    (string.IsNullOrWhiteSpace(empresa.NomeFantasia) ? string.Empty : "\nNome fantasia: " + empresa.NomeFantasia) +
                    (string.IsNullOrWhiteSpace(empresa.CnaeDescricao) ? string.Empty : "\nCNAE: " + empresa.CnaeDescricao),
                    "CNPJ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel consultar o CNPJ.\n\n" + ex.Message, "CNPJ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarEndereco(CepLookupResult endereco)
        {
            if (endereco == null)
                return;

            txtCep.Text = endereco.Cep;
            txtLogradouro.Text = endereco.Logradouro;
            txtBairro.Text = endereco.Bairro;
            txtMunicipio.Text = endereco.Localidade;
            txtUf.Text = endereco.Uf;

            if (!string.IsNullOrWhiteSpace(endereco.Complemento))
                txtComplemento.Text = endereco.Complemento;
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
            AppNavigator.Show(new CatWitnessesForm(_catId));
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
