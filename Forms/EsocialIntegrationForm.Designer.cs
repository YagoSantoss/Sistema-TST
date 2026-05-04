using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class EsocialIntegrationForm
    {
        private RoundButton btnSincronizar;
        private RoundButton btnEnviarTodos;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Painel de Gestão",
                "Indicadores de CAT e ASO",
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
                resumo.Total == 1 ? "1 registro" : resumo.Total + " registros",
                UiColors.AccentBlue,
                Color.FromArgb(231, 241, 254)
            );

            AddMetricCard(
                ContentPanel,
                margem + larguraCardMetrica + espacamento,
                18,
                larguraCardMetrica,
                "APTOS",
                resumo.Aptos.ToString(),
                resumo.Aptos == 1 ? "1 CAT apta" : resumo.Aptos + " CATs aptas",
                UiColors.Green,
                Color.FromArgb(217, 248, 234)
            );

            AddMetricCard(
                ContentPanel,
                margem + ((larguraCardMetrica + espacamento) * 2),
                18,
                larguraCardMetrica,
                "AGUARDANDO",
                resumo.Aguardando.ToString(),
                resumo.Aguardando == 1 ? "1 pendente" : resumo.Aguardando + " pendentes",
                UiColors.Orange,
                Color.FromArgb(255, 246, 206)
            );

            AddMetricCard(
                ContentPanel,
                margem + ((larguraCardMetrica + espacamento) * 3),
                18,
                larguraCardMetrica,
                "INAPTOS",
                resumo.Inaptos.ToString(),
                resumo.Inaptos == 1 ? "1 CAT inapta" : resumo.Inaptos + " CATs inaptas",
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
            RoundPanel card = UiBuilder.Card(x, y, width, 84);
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
                    30,
                    18F,
                    FontStyle.Bold,
                    corPrincipal
                )
            );

            PillLabel pill = new PillLabel
            {
                Text = detalhe,
                Location = new Point(18, 62),
                Size = new Size(120, 18),
                FillColor = corFundoDetalhe,
                ForeColor = corPrincipal,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                Radius = 9
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
            RoundPanel eventsCard = UiBuilder.Card(18, 120, largura, 250);
            eventsCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(eventsCard);

            eventsCard.Controls.Add(
                UiBuilder.Label(
                    "📊 Painel de Gestão — Indicadores de CAT",
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
                "📤 Enviar Todos",
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
                "🔄 Sincronizar",
                largura - 226,
                13,
                102,
                Color.White,
                UiColors.BodyText
            );

            btnSincronizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSincronizar.BorderColor = UiColors.Border;
            btnSincronizar.Click += BtnSincronizar_Click;
            eventsCard.Controls.Add(btnSincronizar);

            Panel divider = new Panel
            {
                Location = new Point(0, 52),
                Size = new Size(largura, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            eventsCard.Controls.Add(divider);

            eventsCard.Controls.Add(
                UiBuilder.CenterLabel(
                    "Nenhuma CAT gerada",
                    0,
                    130,
                    largura,
                    34,
                    8.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );
        }

        private void MontarPainelLogs(int largura)
        {
            RoundPanel logCard = UiBuilder.Card(18, 390, largura, 155);
            logCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(logCard);

            logCard.Controls.Add(
                UiBuilder.Label(
                    "📋 Log de Transmissões",
                    16,
                    12,
                    largura - 32,
                    20,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            Panel header = new Panel
            {
                Location = new Point(0, 44),
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

            logCard.Controls.Add(
                UiBuilder.CenterLabel(
                    "Nenhuma transmissao registrada",
                    0,
                    88,
                    largura,
                    34,
                    8.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );
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
            int statusW = 90;
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
