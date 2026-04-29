using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class CatListForm : DashboardFormBase
    {
        private CueTextBox txtBusca;
        private string _termoBusca = string.Empty;
        private bool _montandoConteudo;

        public CatListForm()
        {
            BuildDashboardShell("CAT - Acidente de Trabalho", "S-2210 - Consulta e cadastro de CAT", DashboardMenu.Cat);
            ContentPanel.AutoScroll = true;
            MontarConteudo();
            ContentPanel.Resize += (sender, e) => MontarConteudo();
        }

        private void MontarConteudo()
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

            RoundPanel table = UiBuilder.Card(margem, 18, largura, 420);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            table.Controls.Add(UiBuilder.Label("CAT cadastradas", 16, 12, largura - 240, 20, 9F, FontStyle.Bold, UiColors.AccentBlue));
            table.Controls.Add(UiBuilder.Label("Pesquise, abra ou cadastre comunicacoes de acidente de trabalho", 16, 30, largura - 240, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            RoundButton novo = UiBuilder.SmallButton("+ Nova CAT", largura - 105, 16, 88, UiColors.AccentBlue, Color.White);
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (sender, e) => AppNavigator.Show(new CatBasicForm());
            table.Controls.Add(novo);

            txtBusca = UiBuilder.TextBox("Buscar por empregado, matricula, tipo ou situacao", 16, 62, largura - 130);
            txtBusca.Text = _termoBusca;
            txtBusca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            table.Controls.Add(txtBusca);

            RoundButton buscar = UiBuilder.SmallButton("Buscar", largura - 100, 65, 82, UiColors.Orange, Color.White);
            buscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buscar.Click += (sender, e) =>
            {
                _termoBusca = txtBusca.Text.Trim();
                MontarConteudo();
            };
            table.Controls.Add(buscar);

            MontarCabecalho(table, largura);
            MontarLinhas(table, largura);

            ContentPanel.ResumeLayout(false);
            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel table, int largura)
        {
            Panel header = new Panel
            {
                Location = new Point(0, 110),
                Size = new Size(largura, 28),
                BackColor = UiColors.HeaderBlue,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(header);

            int dataW = 120;
            int tipoW = 120;
            int situacaoW = 110;
            int resultadoW = 130;
            int acoesW = 90;
            int empregadoW = (int)(largura * 0.28);
            int localW = largura - empregadoW - dataW - tipoW - situacaoW - resultadoW - acoesW - 20;
            int x = 12;

            header.Controls.Add(UiBuilder.HeaderCell("EMPREGADO", x, 0, empregadoW));
            x += empregadoW;
            header.Controls.Add(UiBuilder.HeaderCell("DATA", x, 0, dataW));
            x += dataW;
            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;
            header.Controls.Add(UiBuilder.HeaderCell("LOCAL", x, 0, localW));
            x += localW;
            header.Controls.Add(UiBuilder.HeaderCell("SITUACAO", x, 0, situacaoW));
            x += situacaoW;
            header.Controls.Add(UiBuilder.HeaderCell("RESULTADO ASO", x, 0, resultadoW));
            x += resultadoW;
            header.Controls.Add(UiBuilder.HeaderCell("ACOES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<CatRecord> cats = CadastrosRepository.GetCats(_termoBusca);

                if (cats.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhuma CAT cadastrada", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 138;
                foreach (CatRecord cat in cats)
                {
                    AddCatRow(table, largura, y, cat);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar as CATs do MySQL", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void AddCatRow(Panel table, int largura, int y, CatRecord cat)
        {
            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(largura, 38),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(row);

            int dataW = 120;
            int tipoW = 120;
            int situacaoW = 110;
            int resultadoW = 130;
            int acoesW = 90;
            int empregadoW = (int)(largura * 0.28);
            int localW = largura - empregadoW - dataW - tipoW - situacaoW - resultadoW - acoesW - 20;
            int x = 12;

            row.Controls.Add(UiBuilder.Label(cat.EmpregadoNome, x, 2, empregadoW, 34, 8F, FontStyle.Bold, UiColors.BodyText));
            x += empregadoW;
            row.Controls.Add(UiBuilder.Cell(cat.DataAcidente, x, 2, dataW, UiColors.AccentBlue, FontStyle.Bold));
            x += dataW;
            row.Controls.Add(UiBuilder.Cell(cat.TipoCat, x, 2, tipoW, UiColors.BodyText, FontStyle.Regular));
            x += tipoW;
            row.Controls.Add(UiBuilder.Label(cat.LocalAcidente, x, 2, localW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += localW;
            row.Controls.Add(UiBuilder.Pill(cat.Situacao, x + 8, 9, 80, UiColors.OrangeSoft, UiColors.Orange));
            x += situacaoW;
            row.Controls.Add(UiBuilder.Pill(cat.ResultadoAso, x + 6, 9, 110, ResultadoAsoBack(cat.ResultadoAso), ResultadoAsoColor(cat.ResultadoAso)));
            x += resultadoW;

            RoundButton abrir = UiBuilder.SmallButton("Abrir", x + 12, 7, 58, Color.White, UiColors.BodyText);
            abrir.BorderColor = UiColors.Border;
            abrir.Tag = cat.Id;
            abrir.Click += (sender, e) => AppNavigator.Show(new CatBasicForm((int)((Control)sender).Tag));
            row.Controls.Add(abrir);
        }

        private Color ResultadoAsoColor(string resultado)
        {
            if (resultado == "Apto")
                return UiColors.Green;

            if (resultado == "Inapto")
                return UiColors.Red;

            return UiColors.MutedText;
        }

        private Color ResultadoAsoBack(string resultado)
        {
            if (resultado == "Apto")
                return Color.FromArgb(217, 248, 234);

            if (resultado == "Inapto")
                return Color.FromArgb(255, 233, 232);

            return Color.FromArgb(245, 250, 255);
        }
    }
}
