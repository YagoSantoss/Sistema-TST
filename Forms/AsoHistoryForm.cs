using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class AsoHistoryForm : DashboardFormBase
    {
        private readonly int _empregadoId;

        public AsoHistoryForm(int empregadoId)
        {
            _empregadoId = empregadoId;
            BuildDashboardShell("Histórico de ASO", "ASOs registrados para o empregado", DashboardMenu.Aso);
            ContentPanel.AutoScroll = true;
            MontarConteudo();
            ContentPanel.Resize += (sender, e) => MontarConteudo();
        }

        private void MontarConteudo()
        {
            ContentPanel.SuspendLayout();
            ContentPanel.Controls.Clear();

            int margem = 18;
            int largura = ContentPanel.ClientSize.Width - (margem * 2);
            if (largura < 790)
                largura = 790;

            RoundPanel table = UiBuilder.Card(margem, 18, largura, 330);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            table.Controls.Add(UiBuilder.Label("ASOs do empregado", 16, 12, largura - 260, 20, 9F, FontStyle.Bold, UiColors.AccentBlue));
            table.Controls.Add(UiBuilder.Label("Consulte o resultado apto/inapto e a CAT vinculada", 16, 30, largura - 260, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            RoundButton novo = UiBuilder.SmallButton("+ Novo ASO", largura - 105, 16, 88, UiColors.AccentBlue, Color.White);
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (sender, e) => AppNavigator.Show(new AsoForm());
            table.Controls.Add(novo);

            MontarCabecalho(table, largura);
            MontarLinhas(table, largura);

            ContentPanel.ResumeLayout(false);
        }

        private void MontarCabecalho(RoundPanel table, int largura)
        {
            Panel header = new Panel
            {
                Location = new Point(0, 62),
                Size = new Size(largura, 28),
                BackColor = UiColors.HeaderBlue,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(header);

            int dataW = 120;
            int resultadoW = 120;
            int tipoW = 160;
            int catW = 90;
            int medicoW = 220;
            int obsW = largura - dataW - resultadoW - tipoW - catW - medicoW - 20;

            if (obsW < 180)
                obsW = 180;

            int x = 12;
            header.Controls.Add(UiBuilder.HeaderCell("DATA", x, 0, dataW));
            x += dataW;
            header.Controls.Add(UiBuilder.HeaderCell("RESULTADO", x, 0, resultadoW));
            x += resultadoW;
            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;
            header.Controls.Add(UiBuilder.HeaderCell("CAT", x, 0, catW));
            x += catW;
            header.Controls.Add(UiBuilder.HeaderCell("MÉDICO", x, 0, medicoW));
            x += medicoW;
            header.Controls.Add(UiBuilder.HeaderCell("OBSERVAÇÕES", x, 0, obsW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<AsoRecord> asos = CadastrosRepository.GetAsosPorEmpregado(_empregadoId);

                if (asos.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum ASO registrado para este empregado", 0, 145, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 90;
                foreach (AsoRecord aso in asos)
                {
                    AddAsoRow(table, largura, y, aso);
                    y += 38;
                }
            }
            catch (Exception ex)
            {
                table.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar ASOs: " + ex.Message, 0, 145, largura, 34, 8F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void AddAsoRow(Panel table, int largura, int y, AsoRecord aso)
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
            int resultadoW = 120;
            int tipoW = 160;
            int catW = 90;
            int medicoW = 220;
            int obsW = largura - dataW - resultadoW - tipoW - catW - medicoW - 20;

            if (obsW < 180)
                obsW = 180;

            int x = 12;
            row.Controls.Add(UiBuilder.Cell(aso.DataAso, x, 2, dataW, UiColors.AccentBlue, FontStyle.Bold));
            x += dataW;
            row.Controls.Add(UiBuilder.Pill(aso.Resultado, x + 12, 9, 80, ResultadoBack(aso.Resultado), ResultadoColor(aso.Resultado)));
            x += resultadoW;
            row.Controls.Add(UiBuilder.Label(aso.TipoExame, x, 2, tipoW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += tipoW;
            row.Controls.Add(UiBuilder.Cell(aso.CatId.HasValue ? "CAT " + aso.CatId.Value : "-", x, 2, catW, UiColors.BodyText, FontStyle.Bold));
            x += catW;
            row.Controls.Add(UiBuilder.Label(aso.MedicoNome, x, 2, medicoW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += medicoW;
            row.Controls.Add(UiBuilder.Label(aso.Observacoes, x, 2, obsW, 34, 8F, FontStyle.Regular, UiColors.MutedText));
        }

        private Color ResultadoColor(string resultado)
        {
            return resultado == "Apto" ? UiColors.Green : UiColors.Red;
        }

        private Color ResultadoBack(string resultado)
        {
            return resultado == "Apto" ? Color.FromArgb(217, 248, 234) : Color.FromArgb(255, 233, 232);
        }
    }
}
