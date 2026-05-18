using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public sealed class EpiSelectionForm : Form
    {
        private readonly List<CheckBox> _checks = new List<CheckBox>();
        private readonly string[] _epis =
        {
            "Capacete de segurança",
            "Oculos de proteção",
            "Protetor auricular",
            "Mascara ou respirador",
            "Luvas de proteção",
            "Calcado ou bota de segurança",
            "Avental de proteção",
            "Cinto de segurança",
            "Protetor facial",
            "Creme de proteção"
        };

        public string SelectedEpis { get; private set; }

        public EpiSelectionForm(string selectedEpis)
        {
            SelectedEpis = selectedEpis ?? string.Empty;
            InitializeComponent();
            MarcarSelecionados(SelectedEpis);
        }

        private void InitializeComponent()
        {
            Text = "Selecionar EPIs";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(430, 410);
            BackColor = UiColors.PageBg;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = new Size(394, 374),
                Radius = 8,
                FillColor = Color.White,
                BorderColor = UiColors.Border
            };
            Controls.Add(card);

            card.Controls.Add(UiBuilder.Label("EPIs utilizados pelo trabalhador", 18, 16, 350, 22, 10F, FontStyle.Bold, UiColors.AccentBlue));
            card.Controls.Add(UiBuilder.Label("Selecione um ou mais equipamentos usados neste registro.", 18, 38, 350, 18, 7.5F, FontStyle.Regular, UiColors.MutedText));

            FlowLayoutPanel lista = new FlowLayoutPanel
            {
                Location = new Point(18, 70),
                Size = new Size(358, 230),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.White
            };
            card.Controls.Add(lista);

            foreach (string epi in _epis)
            {
                CheckBox check = new CheckBox
                {
                    Text = epi,
                    Width = 320,
                    Height = 24,
                    BackColor = Color.White,
                    ForeColor = UiColors.BodyText,
                    Font = new Font("Segoe UI", 8F, FontStyle.Regular)
                };
                _checks.Add(check);
                lista.Controls.Add(check);
            }

            RoundButton btnCancelar = UiBuilder.SmallButton("Cancelar", 202, 324, 82, Color.White, UiColors.BodyText);
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            card.Controls.Add(btnCancelar);

            RoundButton btnConfirmar = UiBuilder.SmallButton("Confirmar", 294, 324, 82, UiColors.AccentBlue, Color.White);
            btnConfirmar.Click += Confirmar_Click;
            card.Controls.Add(btnConfirmar);
        }

        private void Confirmar_Click(object sender, EventArgs e)
        {
            StringBuilder selecionados = new StringBuilder();

            foreach (CheckBox check in _checks)
            {
                if (!check.Checked)
                    continue;

                if (selecionados.Length > 0)
                    selecionados.Append(", ");

                selecionados.Append(check.Text);
            }

            if (selecionados.Length == 0)
            {
                MessageBox.Show("Selecione pelo menos um EPI.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedEpis = selecionados.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MarcarSelecionados(string selecionados)
        {
            if (string.IsNullOrWhiteSpace(selecionados))
                return;

            HashSet<string> itens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string item in selecionados.Split(','))
            {
                string valor = item.Trim();
                if (valor.Length > 0)
                    itens.Add(valor);
            }

            foreach (CheckBox check in _checks)
                check.Checked = itens.Contains(check.Text);
        }
    }
}
