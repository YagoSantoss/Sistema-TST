using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatMedicalForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarMedico;
        private ComboBox cmbParteCorpo;
        private ComboBox cmbLateralidade;
        private CueTextBox txtAgenteCausador;
        private CueTextBox txtCid10;
        private CueTextBox txtNaturezaLesao;
        private CueTextBox txtDuracaoTratamento;
        private ComboBox cmbMedicoAssistente;
        private CueTextBox txtObservacaoMedica;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();
            BuildDashboardShell("CAT - Acidente de Trabalho", "S-2210 - Comunicacao de Acidente", DashboardMenu.Cat);
            ContentPanel.AutoScroll = true;
            MontarConteudoDadosMedicos();
            ContentPanel.Resize += (sender, e) => MontarConteudoDadosMedicos();
            ResumeLayout(false);
        }

        private void MontarConteudoDadosMedicos()
        {
            if (_montandoConteudo)
                return;

            _montandoConteudo = true;
            ContentPanel.SuspendLayout();
            ContentPanel.Controls.Clear();

            int margem = 18;
            int largura = ContentPanel.ClientSize.Width - (margem * 2);
            if (largura < 790)
                largura = 790;

            RoundPanel form = UiBuilder.Card(margem, 18, largura, 560);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            BuildCatHeader(form, 2, largura);
            form.Controls.Add(UiBuilder.CenterLabel("DADOS COMPLEMENTARES E MEDICOS", 0, 126, largura, 20, 8F, FontStyle.Bold, UiColors.AccentBlue));
            MontarCamposMedicos(form, largura);

            ContentPanel.ResumeLayout(false);
            _montandoConteudo = false;
        }

        private void MontarCamposMedicos(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 18;
            int metadeW = (largura - (margem * 2) - gap) / 2;
            if (metadeW < 340)
                metadeW = 340;

            int xEsq = margem;
            int xDir = margem + metadeW + gap;

            UiBuilder.AddField(form, "PARTE DO CORPO ATINGIDA", cmbParteCorpo = CriarComboPartesCorpo(metadeW), xEsq, 154, metadeW, true);
            UiBuilder.AddField(form, "LATERALIDADE", cmbLateralidade = UiBuilder.Combo("Nao aplicavel", 0, 0, metadeW), xDir, 154, metadeW, false);
            cmbLateralidade.Items.Add("Direita");
            cmbLateralidade.Items.Add("Esquerda");
            cmbLateralidade.Items.Add("Bilateral");

            UiBuilder.AddField(form, "AGENTE CAUSADOR", txtAgenteCausador = UiBuilder.TextBox("Ex.: composto de fosforo", 0, 0, metadeW), xEsq, 232, metadeW, true);
            UiBuilder.AddField(form, "NATUREZA DA LESAO", txtNaturezaLesao = UiBuilder.TextBox("Ex.: doenca, NIC", 0, 0, metadeW), xDir, 232, metadeW, true);
            UiBuilder.AddField(form, "CID-10", txtCid10 = UiBuilder.TextBox("Ex.: A23.0", 0, 0, metadeW), xEsq, 310, metadeW, true);
            UiBuilder.AddField(form, "DURACAO DO TRATAMENTO", txtDuracaoTratamento = UiBuilder.TextBox("Ex.: 10 dias", 0, 0, metadeW), xDir, 310, metadeW, false);

            int botaoW = 28;
            int medicoW = metadeW - botaoW - 8;
            UiBuilder.AddField(form, "MEDICO / DENTISTA", cmbMedicoAssistente = CriarComboMedicos(medicoW), xEsq, 388, medicoW, false);
            btnAdicionarMedico = UiBuilder.Button("+", xEsq + medicoW + 6, 412, botaoW, 30, Color.White, UiColors.AccentBlue);
            btnAdicionarMedico.BorderColor = UiColors.Border;
            btnAdicionarMedico.Click += BtnAdicionarMedico_Click;
            form.Controls.Add(btnAdicionarMedico);

            UiBuilder.AddField(form, "OBSERVACAO", txtObservacaoMedica = UiBuilder.TextBox("Observacao complementar da lesao", 0, 0, metadeW), xDir, 388, metadeW, false);

            CarregarDadosComplementares();
        }

        private ComboBox CriarComboPartesCorpo(int width)
        {
            ComboBox combo = UiBuilder.Combo("Boca (inclusive labios, dentes, lingua, garganta e paladar)", 0, 0, width);
            combo.Items.Add("Cabeca");
            combo.Items.Add("Olho");
            combo.Items.Add("Ouvido");
            combo.Items.Add("Nariz");
            combo.Items.Add("Pescoco");
            combo.Items.Add("Ombro");
            combo.Items.Add("Braco");
            combo.Items.Add("Cotovelo");
            combo.Items.Add("Antebraco");
            combo.Items.Add("Punho");
            combo.Items.Add("Mao");
            combo.Items.Add("Dedo da mao");
            combo.Items.Add("Torax");
            combo.Items.Add("Costas");
            combo.Items.Add("Abdome");
            combo.Items.Add("Quadril");
            combo.Items.Add("Perna");
            combo.Items.Add("Joelho");
            combo.Items.Add("Tornozelo");
            combo.Items.Add("Pe");
            combo.Items.Add("Dedo do pe");
            combo.Items.Add("Pele");
            combo.Items.Add("Sistema respiratorio");
            combo.Items.Add("Sistema circulatorio");
            combo.Items.Add("Multiplas partes");
            combo.Items.Add("Nao aplicavel");
            return combo;
        }

        private ComboBox CriarComboMedicos(int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 34),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            combo.Items.Add(new ComboItem(0, "Digite o nome ou CRM do medico"));

            try
            {
                foreach (MedicoRecord medico in CadastrosRepository.GetMedicos())
                {
                    combo.Items.Add(new ComboItem(medico.Id, TextoMedico(medico)));
                }

                if (combo.Items.Count == 1)
                    combo.Items.Add(new ComboItem(0, "Nenhum medico cadastrado"));
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            combo.Leave += CmbMedicoAssistente_Leave;
            combo.SelectedIndexChanged += CmbMedicoAssistente_SelectedIndexChanged;
            return combo;
        }

        private void BuildCatHeader(Panel form, int activeTab, int largura)
        {
            form.Controls.Add(UiBuilder.Pill("S-2210", 18, 18, 58, UiColors.Red, Color.White));
            form.Controls.Add(UiBuilder.Label("Comunicacao de Acidente de Trabalho (CAT)", 88, 14, largura - 320, 20, 9.5F, FontStyle.Bold, UiColors.AccentBlue));
            form.Controls.Add(UiBuilder.Label("Registro de novo acidente ou doenca ocupacional", 88, 31, largura - 320, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            btnSalvar = UiBuilder.SmallButton("Salvar CAT", largura - 110, 16, 92, UiColors.AccentBlue, Color.White);
            btnSalvar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalvar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            form.Controls.Add(btnSalvar);

            btnCancelar = UiBuilder.SmallButton("Cancelar", largura - 195, 16, 75, Color.White, UiColors.BodyText);
            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            form.Controls.Add(btnCancelar);

            form.Controls.Add(new Panel { Location = new Point(0, 58), Size = new Size(largura, 1), BackColor = UiColors.Border, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right });
            AddTab(form, "DADOS CADASTRAIS", 18, activeTab == 0, TabDados_Click);
            AddTab(form, "TESTEMUNHAS", 190, activeTab == 1, TabTestemunhas_Click);
            AddTab(form, "DADOS COMPLEMENTARES", 370, activeTab == 2, TabComplementares_Click);
        }

        private void AddTab(Panel form, string text, int x, bool active, System.EventHandler handler)
        {
            int larguraTab = text.Contains("COMPLEMENTARES") ? 225 : 165;
            Button tab = new Button { Text = text, Location = new Point(x, 72), Size = new Size(larguraTab, 28), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = active ? UiColors.AccentBlue : UiColors.MutedText, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            tab.FlatAppearance.BorderSize = 0;
            tab.Click += handler;
            form.Controls.Add(tab);
            if (active)
                form.Controls.Add(new Panel { Location = new Point(x, 101), Size = new Size(larguraTab, 2), BackColor = UiColors.Orange });
        }
    }
}
