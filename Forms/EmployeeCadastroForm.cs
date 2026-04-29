using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class EmployeeCadastroForm : Form
    {
        private CueTextBox txtMatricula;
        private CueTextBox txtNome;
        private CueTextBox txtCpf;
        private CueTextBox txtSetor;
        private CueTextBox txtCargo;
        private CueTextBox txtAdmissao;
        private CueTextBox txtVencimentoAso;
        private ComboBox cmbStatusAso;

        public EmployeeCadastroForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Novo empregado";
            ClientSize = new Size(650, 430);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = UiBuilder.Card(18, 18, 614, 394);
            Controls.Add(card);

            card.Controls.Add(UiBuilder.Label("Novo empregado", 22, 18, 360, 25, 13F, FontStyle.Bold, UiColors.AccentBlue));
            card.Controls.Add(UiBuilder.Label("Cadastre o funcionario para uso em CAT, ASO e riscos.", 22, 43, 470, 18, 8F, FontStyle.Regular, UiColors.MutedText));
            card.Controls.Add(new Panel { Location = new Point(0, 76), Size = new Size(614, 1), BackColor = UiColors.Border });

            txtMatricula = UiBuilder.TextBox("Ex.: 00412", 0, 0, 130);
            UiBuilder.AddField(card, "Matricula", txtMatricula, 22, 100, 130, true);

            txtNome = UiBuilder.TextBox("Nome completo", 0, 0, 420);
            UiBuilder.AddField(card, "Nome", txtNome, 172, 100, 420, true);

            txtCpf = UiBuilder.TextBox("000.000.000-00", 0, 0, 170);
            UiBuilder.AddField(card, "CPF", txtCpf, 22, 172, 170, true);

            txtSetor = UiBuilder.TextBox("Setor", 0, 0, 185);
            UiBuilder.AddField(card, "Setor", txtSetor, 212, 172, 185, false);

            txtCargo = UiBuilder.TextBox("Cargo", 0, 0, 175);
            UiBuilder.AddField(card, "Cargo", txtCargo, 417, 172, 175, false);

            txtAdmissao = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, 170);
            UiBuilder.AddField(card, "Data de admissao", txtAdmissao, 22, 244, 170, false);

            txtVencimentoAso = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, 170);
            UiBuilder.AddField(card, "Vencimento ASO", txtVencimentoAso, 212, 244, 170, false);

            cmbStatusAso = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbStatusAso.Items.AddRange(new[] { "Pendente", "Vigente", "A vencer", "Vencido" });
            cmbStatusAso.SelectedIndex = 0;
            UiBuilder.AddField(card, "Status ASO", cmbStatusAso, 402, 244, 190, false);

            card.Controls.Add(new Panel { Location = new Point(0, 322), Size = new Size(614, 1), BackColor = UiColors.Border });

            RoundButton cancelar = UiBuilder.SmallButton("Cancelar", 430, 346, 78, Color.White, UiColors.BodyText);
            cancelar.BorderColor = UiColors.Border;
            cancelar.Click += (sender, e) => Close();
            card.Controls.Add(cancelar);

            RoundButton salvar = UiBuilder.SmallButton("Cadastrar", 520, 346, 82, UiColors.AccentBlue, Color.White);
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

            try
            {
                CadastrosRepository.SaveEmpregado(new EmpregadoRecord
                {
                    Matricula = txtMatricula.Text.Trim(),
                    Nome = txtNome.Text.Trim(),
                    Cpf = txtCpf.Text.Trim(),
                    Setor = txtSetor.Text.Trim(),
                    Cargo = txtCargo.Text.Trim(),
                    DataAdmissao = txtAdmissao.Text.Trim(),
                    DataVencimentoAso = txtVencimentoAso.Text.Trim(),
                    StatusAso = cmbStatusAso.Text
                });

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel salvar o empregado no MySQL.\n\n" + ex.Message, "Empregados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
