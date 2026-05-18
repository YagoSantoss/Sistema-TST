using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class EmployeeCadastroForm : Form
    {
        private readonly int _empregadoId;
        private CueTextBox txtMatricula;
        private CueTextBox txtNome;
        private CueTextBox txtCpf;
        private CueTextBox txtSetor;
        private CueTextBox txtCargo;
        private CueTextBox txtAdmissao;
        private CueTextBox txtVencimentoAso;
        private ComboBox cmbStatusAso;
        private ComboBox cmbMedico;

        public EmployeeCadastroForm()
            : this(0)
        {
        }

        public EmployeeCadastroForm(int empregadoId)
        {
            _empregadoId = empregadoId;
            InitializeComponent();
            CarregarEmpregado();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = _empregadoId > 0 ? "Editar empregado" : "Novo empregado";
            ClientSize = new Size(650, 500);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = UiBuilder.Card(18, 18, 614, 464);
            Controls.Add(card);

            card.Controls.Add(UiBuilder.Label(_empregadoId > 0 ? "Editar empregado" : "Novo empregado", 22, 18, 360, 25, 13F, FontStyle.Bold, UiColors.AccentBlue));
            card.Controls.Add(UiBuilder.Label("Cadastre o funcionário para uso em CAT, ASO e riscos.", 22, 43, 470, 18, 8F, FontStyle.Regular, UiColors.MutedText));
            card.Controls.Add(new Panel { Location = new Point(0, 76), Size = new Size(614, 1), BackColor = UiColors.Border });

            txtMatricula = UiBuilder.TextBox("Ex.: 00412", 0, 0, 130);
            UiBuilder.AddField(card, "Matrícula", txtMatricula, 22, 100, 130, true);

            txtNome = UiBuilder.TextBox("Nome completo", 0, 0, 420);
            UiBuilder.AddField(card, "Nome", txtNome, 172, 100, 420, true);

            txtCpf = UiBuilder.TextBox("000.000.000-00", 0, 0, 170);
            InputFormatHelper.ApplyCpfMask(txtCpf);
            UiBuilder.AddField(card, "CPF", txtCpf, 22, 172, 170, true);

            txtSetor = UiBuilder.TextBox("Setor", 0, 0, 185);
            UiBuilder.AddField(card, "Setor", txtSetor, 212, 172, 185, false);

            txtCargo = UiBuilder.TextBox("Cargo", 0, 0, 175);
            UiBuilder.AddField(card, "Cargo", txtCargo, 417, 172, 175, false);

            txtAdmissao = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, 170);
            InputFormatHelper.ApplyDateMask(txtAdmissao);
            UiBuilder.AddField(card, "Data de admissao", txtAdmissao, 22, 244, 170, false);

            txtVencimentoAso = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, 170);
            InputFormatHelper.ApplyDateMask(txtVencimentoAso);
            UiBuilder.AddField(card, "Vencimento ASO", txtVencimentoAso, 212, 244, 170, false);

            cmbStatusAso = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbStatusAso.Items.AddRange(new[] { "Pendente", "Vigente", "A vencer", "Vencido", "CAT aberta", "Aguardando retorno", "Apto", "Inapto" });
            cmbStatusAso.SelectedIndex = 0;
            UiBuilder.AddField(card, "Status ASO", cmbStatusAso, 402, 244, 190, false);

            cmbMedico = CriarComboMedicos(570);
            UiBuilder.AddField(card, "Médico responsável", cmbMedico, 22, 316, 570, false);

            card.Controls.Add(new Panel { Location = new Point(0, 392), Size = new Size(614, 1), BackColor = UiColors.Border });

            RoundButton cancelar = UiBuilder.SmallButton("Cancelar", 430, 416, 78, Color.White, UiColors.BodyText);
            cancelar.BorderColor = UiColors.Border;
            cancelar.Click += (sender, e) => Close();
            card.Controls.Add(cancelar);

            RoundButton salvar = UiBuilder.SmallButton(_empregadoId > 0 ? "Salvar" : "Cadastrar", 520, 416, 82, UiColors.AccentBlue, Color.White);
            salvar.Click += Salvar_Click;
            card.Controls.Add(salvar);

            ResumeLayout(false);
        }

        private void Salvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatricula.Text) || string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtCpf.Text))
            {
                MessageBox.Show("Preencha matricula, nome e CPF.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidationHelper.IsCompleteCpf(txtCpf.Text))
            {
                MessageBox.Show("Informe o CPF no formato 000.000.000-00.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtAdmissao.Text) && !ValidationHelper.IsValidDate(txtAdmissao.Text))
            {
                MessageBox.Show("Informe a data de admissao no formato dd/mm/aaaa.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtVencimentoAso.Text) && !ValidationHelper.IsValidDate(txtVencimentoAso.Text))
            {
                MessageBox.Show("Informe o vencimento do ASO no formato dd/mm/aaaa.", "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ComboItem medico = cmbMedico.SelectedItem as ComboItem;

                CadastrosRepository.SaveEmpregado(new EmpregadoRecord
                {
                    Id = _empregadoId,
                    Matricula = txtMatricula.Text.Trim(),
                    Nome = txtNome.Text.Trim(),
                    Cpf = txtCpf.Text.Trim(),
                    Setor = txtSetor.Text.Trim(),
                    Cargo = txtCargo.Text.Trim(),
                    DataAdmissao = txtAdmissao.Text.Trim(),
                    DataVencimentoAso = txtVencimentoAso.Text.Trim(),
                    StatusAso = cmbStatusAso.Text,
                    MedicoId = medico != null && medico.Id > 0 ? (int?)medico.Id : null
                });

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o empregado no MySQL.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ComboBox CriarComboMedicos(int width)
        {
            ComboBox combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };

            combo.Items.Add(new ComboItem(0, "-- Sem médico vinculado --"));

            try
            {
                foreach (MedicoRecord medico in CadastrosRepository.GetMedicos())
                {
                    combo.Items.Add(new ComboItem(medico.Id, medico.Nome + " - " + medico.Crm));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponível"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void CarregarEmpregado()
        {
            if (_empregadoId <= 0)
                return;

            try
            {
                EmpregadoRecord empregado = CadastrosRepository.GetEmpregado(_empregadoId);
                if (empregado == null)
                    return;

                txtMatricula.Text = empregado.Matricula;
                txtNome.Text = empregado.Nome;
                txtCpf.Text = empregado.Cpf;
                txtSetor.Text = empregado.Setor;
                txtCargo.Text = empregado.Cargo;
                txtAdmissao.Text = empregado.DataAdmissao;
                txtVencimentoAso.Text = empregado.DataVencimentoAso;
                SelecionarTexto(cmbStatusAso, empregado.StatusAso);

                if (empregado.MedicoId.HasValue)
                    SelecionarComboItem(cmbMedico, empregado.MedicoId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar o empregado do MySQL.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelecionarTexto(ComboBox combo, string texto)
        {
            int index = combo.Items.IndexOf(texto);
            if (index >= 0)
                combo.SelectedIndex = index;
        }

        private void SelecionarComboItem(ComboBox combo, int id)
        {
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
    }
}
