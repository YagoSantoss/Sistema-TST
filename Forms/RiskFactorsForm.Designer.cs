using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RiskFactorsForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarAmbiente;
        private ComboBox cmbEmpregado;
        private ComboBox cmbAmbiente;
        private ComboBox cmbTipoFator;
        private CueTextBox txtDataAvaliacao;
        private CueTextBox txtInicioExposicao;
        private CueTextBox txtFimExposicao;
        private CueTextBox txtDescricaoAtividades;
        private CueTextBox txtAgente;
        private CueTextBox txtIntensidade;
        private CueTextBox txtTecnicaMedicao;
        private CheckBox cbUsaEpi;
        private CheckBox cbEpiEficaz;
        private Label lblEpisSelecionados;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Fatores de Risco Ambiental",
                "S-2240 - Condições Ambientais do Trabalho",
                DashboardMenu.Risk
            );

            ContentPanel.AutoScroll = true;
            MontarConteudoFatoresRisco();
            ContentPanel.Resize += (sender, e) => MontarConteudoFatoresRisco();

            ResumeLayout(false);
        }

        private void MontarConteudoFatoresRisco()
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

            MontarCabecalho(form, largura);
            MontarCampos(form, largura);
            MontarEpiEpc(form, largura);

            ContentPanel.ResumeLayout(false);
            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel form, int largura)
        {
            form.Controls.Add(UiBuilder.Pill("S-2240", 18, 18, 58, UiColors.AccentBlue, Color.White));
            form.Controls.Add(UiBuilder.Label("Condições Ambientais do Trabalho - Fatores de Risco", 88, 14, largura - 300, 20, 9.5F, FontStyle.Bold, UiColors.AccentBlue));
            form.Controls.Add(UiBuilder.Label("Cadastre ou edite o fator de risco ambiental do empregado", 88, 31, largura - 300, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            btnSalvar = UiBuilder.SmallButton("Salvar", largura - 94, 16, 76, UiColors.AccentBlue, Color.White);
            btnSalvar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalvar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            form.Controls.Add(btnSalvar);

            btnCancelar = UiBuilder.SmallButton("Cancelar", largura - 178, 16, 75, Color.White, UiColors.BodyText);
            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            form.Controls.Add(btnCancelar);

            Panel line = new Panel { Location = new Point(0, 58), Size = new Size(largura, 1), BackColor = UiColors.Border, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            form.Controls.Add(line);
        }

        private void MontarCampos(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 18;
            int campoW = (largura - (margem * 2) - (gap * 3)) / 4;
            if (campoW < 170)
                campoW = 170;

            UiBuilder.AddField(form, "EMPREGADO", cmbEmpregado = CriarComboEmpregados(campoW), margem, 84, campoW, true);
            UiBuilder.AddField(form, "DATA DE AVALIAÇÃO", txtDataAvaliacao = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, campoW), margem + campoW + gap, 84, campoW, true);
            InputFormatHelper.ApplyDateMask(txtDataAvaliacao);
            UiBuilder.AddField(form, "INÍCIO DA EXPOSIÇÃO", txtInicioExposicao = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, campoW), margem + ((campoW + gap) * 2), 84, campoW, true);
            InputFormatHelper.ApplyDateMask(txtInicioExposicao);
            UiBuilder.AddField(form, "FIM DA EXPOSIÇÃO", txtFimExposicao = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, campoW), margem + ((campoW + gap) * 3), 84, campoW, false);
            InputFormatHelper.ApplyDateMask(txtFimExposicao);

            int metadeW = (largura - (margem * 2) - gap - 34) / 2;
            if (metadeW < 280)
                metadeW = 280;

            UiBuilder.AddField(form, "AMBIENTE DE TRABALHO", cmbAmbiente = CriarComboAmbientes(metadeW), margem, 158, metadeW, true);
            btnAdicionarAmbiente = UiBuilder.Button("+", margem + metadeW + 6, 182, 28, 30, Color.White, UiColors.AccentBlue);
            btnAdicionarAmbiente.BorderColor = UiColors.Border;
            btnAdicionarAmbiente.Click += BtnAdicionarAmbiente_Click;
            form.Controls.Add(btnAdicionarAmbiente);

            UiBuilder.AddField(form, "DESCRIÇÃO DAS ATIVIDADES", txtDescricaoAtividades = UiBuilder.TextBox("Descreva as atividades realizadas", 0, 0, metadeW), margem + metadeW + gap + 34, 158, metadeW, true);

            UiBuilder.AddField(form, "TIPO DE FATOR", cmbTipoFator = CriarComboTipoFator(campoW), margem, 244, campoW, true);
            UiBuilder.AddField(form, "AGENTE", txtAgente = UiBuilder.TextBox("Ex.: Ruido, calor, poeira", 0, 0, campoW), margem + campoW + gap, 244, campoW, true);
            UiBuilder.AddField(form, "INTENSIDADE", txtIntensidade = UiBuilder.TextBox("Ex.: 85 dB", 0, 0, campoW), margem + ((campoW + gap) * 2), 244, campoW, false);
            UiBuilder.AddField(form, "TÉCNICA DE MEDIÇÃO", txtTecnicaMedicao = UiBuilder.TextBox("Ex.: Dosimetria", 0, 0, campoW), margem + ((campoW + gap) * 3), 244, campoW, false);
        }

        private void MontarEpiEpc(RoundPanel form, int largura)
        {
            int margem = 18;
            form.Controls.Add(UiBuilder.CenterLabel("EPI / EPC - MEDIDAS PROTETIVAS", 0, 334, largura, 20, 8F, FontStyle.Bold, UiColors.AccentBlue));

            RoundPanel epiPanel = new RoundPanel
            {
                Location = new Point(margem, 370),
                Size = new Size(largura - (margem * 2), 90),
                Radius = 8,
                FillColor = Color.FromArgb(248, 251, 254),
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            form.Controls.Add(epiPanel);

            cbUsaEpi = new CheckBox { Text = "Trabalhador utiliza EPI conforme recomendado", Location = new Point(18, 22), Size = new Size(340, 22), BackColor = Color.Transparent, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = UiColors.BodyText };
            cbUsaEpi.CheckedChanged += CbUsaEpi_CheckedChanged;
            epiPanel.Controls.Add(cbUsaEpi);

            cbEpiEficaz = new CheckBox { Text = "EPI e eficaz na neutralização / redução do risco", Location = new Point(390, 22), Size = new Size(380, 22), BackColor = Color.Transparent, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = UiColors.BodyText };
            epiPanel.Controls.Add(cbEpiEficaz);

            lblEpisSelecionados = UiBuilder.Label("EPIs selecionados: nenhum", 18, 54, largura - 80, 18, 7.5F, FontStyle.Regular, UiColors.MutedText);
            lblEpisSelecionados.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            epiPanel.Controls.Add(lblEpisSelecionados);
            AtualizarResumoEpi();
        }

        private ComboBox CriarComboEmpregados(int width)
        {
            ComboBox combo = UiBuilder.Combo("Selecione um empregado", 0, 0, width);
            combo.Items.Add(new ComboItem(0, "Selecione um empregado"));
            try
            {
                foreach (EmpregadoRecord empregado in CadastrosRepository.GetEmpregados())
                    combo.Items.Add(new ComboItem(empregado.Id, empregado.Nome + " - " + empregado.Matricula));
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponível"));
            }
            combo.SelectedIndex = 0;
            return combo;
        }

        private ComboBox CriarComboAmbientes(int width)
        {
            ComboBox combo = UiBuilder.Combo("Selecione um ambiente", 0, 0, width);
            combo.Items.Add(new ComboItem(0, "Selecione um ambiente"));
            try
            {
                foreach (AmbienteTrabalhoRecord ambiente in CadastrosRepository.GetAmbientes())
                    combo.Items.Add(new ComboItem(ambiente.Id, ambiente.Ambiente + " - " + ambiente.Setor));
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponível"));
            }
            combo.SelectedIndex = 0;
            return combo;
        }

        private ComboBox CriarComboTipoFator(int width)
        {
            ComboBox combo = UiBuilder.Combo("Fisico", 0, 0, width);
            combo.Items.AddRange(new object[] { "Fisico", "Quimico", "Biologico", "Ergonomico", "Acidente" });
            combo.SelectedIndex = 0;
            return combo;
        }
    }
}
