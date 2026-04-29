using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class RiskFactorsForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarFator;
        private RoundButton btnAdicionarAmbiente;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Fatores de Risco Ambiental",
                "S-2240 · Condições Ambientais do Trabalho",
                DashboardMenu.Risk
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoFatoresRisco();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoFatoresRisco();
            };

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
            int larguraDisponivel = ContentPanel.ClientSize.Width - (margem * 2);

            if (larguraDisponivel < 790)
                larguraDisponivel = 790;

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 730);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            MontarCabecalho(form, larguraDisponivel);
            MontarPeriodoExposicao(form, larguraDisponivel);
            MontarAmbienteAtividades(form, larguraDisponivel);
            MontarFatoresRisco(form, larguraDisponivel);
            MontarEpiEpc(form, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.Pill(
                    "S-2240",
                    18,
                    18,
                    58,
                    UiColors.AccentBlue,
                    Color.White
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Condições Ambientais do Trabalho — Fatores de Risco",
                    88,
                    14,
                    largura - 300,
                    20,
                    9.5F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Funcionario: selecione um empregado para carregar os dados",
                    88,
                    31,
                    largura - 300,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnSalvar = UiBuilder.SmallButton(
                "💾 Salvar",
                largura - 94,
                16,
                76,
                UiColors.AccentBlue,
                Color.White
            );

            btnSalvar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalvar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            form.Controls.Add(btnSalvar);

            btnCancelar = UiBuilder.SmallButton(
                "Cancelar",
                largura - 178,
                16,
                75,
                Color.White,
                UiColors.BodyText
            );

            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            form.Controls.Add(btnCancelar);

            Panel line = new Panel
            {
                Location = new Point(0, 58),
                Size = new Size(largura, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(line);
        }

        private void MontarPeriodoExposicao(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "PERÍODO DA EXPOSIÇÃO",
                    0,
                    78,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int margem = 18;
            int gap = 20;
            int campoW = (largura - (margem * 2) - (gap * 2)) / 3;

            if (campoW < 200)
                campoW = 200;

            UiBuilder.AddField(
                form,
                "DATA DE AVALIAÇÃO",
                UiBuilder.TextBox("03/23/2026", 18, 0, campoW),
                margem,
                106,
                campoW,
                true
            );

            UiBuilder.AddField(
                form,
                "INÍCIO DA EXPOSIÇÃO",
                UiBuilder.TextBox("mm/dd/yyyy", 0, 0, campoW),
                margem + campoW + gap,
                106,
                campoW,
                true
            );

            UiBuilder.AddField(
                form,
                "FIM DA EXPOSIÇÃO",
                UiBuilder.TextBox("mm/dd/yyyy", 0, 0, campoW),
                margem + ((campoW + gap) * 2),
                106,
                campoW,
                false
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "⚠ Campo obrigatório",
                    margem + campoW + gap,
                    158,
                    campoW,
                    18,
                    7.2F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Preencha apenas se encerrado",
                    margem + ((campoW + gap) * 2),
                    158,
                    campoW,
                    18,
                    7.2F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );
        }

        private void MontarAmbienteAtividades(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "AMBIENTE E ATIVIDADES",
                    0,
                    188,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int margem = 18;
            int gap = 20;
            int botaoW = 28;

            int ambienteW = (largura - (margem * 2) - gap - botaoW - 12) / 2;
            int processoW = largura - (margem * 2) - ambienteW - botaoW - gap - 12;

            if (ambienteW < 300)
                ambienteW = 300;

            if (processoW < 300)
                processoW = 300;

            ComboBox ambiente = UiBuilder.Combo(
                "Selecione um ambiente",
                0,
                0,
                ambienteW
            );

            UiBuilder.AddField(
                form,
                "AMBIENTE DE TRABALHO",
                ambiente,
                margem,
                216,
                ambienteW,
                true
            );

            btnAdicionarAmbiente = UiBuilder.Button(
                "+",
                margem + ambienteW + 6,
                240,
                botaoW,
                30,
                Color.White,
                UiColors.AccentBlue
            );

            btnAdicionarAmbiente.BorderColor = UiColors.Border;
            btnAdicionarAmbiente.Click += BtnAdicionarAmbiente_Click;
            form.Controls.Add(btnAdicionarAmbiente);

            UiBuilder.AddField(
                form,
                "PROCESSO PRODUTIVO",
                UiBuilder.TextBox("Ex: Montagem de peças metálicas", 0, 0, processoW),
                margem + ambienteW + botaoW + gap,
                216,
                processoW,
                true
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "DESCRIÇÃO DAS ATIVIDADES *",
                    margem,
                    270,
                    300,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            CueTextBox descricao = UiBuilder.TextBox(
                "Descreva as atividades realizadas pelo trabalhador neste ambiente...",
                margem,
                294,
                largura - (margem * 2)
            );

            descricao.Multiline = true;
            descricao.Height = 58;
            descricao.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form.Controls.Add(descricao);

            form.Controls.Add(
                UiBuilder.Label(
                    "⚠ Campo obrigatório",
                    margem,
                    354,
                    180,
                    18,
                    7.2F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );
        }

        private void MontarFatoresRisco(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "FATORES DE RISCO",
                    0,
                    386,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "Nenhum fator de risco cadastrado",
                    0,
                    438,
                    largura,
                    34,
                    8.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnAdicionarFator = UiBuilder.SmallButton(
                "+ Adicionar Fator de Risco",
                18,
                525,
                170,
                Color.White,
                UiColors.BodyText
            );

            btnAdicionarFator.BorderColor = UiColors.Border;
            btnAdicionarFator.Click += BtnAdicionarFator_Click;
            form.Controls.Add(btnAdicionarFator);
        }

        private void MontarEpiEpc(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "EPI / EPC — MEDIDAS PROTETIVAS",
                    0,
                    568,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int margem = 18;

            RoundPanel epiPanel = new RoundPanel
            {
                Location = new Point(margem, 598),
                Size = new Size(largura - (margem * 2), 110),
                Radius = 8,
                FillColor = Color.FromArgb(248, 251, 254),
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(epiPanel);

            epiPanel.Controls.Add(
                UiBuilder.Label(
                    "🧤 EPI — EQUIPAMENTO DE PROTEÇÃO INDIVIDUAL",
                    16,
                    12,
                    420,
                    20,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            CheckBox cb1 = new CheckBox
            {
                Text = "Trabalhador utiliza EPI conforme recomendado",
                Checked = false,
                Location = new Point(18, 44),
                Size = new Size(340, 22),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UiColors.BodyText
            };

            epiPanel.Controls.Add(cb1);

            CheckBox cb2 = new CheckBox
            {
                Text = "EPI é eficaz na neutralização / redução do risco",
                Checked = false,
                Location = new Point(390, 44),
                Size = new Size(380, 22),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UiColors.BodyText
            };

            cb2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            epiPanel.Controls.Add(cb2);

            epiPanel.Controls.Add(
                UiBuilder.Label(
                    "Nenhum EPI cadastrado",
                    18,
                    76,
                    330,
                    20,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            PillLabel eficaz = UiBuilder.Pill(
                "Eficaz",
                epiPanel.Width - 140,
                76,
                72,
                Color.FromArgb(217, 248, 234),
                UiColors.Green
            );

            eficaz.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            epiPanel.Controls.Add(eficaz);
        }

        private void AddRiskRow(
            Panel form,
            int largura,
            int y,
            string tipo,
            string agente,
            string intensidade,
            string tecnica)
        {
            int margem = 18;
            int gap = 12;
            int removeW = 40;

            int larguraDisponivel = largura - (margem * 2) - removeW - (gap * 4);

            int campoW = larguraDisponivel / 4;

            if (campoW < 140)
                campoW = 140;

            int x = margem;

            ComboBox comboTipo = UiBuilder.Combo(tipo, 0, 0, campoW);
            UiBuilder.AddField(form, "TIPO DE FATOR", comboTipo, x, y, campoW, true);

            x += campoW + gap;

            ComboBox comboAgente = UiBuilder.Combo(agente, 0, 0, campoW);
            UiBuilder.AddField(form, "AGENTE", comboAgente, x, y, campoW, true);

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "INTENSIDADE",
                UiBuilder.TextBox(intensidade, 0, 0, campoW),
                x,
                y,
                campoW,
                false
            );

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "TÉCNICA DE MEDIÇÃO",
                UiBuilder.TextBox(tecnica, 0, 0, campoW),
                x,
                y,
                campoW,
                false
            );

            RoundButton remove = UiBuilder.Button(
                "×",
                largura - margem - removeW,
                y + 24,
                removeW,
                30,
                UiColors.Red,
                Color.White
            );

            remove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            remove.Click += BtnRemoverFator_Click;
            form.Controls.Add(remove);
        }
    }
}
