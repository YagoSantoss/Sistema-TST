using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class WorkEnvironmentsForm
    {
        private RoundButton btnNovo;
        private RoundButton btnEditar;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Ambientes de Trabalho",
                "Cadastros Base · Locais e setores produtivos",
                DashboardMenu.WorkEnvironments
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoAmbientes();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoAmbientes();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoAmbientes()
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

            RoundPanel table = UiBuilder.Card(margem, 18, larguraDisponivel, 230);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            MontarCabecalho(table, larguraDisponivel);
            MontarCabecalhoTabela(table, larguraDisponivel);
            MontarLinhas(table, larguraDisponivel);
            MontarRodape(table, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel table, int largura)
        {
            table.Controls.Add(
                UiBuilder.Label(
                    "Ambientes de Trabalho",
                    16,
                    12,
                    largura - 240,
                    20,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            table.Controls.Add(
                UiBuilder.Label(
                    "Ambientes usados nos eventos S-2240 e ASO",
                    16,
                    30,
                    largura - 240,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnNovo = UiBuilder.SmallButton(
                "+ Novo Ambiente",
                largura - 130,
                16,
                112,
                UiColors.AccentBlue,
                Color.White
            );

            btnNovo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovo.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            btnNovo.Click += BtnNovo_Click;
            table.Controls.Add(btnNovo);
        }

        private void MontarCabecalhoTabela(RoundPanel table, int largura)
        {
            Panel header = new Panel
            {
                Location = new Point(0, 55),
                Size = new Size(largura, 28),
                BackColor = UiColors.HeaderBlue,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(header);

            int codigoW = 120;
            int setorW = 220;
            int statusW = 120;
            int acoesW = 90;
            int ambienteW = largura - codigoW - setorW - statusW - acoesW - 24;

            if (ambienteW < 260)
                ambienteW = 260;

            int x = 12;

            header.Controls.Add(UiBuilder.HeaderCell("CÓDIGO", x, 0, codigoW));
            x += codigoW;

            header.Controls.Add(UiBuilder.HeaderCell("AMBIENTE", x, 0, ambienteW));
            x += ambienteW;

            header.Controls.Add(UiBuilder.HeaderCell("SETOR", x, 0, setorW));
            x += setorW;

            header.Controls.Add(UiBuilder.HeaderCell("STATUS", x, 0, statusW));
            x += statusW;

            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<AmbienteTrabalhoRecord> ambientes = CadastrosRepository.GetAmbientes();

                if (ambientes.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum ambiente cadastrado", 0, 112, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 83;
                foreach (AmbienteTrabalhoRecord ambiente in ambientes)
                {
                    AddEnvironmentRow(table, largura, y, ambiente.Id, ambiente.Codigo, ambiente.Ambiente, ambiente.Setor, ambiente.Status);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar os ambientes do MySQL", 0, 112, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void MontarRodape(RoundPanel table, int largura)
        {
            Label total = UiBuilder.Label(
                TotalAmbientesTexto(),
                largura - 180,
                195,
                160,
                20,
                7.5F,
                FontStyle.Regular,
                UiColors.MutedText
            );

            total.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            table.Controls.Add(total);
        }

        private void AddEnvironmentRow(
            Panel table,
            int largura,
            int y,
            int id,
            string codigo,
            string ambiente,
            string setor,
            string status)
        {
            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(largura, 38),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(row);

            int codigoW = 120;
            int setorW = 220;
            int statusW = 120;
            int acoesW = 90;
            int ambienteW = largura - codigoW - setorW - statusW - acoesW - 24;

            if (ambienteW < 260)
                ambienteW = 260;

            int x = 12;

            row.Controls.Add(
                UiBuilder.Label(
                    codigo,
                    x,
                    2,
                    codigoW,
                    34,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            x += codigoW;

            row.Controls.Add(
                UiBuilder.Label(
                    ambiente,
                    x,
                    2,
                    ambienteW,
                    34,
                    8F,
                    FontStyle.Bold,
                    UiColors.BodyText
                )
            );

            x += ambienteW;

            row.Controls.Add(
                UiBuilder.Label(
                    setor,
                    x,
                    2,
                    setorW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += setorW;

            row.Controls.Add(
                UiBuilder.Pill(
                    status,
                    x + 5,
                    9,
                    72,
                    status == "Ativo" ? Color.FromArgb(217, 248, 234) : UiColors.OrangeSoft,
                    status == "Ativo" ? UiColors.Green : UiColors.Orange
                )
            );

            x += statusW;

            btnEditar = UiBuilder.SmallButton(
                "✎ Editar",
                x + 8,
                8,
                68,
                Color.White,
                UiColors.BodyText
            );

            btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditar.BorderColor = UiColors.Border;
            btnEditar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnEditar.Tag = id;
            btnEditar.Click += BtnEditar_Click;
            row.Controls.Add(btnEditar);
        }

        private string TotalAmbientesTexto()
        {
            try
            {
                int total = CadastrosRepository.GetAmbientes().Count;
                return total + (total == 1 ? " ambiente cadastrado" : " ambientes cadastrados");
            }
            catch
            {
                return "MySQL indisponivel";
            }
        }
    }
}
