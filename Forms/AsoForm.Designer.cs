using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class AsoForm
    {
        private RoundButton btnRegistrar;
        private RoundButton btnHistorico;
        private RoundButton btnAdicionarMedico;
        private RoundButton btnAdicionarExame;
        private ComboBox cmbEmpregado;
        private ComboBox cmbCat;
        private ComboBox cmbTipoExame;
        private ComboBox cmbMedico;
        private CueTextBox txtDataAso;
        private CueTextBox txtObservacoes;
        private RoundPanel cardApto;
        private RoundPanel cardInapto;
        private string resultadoSelecionado = "Apto";

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Monitoramento da Saúde / ASO",
                "S-2220 · Atestado de Saúde Ocupacional",
                DashboardMenu.Aso
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoAso();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoAso();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoAso()
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

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 560);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            MontarCabecalho(form, larguraDisponivel);
            MontarIdentificacao(form, larguraDisponivel);
            MontarResultadoAso(form, larguraDisponivel);
            MontarMedicoObservacoes(form, larguraDisponivel);
            MontarExamesComplementares(form, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.Pill(
                    "S-2220",
                    18,
                    18,
                    58,
                    UiColors.Green,
                    Color.White
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Atestado de Saúde Ocupacional (ASO)",
                    88,
                    14,
                    largura - 330,
                    20,
                    9.5F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Monitoramento da Saúde do Trabalhador",
                    88,
                    31,
                    largura - 330,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnRegistrar = UiBuilder.SmallButton(
                "💾 Registrar ASO",
                largura - 125,
                16,
                107,
                UiColors.AccentBlue,
                Color.White
            );

            btnRegistrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegistrar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnRegistrar.Click += BtnRegistrar_Click;
            form.Controls.Add(btnRegistrar);

            btnHistorico = UiBuilder.SmallButton(
                "Histórico",
                largura - 210,
                16,
                75,
                Color.White,
                UiColors.BodyText
            );

            btnHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHistorico.BorderColor = UiColors.Border;
            btnHistorico.Click += BtnHistorico_Click;
            form.Controls.Add(btnHistorico);

            Panel line = new Panel
            {
                Location = new Point(0, 58),
                Size = new Size(largura, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(line);
        }

        private void MontarIdentificacao(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "IDENTIFICAÇÃO",
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
            int gap = 14;

            int totalW = largura - (margem * 2) - (gap * 3);
            int empregadoW = (int)(totalW * 0.34);
            int catW = (int)(totalW * 0.30);
            int dataW = (int)(totalW * 0.16);
            int tipoW = totalW - empregadoW - catW - dataW;

            if (empregadoW < 220)
                empregadoW = 220;

            if (catW < 210)
                catW = 210;

            if (dataW < 120)
                dataW = 120;

            if (tipoW < 130)
                tipoW = 130;

            int x = margem;

            UiBuilder.AddField(
                form,
                "EMPREGADO",
                cmbEmpregado = CriarComboEmpregados(empregadoW),
                x,
                106,
                empregadoW,
                true
            );

            x += empregadoW + gap;

            UiBuilder.AddField(
                form,
                "CAT VINCULADA",
                cmbCat = CriarComboCats(0, catW),
                x,
                106,
                catW,
                false
            );

            x += catW + gap;

            UiBuilder.AddField(
                form,
                "DATA DO ASO",
                txtDataAso = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, dataW),
                x,
                106,
                dataW,
                true
            );

            x += dataW + gap;

            UiBuilder.AddField(
                form,
                "TIPO DE EXAME",
                cmbTipoExame = CriarComboTipoAso(tipoW),
                x,
                106,
                tipoW,
                true
            );
        }

        private void MontarResultadoAso(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "RESULTADO DO ASO",
                    0,
                    166,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int margem = 18;
            int gap = 18;
            int cardW = (largura - (margem * 2) - gap) / 2;

            if (cardW < 300)
                cardW = 300;

            cardApto = new RoundPanel
            {
                Location = new Point(margem, 198),
                Size = new Size(cardW, 65),
                Radius = 8,
                FillColor = Color.FromArgb(211, 250, 226),
                BorderColor = UiColors.Green,
                BorderThickness = 2,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            cardApto.Cursor = Cursors.Hand;
            cardApto.Click += ResultadoApto_Click;
            form.Controls.Add(cardApto);

            cardApto.Controls.Add(
                UiBuilder.CenterLabel(
                    "✅",
                    0,
                    10,
                    cardW,
                    24,
                    16F,
                    FontStyle.Bold,
                    UiColors.Green
                )
            );

            cardApto.Controls.Add(
                UiBuilder.CenterLabel(
                    "APTO",
                    0,
                    34,
                    cardW,
                    22,
                    10F,
                    FontStyle.Bold,
                    UiColors.Green
                )
            );

            cardInapto = new RoundPanel
            {
                Location = new Point(margem + cardW + gap, 198),
                Size = new Size(cardW, 65),
                Radius = 8,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(235, 239, 244),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            cardInapto.Cursor = Cursors.Hand;
            cardInapto.Click += ResultadoInapto_Click;
            form.Controls.Add(cardInapto);

            cardInapto.Controls.Add(
                UiBuilder.CenterLabel(
                    "✕",
                    0,
                    10,
                    cardW,
                    24,
                    20F,
                    FontStyle.Bold,
                    Color.FromArgb(245, 150, 145)
                )
            );

            cardInapto.Controls.Add(
                UiBuilder.CenterLabel(
                    "INAPTO",
                    0,
                    34,
                    cardW,
                    22,
                    10F,
                    FontStyle.Bold,
                    Color.FromArgb(180, 186, 195)
                )
            );

            SelecionarResultado("Apto");
        }

        private void MontarMedicoObservacoes(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 18;
            int botaoW = 28;

            int medicoW = (largura - (margem * 2) - gap - botaoW - 8) / 2;
            int obsW = largura - (margem * 2) - medicoW - botaoW - gap - 8;

            if (medicoW < 300)
                medicoW = 300;

            if (obsW < 300)
                obsW = 300;

            UiBuilder.AddField(
                form,
                "MÉDICO RESPONSÁVEL",
                cmbMedico = CriarComboMedicos(medicoW),
                margem,
                292,
                medicoW,
                true
            );

            btnAdicionarMedico = UiBuilder.Button(
                "+",
                margem + medicoW + 6,
                316,
                botaoW,
                30,
                Color.White,
                UiColors.AccentBlue
            );

            btnAdicionarMedico.BorderColor = UiColors.Border;
            btnAdicionarMedico.Click += BtnAdicionarMedico_Click;
            form.Controls.Add(btnAdicionarMedico);

            UiBuilder.AddField(
                form,
                "OBSERVAÇÕES / RESTRIÇÕES",
                txtObservacoes = UiBuilder.TextBox("Restrições, observações clínicas ou relação com a CAT...", 0, 0, obsW),
                margem + medicoW + botaoW + gap,
                292,
                obsW,
                false
            );
        }

        private ComboBox CriarComboEmpregados(int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            combo.Items.Add(new ComboItem(0, "Selecione um empregado"));

            try
            {
                foreach (EmpregadoRecord empregado in CadastrosRepository.GetEmpregados())
                {
                    combo.Items.Add(new ComboItem(empregado.Id, empregado.Nome + " - " + empregado.Matricula));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            combo.SelectedIndexChanged += Empregado_SelectedIndexChanged;
            return combo;
        }

        private ComboBox CriarComboCats(int empregadoId, int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            combo.Items.Add(new ComboItem(0, "Sem CAT vinculada"));

            if (empregadoId > 0)
            {
                try
                {
                    foreach (CatRecord cat in CadastrosRepository.GetCatsPorEmpregado(empregadoId))
                    {
                        combo.Items.Add(new ComboItem(cat.Id, "CAT " + cat.Id + " - " + cat.DataAcidente + " - " + cat.Situacao));
                    }
                }
                catch
                {
                    combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
                }
            }

            combo.SelectedIndex = 0;
            combo.SelectedIndexChanged += Cat_SelectedIndexChanged;
            return combo;
        }

        private ComboBox CriarComboTipoAso(int width)
        {
            ComboBox combo = UiBuilder.Combo("Periódico", 0, 0, width);
            combo.Items.Add("Admissional");
            combo.Items.Add("Demissional");
            combo.Items.Add("Retorno ao trabalho");
            combo.Items.Add("Mudança de risco");
            combo.Items.Add("Relacionado a CAT");
            return combo;
        }

        private ComboBox CriarComboMedicos(int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            combo.Items.Add(new ComboItem(0, "Selecione um medico"));

            try
            {
                foreach (MedicoRecord medico in CadastrosRepository.GetMedicos())
                {
                    combo.Items.Add(new ComboItem(medico.Id, medico.Nome + " - " + medico.Crm));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void MontarExamesComplementares(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "EXAMES COMPLEMENTARES REALIZADOS",
                    0,
                    356,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "Nenhum exame complementar vinculado",
                    0,
                    424,
                    largura,
                    34,
                    8.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnAdicionarExame = UiBuilder.SmallButton(
                "+ Adicionar Exame",
                18,
                514,
                125,
                Color.White,
                UiColors.BodyText
            );

            btnAdicionarExame.BorderColor = UiColors.Border;
            btnAdicionarExame.Click += BtnAdicionarExame_Click;
            form.Controls.Add(btnAdicionarExame);
        }

        private void AddExamResult(
            Panel form,
            int largura,
            int y,
            string icon,
            string exam,
            string date,
            string status,
            Color color)
        {
            int margem = 18;
            int rowW = largura - (margem * 2);

            RoundPanel row = new RoundPanel
            {
                Location = new Point(margem, y),
                Size = new Size(rowW, 34),
                Radius = 6,
                FillColor = Color.White,
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(row);

            row.Controls.Add(
                UiBuilder.Label(
                    icon,
                    14,
                    0,
                    26,
                    34,
                    12F,
                    FontStyle.Bold,
                    color
                )
            );

            row.Controls.Add(
                UiBuilder.Label(
                    exam,
                    48,
                    0,
                    rowW - 260,
                    34,
                    8F,
                    FontStyle.Bold,
                    UiColors.BodyText
                )
            );

            Label data = UiBuilder.Label(
                date,
                rowW - 150,
                0,
                80,
                34,
                8F,
                FontStyle.Bold,
                UiColors.MutedText
            );

            data.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            row.Controls.Add(data);

            Label resultado = UiBuilder.Label(
                status,
                rowW - 65,
                0,
                55,
                34,
                8F,
                FontStyle.Bold,
                color
            );

            resultado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            row.Controls.Add(resultado);
        }
    }
}
