using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class EsocialIntegrationForm
    {
        private RoundButton btnSincronizar;
        private RoundButton btnEnviarTodos;
        private RoundButton btnBuscarLog;
        private CueTextBox txtBuscaLog;
        private string _termoBuscaLog = string.Empty;
        private int _logTop = 390;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "eSocial SST - Simulado",
                "Controle didático e sem transmissão real dos eventos S-2210, S-2220 e S-2240",
                DashboardMenu.Esocial
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoEsocial();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoEsocial();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoEsocial()
        {
            if (_montandoConteudo)
                return;

            _montandoConteudo = true;

            ContentPanel.SuspendLayout();
            ContentPanel.Controls.Clear();

            int margem = 18;
            int espacamento = 18;

            int larguraDisponivel = ContentPanel.ClientSize.Width - (margem * 2);

            if (larguraDisponivel < 790)
                larguraDisponivel = 790;

            int larguraCardMetrica = (larguraDisponivel - (espacamento * 3)) / 4;
            CatStatusResumo resumo = CarregarResumoCats();

            AddMetricCard(
                ContentPanel,
                margem,
                18,
                larguraCardMetrica,
                "CATs GERADAS",
                resumo.Total.ToString(),
                resumo.Total == 1 ? "registro" : "registros",
                UiColors.AccentBlue,
                Color.FromArgb(231, 241, 254)
            );

            AddMetricCard(
                ContentPanel,
                margem + larguraCardMetrica + espacamento,
                18,
                larguraCardMetrica,
                "RETORNOS APTOS",
                resumo.Aptos.ToString(),
                resumo.Aptos == 1 ? "retorno apto" : "retornos aptos",
                UiColors.Green,
                Color.FromArgb(217, 248, 234)
            );

            AddMetricCard(
                ContentPanel,
                margem + ((larguraCardMetrica + espacamento) * 2),
                18,
                larguraCardMetrica,
                "AGUARDANDO ASO",
                resumo.Aguardando.ToString(),
                resumo.Aguardando == 1 ? "pendente" : "pendentes",
                UiColors.Orange,
                Color.FromArgb(255, 246, 206)
            );

            AddMetricCard(
                ContentPanel,
                margem + ((larguraCardMetrica + espacamento) * 3),
                18,
                larguraCardMetrica,
                "RETORNOS INAPTOS",
                resumo.Inaptos.ToString(),
                resumo.Inaptos == 1 ? "retorno inapto" : "retornos inaptos",
                UiColors.Red,
                Color.FromArgb(255, 230, 232)
            );

            MontarPainelEventos(larguraDisponivel);
            MontarPainelLogs(larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void AddMetricCard(
            Control parent,
            int x,
            int y,
            int width,
            string titulo,
            string valor,
            string detalhe,
            Color corPrincipal,
            Color corFundoDetalhe)
        {
            RoundPanel card = UiBuilder.Card(x, y, width, 88);
            card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            parent.Controls.Add(card);

            Panel topLine = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(width, 3),
                BackColor = corPrincipal,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            card.Controls.Add(topLine);

            card.Controls.Add(
                UiBuilder.Label(
                    titulo,
                    18,
                    18,
                    width - 36,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.MutedText
                )
            );

            card.Controls.Add(
                UiBuilder.Label(
                    valor,
                    18,
                    37,
                    width - 36,
                    28,
                    18F,
                    FontStyle.Bold,
                    corPrincipal
                )
            );

            PillLabel pill = new PillLabel
            {
                Text = detalhe,
                Location = new Point(18, 62),
                Size = new Size(width - 36, 20),
                FillColor = corFundoDetalhe,
                ForeColor = corPrincipal,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                Radius = 10
            };

            card.Controls.Add(pill);
        }

        private CatStatusResumo CarregarResumoCats()
        {
            try
            {
                return CadastrosRepository.GetCatStatusResumo();
            }
            catch
            {
                return new CatStatusResumo();
            }
        }

        private void MontarPainelEventos(int largura)
        {
            List<CatRecord> cats = CarregarCatsEsocial();
            List<AsoRecord> asos = CarregarAsosEsocial();
            List<RiskFactorRecord> riscos = CarregarRiscosEsocial();

            int totalEventos = cats.Count + asos.Count + riscos.Count;
            int alturaEventos = 98 + (totalEventos * 42) + 20;
            if (alturaEventos < 310)
                alturaEventos = 310;
            _logTop = 120 + alturaEventos + 20;

            RoundPanel eventsCard = UiBuilder.Card(18, 120, largura, alturaEventos);
            eventsCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(eventsCard);

            eventsCard.Controls.Add(
                UiBuilder.Label(
                    "Eventos SST para aprendizagem e simulação",
                    16,
                    14,
                    largura - 260,
                    22,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            btnEnviarTodos = UiBuilder.SmallButton(
                "Registrar no Log",
                largura - 116,
                13,
                100,
                UiColors.Orange,
                Color.White
            );

            btnEnviarTodos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEnviarTodos.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnEnviarTodos.Click += BtnEnviarTodos_Click;
            eventsCard.Controls.Add(btnEnviarTodos);

            btnSincronizar = UiBuilder.SmallButton(
                "Atualizar",
                largura - 226,
                13,
                102,
                Color.White,
                UiColors.BodyText
            );

            eventsCard.Controls.Add(
                UiBuilder.Label(
                    "Este painel não envia ao governo; ele simula a organização dos eventos usados em sistemas como SOC/TOTVS.",
                    16,
                    32,
                    largura - 260,
                    16,
                    7.3F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnSincronizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSincronizar.BorderColor = UiColors.Border;
            btnSincronizar.Click += BtnSincronizar_Click;
            eventsCard.Controls.Add(btnSincronizar);

            Panel divider = new Panel
            {
                Location = new Point(0, 66),
                Size = new Size(largura, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            eventsCard.Controls.Add(divider);

            try
            {
                if (totalEventos == 0)
                {
                    eventsCard.Controls.Add(UiBuilder.CenterLabel("Nenhum evento SST cadastrado", 0, 145, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 84;
                foreach (CatRecord cat in cats)
                {
                    AddCatEventRow(eventsCard, largura, y, cat);
                    y += 42;
                }

                foreach (AsoRecord aso in asos)
                {
                    AddAsoEventRow(eventsCard, largura, y, aso);
                    y += 42;
                }

                foreach (RiskFactorRecord risco in riscos)
                {
                    AddRiskEventRow(eventsCard, largura, y, risco);
                    y += 42;
                }
            }
            catch
            {
                eventsCard.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar os eventos SST do MySQL", 0, 145, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private List<CatRecord> CarregarCatsEsocial()
        {
            try
            {
                return CadastrosRepository.GetCats(string.Empty);
            }
            catch
            {
                return new List<CatRecord>();
            }
        }

        private List<AsoRecord> CarregarAsosEsocial()
        {
            try
            {
                return CadastrosRepository.GetAsos(string.Empty);
            }
            catch
            {
                return new List<AsoRecord>();
            }
        }

        private List<RiskFactorRecord> CarregarRiscosEsocial()
        {
            try
            {
                return CadastrosRepository.GetFatoresRisco(string.Empty);
            }
            catch
            {
                return new List<RiskFactorRecord>();
            }
        }

        private void MontarPainelLogs(int largura)
        {
            List<EsocialTransmissaoRecord> logs = CarregarLogsEsocial();
            int alturaLog = 96 + (logs.Count * 32) + 28;
            if (alturaLog < 340)
                alturaLog = 340;

            RoundPanel logCard = UiBuilder.Card(18, _logTop, largura, alturaLog);
            logCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(logCard);

            logCard.Controls.Add(
                UiBuilder.Label(
                    "Log interno de transmissões simuladas",
                    16,
                    12,
                    largura - 420,
                    20,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            logCard.Controls.Add(
                UiBuilder.Label(
                    "Use o log para treinar consulta por data, protocolo, recibo e status do evento.",
                    16,
                    29,
                    largura - 420,
                    16,
                    7.2F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            txtBuscaLog = UiBuilder.TextBox("Buscar por data, protocolo ou recibo", largura - 388, 10, 270);
            txtBuscaLog.Text = _termoBuscaLog;
            txtBuscaLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtBuscaLog.KeyDown += TxtBuscaLog_KeyDown;
            logCard.Controls.Add(txtBuscaLog);

            btnBuscarLog = UiBuilder.SmallButton("Buscar", largura - 108, 10, 90, UiColors.Orange, Color.White);
            btnBuscarLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscarLog.Click += BtnBuscarLog_Click;
            logCard.Controls.Add(btnBuscarLog);

            Panel header = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(largura, 28),
                BackColor = UiColors.HeaderBlue,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            logCard.Controls.Add(header);

            int dataW = 150;
            int eventoW = 90;
            int protocoloW = 160;
            int statusW = 100;
            int reciboW = 150;
            int trabalhadorW = largura - dataW - eventoW - protocoloW - statusW - reciboW - 16;

            if (trabalhadorW < 180)
                trabalhadorW = 180;

            int x = 8;

            header.Controls.Add(UiBuilder.HeaderCell("DATA/HORA", x, 0, dataW));
            x += dataW;

            header.Controls.Add(UiBuilder.HeaderCell("EVENTO", x, 0, eventoW));
            x += eventoW;

            header.Controls.Add(UiBuilder.HeaderCell("TRABALHADOR", x, 0, trabalhadorW));
            x += trabalhadorW;

            header.Controls.Add(UiBuilder.HeaderCell("PROTOCOLO", x, 0, protocoloW));
            x += protocoloW;

            header.Controls.Add(UiBuilder.HeaderCell("STATUS", x, 0, statusW));
            x += statusW;

            header.Controls.Add(UiBuilder.HeaderCell("Nº DO RECIBO", x, 0, reciboW));

            try
            {
                if (logs.Count == 0)
                {
                    logCard.Controls.Add(UiBuilder.CenterLabel("Nenhuma transmissão registrada", 0, 96, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 78;
                foreach (EsocialTransmissaoRecord log in logs)
                {
                    AddLogRow(logCard, largura, y, log.DataHora, log.Evento, log.Trabalhador, log.Protocolo, log.Status, log.Recibo, StatusColor(log.Status));
                    y += 32;
                }
            }
            catch
            {
                logCard.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar o log do MySQL", 0, 96, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private List<EsocialTransmissaoRecord> CarregarLogsEsocial()
        {
            try
            {
                return CadastrosRepository.GetEsocialTransmissoes(_termoBuscaLog);
            }
            catch
            {
                return new List<EsocialTransmissaoRecord>();
            }
        }

        private void BtnBuscarLog_Click(object sender, System.EventArgs e)
        {
            _termoBuscaLog = txtBuscaLog == null ? string.Empty : txtBuscaLog.Text.Trim();
            MontarConteudoEsocial();
        }

        private void TxtBuscaLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _termoBuscaLog = txtBuscaLog == null ? string.Empty : txtBuscaLog.Text.Trim();
                MontarConteudoEsocial();
            }
        }

        private void AddCatEventRow(Panel parent, int largura, int y, CatRecord cat)
        {
            string status = string.IsNullOrWhiteSpace(cat.ResultadoAso) ? "Aguardando" : cat.ResultadoAso;
            Color color = StatusColor(status);
            string subtitle = "Acidente em " + cat.DataAcidente + " - " + status;

            AddEventRow(
                parent,
                largura,
                y,
                "S-2210",
                "CAT " + cat.Id + " - " + cat.EmpregadoNome,
                subtitle,
                status,
                color,
                "Abrir CAT",
                delegate { AppNavigator.Show(new CatBasicForm(cat.Id)); }
            );
        }

        private void AddAsoEventRow(Panel parent, int largura, int y, AsoRecord aso)
        {
            string status = string.IsNullOrWhiteSpace(aso.Resultado) ? "Pendente" : aso.Resultado;
            Color color = StatusColor(status);
            string subtitle = aso.DataAso + " - " + aso.TipoExame;

            AddEventRow(
                parent,
                largura,
                y,
                "S-2220",
                "ASO " + aso.Id + " - " + aso.EmpregadoNome,
                subtitle,
                status,
                color,
                "Abrir ASO",
                delegate { AppNavigator.Show(new AsoHistoryForm(aso.EmpregadoId)); }
            );
        }

        private void AddRiskEventRow(Panel parent, int largura, int y, RiskFactorRecord risco)
        {
            string trabalhador = string.IsNullOrWhiteSpace(risco.EmpregadoNome) ? risco.AmbienteNome : risco.EmpregadoNome;
            string subtitle = risco.TipoFator + " - " + risco.Agente;

            AddEventRow(
                parent,
                largura,
                y,
                "S-2240",
                "Fator " + risco.Id + " - " + trabalhador,
                subtitle,
                "Pronto",
                UiColors.AccentBlue,
                "Abrir Risco",
                delegate { AppNavigator.Show(new RiskFactorsForm(risco.Id)); }
            );
        }

        private Color StatusColor(string status)
        {
            string value = (status ?? string.Empty).Trim().ToLowerInvariant();
            if (value.StartsWith("apto"))
                return UiColors.Green;

            if (value.StartsWith("inapto"))
                return UiColors.Red;

            if (value.StartsWith("transmitido") || value.StartsWith("aceito"))
                return UiColors.Green;

            if (value.StartsWith("pronto"))
                return UiColors.AccentBlue;

            return UiColors.Orange;
        }

        private void AddEventRow(
            Panel parent,
            int largura,
            int y,
            string code,
            string title,
            string subtitle,
            string status,
            Color color,
            string actionText,
            System.EventHandler handler)
        {
            int margem = 16;
            int rowW = largura - (margem * 2);

            RoundPanel row = new RoundPanel
            {
                Location = new Point(margem, y),
                Size = new Size(rowW, 42),
                Radius = 8,
                FillColor = Color.White,
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            parent.Controls.Add(row);

            row.Controls.Add(
                UiBuilder.Pill(
                    code,
                    14,
                    10,
                    55,
                    color,
                    Color.White
                )
            );

            int buttonW = 112;
            int statusW = 150;
            int titleX = 85;
            int titleW = rowW - titleX - buttonW - statusW - 45;

            if (titleW < 300)
                titleW = 300;

            row.Controls.Add(
                UiBuilder.Label(
                    title,
                    titleX,
                    4,
                    titleW,
                    20,
                    8.5F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            row.Controls.Add(
                UiBuilder.Label(
                    subtitle,
                    titleX,
                    21,
                    titleW,
                    16,
                    7.3F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            PillLabel statusPill = UiBuilder.Pill(
                status,
                rowW - buttonW - statusW - 20,
                11,
                statusW,
                Color.FromArgb(245, 250, 255),
                color
            );

            statusPill.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            row.Controls.Add(statusPill);

            RoundButton button = UiBuilder.SmallButton(
                actionText,
                rowW - buttonW - 12,
                9,
                buttonW,
                color == UiColors.Orange ? UiColors.Orange : UiColors.AccentBlue,
                Color.White
            );

            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            button.Click += handler;
            row.Controls.Add(button);
        }

        private void AddLogRow(
            Panel parent,
            int largura,
            int y,
            string data,
            string evento,
            string trabalhador,
            string protocolo,
            string status,
            string recibo,
            Color color)
        {
            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(largura, 32),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            parent.Controls.Add(row);

            int dataW = 150;
            int eventoW = 90;
            int protocoloW = 160;
            int statusW = 100;
            int reciboW = 150;
            int trabalhadorW = largura - dataW - eventoW - protocoloW - statusW - reciboW - 16;

            if (trabalhadorW < 180)
                trabalhadorW = 180;

            int x = 8;

            row.Controls.Add(
                UiBuilder.Label(
                    data,
                    x,
                    0,
                    dataW,
                    32,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            x += dataW;

            row.Controls.Add(
                UiBuilder.Pill(
                    evento,
                    x,
                    7,
                    55,
                    evento == "S-2210" ? UiColors.Red : UiColors.AccentBlue,
                    Color.White
                )
            );

            x += eventoW;

            row.Controls.Add(
                UiBuilder.Label(
                    trabalhador,
                    x,
                    0,
                    trabalhadorW,
                    32,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += trabalhadorW;

            row.Controls.Add(
                UiBuilder.Label(
                    protocolo,
                    x,
                    0,
                    protocoloW,
                    32,
                    8F,
                    FontStyle.Regular,
                    UiColors.AccentBlue
                )
            );

            x += protocoloW;

            row.Controls.Add(
                UiBuilder.Pill(
                    status,
                    x,
                    7,
                    76,
                    Color.FromArgb(217, 248, 234),
                    color
                )
            );

            x += statusW;

            row.Controls.Add(
                UiBuilder.Label(
                    recibo,
                    x,
                    0,
                    reciboW,
                    32,
                    8F,
                    FontStyle.Regular,
                    UiColors.AccentBlue
                )
            );
        }
    }
}
