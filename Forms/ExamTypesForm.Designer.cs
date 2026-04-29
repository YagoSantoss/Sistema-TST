using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ExamTypesForm
    {
        private RoundButton btnNovo;
        private RoundButton btnInserir;
        private RoundButton btnEditar;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Tipos de Exame",
                "Cadastros Base · Exames complementares",
                DashboardMenu.ExamTypes
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoTiposExame();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoTiposExame();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoTiposExame()
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

            RoundPanel table = UiBuilder.Card(margem, 18, larguraDisponivel, 275);
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
                    "Tipos de Exame",
                    16,
                    12,
                    largura - 220,
                    20,
                    9F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            table.Controls.Add(
                UiBuilder.Label(
                    "Exames laboratoriais e clínicos cadastrados",
                    16,
                    30,
                    largura - 220,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnNovo = UiBuilder.SmallButton(
                "+ Novo Exame",
                largura - 110,
                16,
                94,
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
            int tipoW = 190;
            int periodicidadeW = 170;
            int acoesW = 90;
            int nomeW = largura - codigoW - tipoW - periodicidadeW - acoesW - 24;

            if (nomeW < 230)
                nomeW = 230;

            int x = 12;

            header.Controls.Add(UiBuilder.HeaderCell("CÓDIGO", x, 0, codigoW));
            x += codigoW;

            header.Controls.Add(UiBuilder.HeaderCell("NOME DO EXAME", x, 0, nomeW));
            x += nomeW;

            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;

            header.Controls.Add(UiBuilder.HeaderCell("PERIODICIDADE", x, 0, periodicidadeW));
            x += periodicidadeW;

            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<TipoExameRecord> exames = CadastrosRepository.GetTiposExames();

                if (exames.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum exame cadastrado", 0, 126, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 83;
                foreach (TipoExameRecord exame in exames)
                {
                    AddExamRow(table, largura, y, exame.Id, exame.Codigo, exame.Nome, exame.Tipo, exame.Periodicidade);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar os exames do MySQL", 0, 126, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void MontarRodape(RoundPanel table, int largura)
        {
            btnInserir = UiBuilder.SmallButton(
                "+ Inserir",
                16,
                235,
                70,
                UiColors.AccentBlue,
                Color.White
            );

            btnInserir.Click += BtnNovo_Click;
            table.Controls.Add(btnInserir);

            btnEditar = UiBuilder.SmallButton(
                "✎ Editar",
                94,
                235,
                70,
                Color.White,
                UiColors.BodyText
            );

            btnEditar.BorderColor = UiColors.Border;
            btnEditar.Click += BtnEditar_Click;
            table.Controls.Add(btnEditar);

            Label total = UiBuilder.Label(
                TotalExamesTexto(),
                largura - 170,
                237,
                150,
                20,
                7.5F,
                FontStyle.Regular,
                UiColors.MutedText
            );

            total.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            table.Controls.Add(total);
        }

        private void AddExamRow(
            Panel table,
            int largura,
            int y,
            int id,
            string codigo,
            string nome,
            string tipo,
            string periodicidade)
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
            int tipoW = 190;
            int periodicidadeW = 170;
            int acoesW = 90;
            int nomeW = largura - codigoW - tipoW - periodicidadeW - acoesW - 24;

            if (nomeW < 230)
                nomeW = 230;

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
                    nome,
                    x,
                    2,
                    nomeW,
                    34,
                    8F,
                    FontStyle.Bold,
                    UiColors.BodyText
                )
            );

            x += nomeW;

            row.Controls.Add(
                UiBuilder.Label(
                    tipo,
                    x,
                    2,
                    tipoW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += tipoW;

            row.Controls.Add(
                UiBuilder.Label(
                    periodicidade,
                    x,
                    2,
                    periodicidadeW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += periodicidadeW;

            RoundButton edit = UiBuilder.SmallButton(
                "✎",
                x + 18,
                7,
                34,
                Color.White,
                UiColors.BodyText
            );

            edit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            edit.BorderColor = UiColors.Border;
            edit.Tag = id;
            edit.Click += BtnEditar_Click;
            row.Controls.Add(edit);
        }

        private string TotalExamesTexto()
        {
            try
            {
                int total = CadastrosRepository.GetTiposExames().Count;
                return total + (total == 1 ? " exame cadastrado" : " exames cadastrados");
            }
            catch
            {
                return "MySQL indisponivel";
            }
        }
    }
}
