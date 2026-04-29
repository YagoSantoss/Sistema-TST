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
                MessageBox.Show("Nao foi possivel registrar o ASO no MySQL.\n\n" + ex.Message, "ASO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHistorico_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Histórico de ASOs do empregado exibido.", "Histórico", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAdicionarExame_Click(object sender, EventArgs e)
        {
            AppNavigator.Show(new ExamTypesForm());
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

                cmbTipoExame.Text = "Relacionado a CAT";
                txtObservacoes.Text = "CAT " + cat.Id + " - Acidente em " + cat.DataAcidente + ". " + cat.Descricao;
                SelecionarResultado("Inapto");
            }
            catch
            {
                SelecionarResultado("Inapto");
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
                    cmbCat.Items.Add(new ComboItem(0, "MySQL indisponivel"));
                }
            }

            cmbCat.SelectedIndex = 0;
        }

        private void RegistrarAso()
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            ComboItem medico = cmbMedico.SelectedItem as ComboItem;
            ComboItem cat = cmbCat.SelectedItem as ComboItem;

            if (empregado == null || empregado.Id <= 0)
                throw new InvalidOperationException("Selecione o empregado.");

            if (medico == null || medico.Id <= 0)
                throw new InvalidOperationException("Selecione o medico responsavel.");

            if (string.IsNullOrWhiteSpace(txtDataAso.Text))
                throw new InvalidOperationException("Informe a data do ASO no formato dd/mm/yyyy.");

            CadastrosRepository.SaveAso(new AsoRecord
            {
                EmpregadoId = empregado.Id,
                MedicoId = medico.Id,
                CatId = cat != null && cat.Id > 0 ? (int?)cat.Id : null,
                DataAso = txtDataAso.Text.Trim(),
                TipoExame = cmbTipoExame.Text.Trim(),
                Resultado = resultadoSelecionado,
                Observacoes = txtObservacoes.Text.Trim()
            });
        }
    }
}
