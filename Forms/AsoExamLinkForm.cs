using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class AsoExamLinkForm : Form
    {
        private readonly int _empregadoId;
        private readonly string _pacienteNome;
        private readonly string _medicoNome;
        private CueTextBox txtPaciente;
        private CueTextBox txtMedico;
        private ComboBox cmbTipoExame;
        private CueTextBox txtDataExame;
        private CueTextBox txtResultado;
        private CueTextBox txtObservacoes;

        public AsoExameRecord Exame { get; private set; }

        public AsoExamLinkForm(int empregadoId, string pacienteNome, string medicoNome)
        {
            _empregadoId = empregadoId;
            _pacienteNome = pacienteNome;
            _medicoNome = medicoNome;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Adicionar exame ao ASO";
            ClientSize = new Size(620, 430);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = new Size(584, 394),
                Radius = 10,
                FillColor = Color.White,
                BorderColor = UiColors.Border
            };
            Controls.Add(card);

            card.Controls.Add(UiBuilder.Label("Adicionar exame", 22, 18, 360, 25, 13F, FontStyle.Bold, UiColors.AccentBlue));
            card.Controls.Add(UiBuilder.Label("Confirme o paciente, o medico responsavel e o exame realizado.", 22, 43, 500, 18, 8F, FontStyle.Regular, UiColors.MutedText));

            Panel line = new Panel { Location = new Point(0, 76), Size = new Size(584, 1), BackColor = UiColors.Border };
            card.Controls.Add(line);

            txtPaciente = UiBuilder.TextBox("Nome do paciente", 0, 0, 255);
            txtPaciente.Text = _pacienteNome;
            txtPaciente.ReadOnly = true;
            UiBuilder.AddField(card, "PACIENTE", txtPaciente, 22, 100, 255, true);

            txtMedico = UiBuilder.TextBox("Nome do medico", 0, 0, 245);
            txtMedico.Text = _medicoNome;
            txtMedico.ReadOnly = true;
            UiBuilder.AddField(card, "MEDICO", txtMedico, 307, 100, 245, true);

            UiBuilder.AddField(card, "EXAME", cmbTipoExame = CriarComboExames(350), 22, 172, 350, true);
            UiBuilder.AddField(card, "DATA", txtDataExame = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, 160), 392, 172, 160, false);
            InputFormatHelper.ApplyDateMask(txtDataExame);
            UiBuilder.AddField(card, "RESULTADO", txtResultado = UiBuilder.TextBox("Ex.: Normal, Alterado, Apto", 0, 0, 255), 22, 244, 255, false);
            UiBuilder.AddField(card, "OBSERVACOES", txtObservacoes = UiBuilder.TextBox("Observacoes do exame", 0, 0, 245), 307, 244, 245, false);

            Panel footerLine = new Panel { Location = new Point(0, 326), Size = new Size(584, 1), BackColor = UiColors.Border };
            card.Controls.Add(footerLine);

            RoundButton cancelar = UiBuilder.SmallButton("Cancelar", 390, 350, 78, Color.White, UiColors.BodyText);
            cancelar.BorderColor = UiColors.Border;
            cancelar.Click += (sender, e) => { DialogResult = DialogResult.Cancel; Close(); };
            card.Controls.Add(cancelar);

            RoundButton adicionar = UiBuilder.SmallButton("Adicionar", 480, 350, 82, UiColors.AccentBlue, Color.White);
            adicionar.Click += Adicionar_Click;
            card.Controls.Add(adicionar);

            ResumeLayout(false);
        }

        private ComboBox CriarComboExames(int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            combo.Items.Add(new ComboItem(0, "Selecione o exame"));

            try
            {
                foreach (TipoExameRecord exame in CadastrosRepository.GetTiposExamesPorEmpregado(_empregadoId))
                {
                    combo.Items.Add(new ComboItem(exame.Id, exame.Nome + " - " + exame.Tipo));
                }

                if (combo.Items.Count == 1)
                    combo.Items.Add(new ComboItem(0, "Nenhum exame cadastrado para este paciente"));
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void Adicionar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPaciente.Text))
            {
                MessageBox.Show("Informe o paciente do exame.", "Exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMedico.Text))
            {
                MessageBox.Show("Informe o medico responsavel pelo exame.", "Exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ComboItem tipo = cmbTipoExame.SelectedItem as ComboItem;
            if (tipo == null || tipo.Id <= 0)
            {
                MessageBox.Show("Selecione o exame.", "Exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtDataExame.Text) && !ValidationHelper.IsValidDate(txtDataExame.Text))
            {
                MessageBox.Show("Informe a data do exame no formato dd/mm/aaaa.", "Exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Exame = new AsoExameRecord
            {
                TipoExameId = tipo.Id,
                TipoExameNome = tipo.Text,
                DataExame = txtDataExame.Text.Trim(),
                Resultado = txtResultado.Text.Trim(),
                Observacoes = txtObservacoes.Text.Trim()
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
