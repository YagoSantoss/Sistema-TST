using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class ExamTypesForm
    {
        private RoundButton btnNovo;
        private RoundButton btnExcluir;
        private readonly HashSet<int> _selecionados = new HashSet<int>();

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
            int checkW = 34;
            int tipoW = 150;
            int periodicidadeW = 140;
            int anexoW = 95;
            int acoesW = 90;
            int nomeW = largura - checkW - codigoW - tipoW - periodicidadeW - anexoW - acoesW - 24;

            if (nomeW < 230)
                nomeW = 230;

            int x = 5;

            header.Controls.Add(UiBuilder.HeaderCell("☑", x, 0, checkW));
            x += checkW;
            header.Controls.Add(UiBuilder.HeaderCell("CÓDIGO", x, 0, codigoW));
            x += codigoW;

            header.Controls.Add(UiBuilder.HeaderCell("NOME DO EXAME", x, 0, nomeW));
            x += nomeW;

            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;

            header.Controls.Add(UiBuilder.HeaderCell("PERIODICIDADE", x, 0, periodicidadeW));
            x += periodicidadeW;

            header.Controls.Add(UiBuilder.HeaderCell("ANEXO", x, 0, anexoW));
            x += anexoW;

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
                    AddExamRow(table, largura, y, exame.Id, exame.Codigo, exame.Nome, exame.Tipo, exame.Periodicidade, exame.AnexoImagem);
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
            btnExcluir = UiBuilder.SmallButton("Excluir", 16, 235, 78, Color.White, UiColors.Red);
            btnExcluir.BorderColor = UiColors.Border;
            btnExcluir.Click += BtnExcluir_Click;
            table.Controls.Add(btnExcluir);

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
            string periodicidade,
            string anexoImagem)
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
            int checkW = 34;
            int tipoW = 150;
            int periodicidadeW = 140;
            int anexoW = 95;
            int acoesW = 90;
            int nomeW = largura - checkW - codigoW - tipoW - periodicidadeW - anexoW - acoesW - 24;

            if (nomeW < 230)
                nomeW = 230;

            int x = 5;

            CheckBox check = new CheckBox { Location = new Point(x + 9, 11), Size = new Size(16, 16), Checked = _selecionados.Contains(id), Tag = id, Cursor = Cursors.Hand };
            check.CheckedChanged += Selecionado_CheckedChanged;
            row.Controls.Add(check);
            x += checkW;

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

            if (!string.IsNullOrWhiteSpace(anexoImagem))
            {
                RoundButton verAnexo = UiBuilder.SmallButton(
                    "Ver",
                    x + 8,
                    8,
                    48,
                    Color.White,
                    UiColors.AccentBlue
                );

                verAnexo.BorderColor = UiColors.Border;
                verAnexo.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
                verAnexo.Tag = anexoImagem;
                verAnexo.Click += AbrirAnexo_Click;
                row.Controls.Add(verAnexo);
            }
            else
            {
                row.Controls.Add(UiBuilder.Label("-", x + 8, 2, anexoW, 34, 8F, FontStyle.Regular, UiColors.MutedText));
            }

            x += anexoW;

            RoundButton edit = UiBuilder.SmallButton(
                "✎ Editar",
                x + 8,
                8,
                68,
                Color.White,
                UiColors.BodyText
            );

            edit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            edit.BorderColor = UiColors.Border;
            edit.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            edit.Tag = id;
            edit.Click += BtnEditar_Click;
            row.Controls.Add(edit);
        }

        private void AbrirAnexo_Click(object sender, System.EventArgs e)
        {
            Control control = sender as Control;
            string caminho = control == null ? string.Empty : control.Tag as string;

            if (string.IsNullOrWhiteSpace(caminho) || !File.Exists(caminho))
            {
                MessageBox.Show("O arquivo anexado nao foi encontrado.", "Anexo do exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Process.Start(caminho);
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
