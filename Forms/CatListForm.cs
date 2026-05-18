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
        private readonly HashSet<int> _selecionados = new HashSet<int>();

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

            int alturaTabela = CalcularAlturaTabela();
            RoundPanel table = UiBuilder.Card(margem, 18, largura, alturaTabela);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            table.Controls.Add(UiBuilder.Label("CAT cadastradas", 16, 12, largura - 240, 20, 9F, FontStyle.Bold, UiColors.AccentBlue));
            table.Controls.Add(UiBuilder.Label("Pesquise, abra ou cadastre comunicacoes de acidente de trabalho", 16, 30, largura - 240, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            RoundButton novo = UiBuilder.SmallButton("+ Nova CAT", largura - 105, 16, 88, UiColors.AccentBlue, Color.White);
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (sender, e) =>
            {
                CatDraftState.Clear();
                AppNavigator.Show(new CatBasicForm());
            };
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
            MontarRodape(table, largura, alturaTabela);

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
            int checkW = 34;
            int tipoW = 120;
            int situacaoW = 110;
            int resultadoW = 130;
            int acoesW = 160;
            int empregadoW = (int)(largura * 0.28);
            int localW = largura - checkW - empregadoW - dataW - tipoW - situacaoW - resultadoW - acoesW - 20;
            int x = 5;

            header.Controls.Add(UiBuilder.HeaderCell("SEL.", x, 0, checkW));
            x += checkW;
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
            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
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
                table.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar as CATs do MySQL", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
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
            int checkW = 34;
            int tipoW = 120;
            int situacaoW = 110;
            int resultadoW = 130;
            int acoesW = 160;
            int empregadoW = (int)(largura * 0.28);
            int localW = largura - checkW - empregadoW - dataW - tipoW - situacaoW - resultadoW - acoesW - 20;
            int x = 5;

            CheckBox check = new CheckBox
            {
                Location = new Point(x + 9, 11),
                Size = new Size(16, 16),
                Checked = _selecionados.Contains(cat.Id),
                Tag = cat.Id,
                Cursor = Cursors.Hand
            };
            check.CheckedChanged += Selecionado_CheckedChanged;
            row.Controls.Add(check);
            x += checkW;

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

            RoundButton abrir = UiBuilder.SmallButton("Abrir", x + 8, 7, 50, Color.White, UiColors.BodyText);
            abrir.BorderColor = UiColors.Border;
            abrir.Tag = cat.Id;
            abrir.Click += (sender, e) => AppNavigator.Show(new CatBasicForm((int)((Control)sender).Tag));
            row.Controls.Add(abrir);

            RoundButton imprimir = UiBuilder.SmallButton("Imprimir", x + 64, 7, 76, UiColors.Orange, Color.White);
            imprimir.Tag = cat.Id;
            imprimir.Click += ImprimirCat_Click;
            row.Controls.Add(imprimir);
        }

        private int CalcularAlturaTabela()
        {
            try
            {
                int registros = CadastrosRepository.GetCats(_termoBusca).Count;
                int altura = 138 + (registros * 38) + 70;
                return altura < 460 ? 460 : altura;
            }
            catch
            {
                return 460;
            }
        }

        private void MontarRodape(RoundPanel table, int largura, int alturaTabela)
        {
            RoundButton excluir = UiBuilder.SmallButton("Excluir", 16, alturaTabela - 40, 78, Color.White, UiColors.Red);
            excluir.BorderColor = UiColors.Border;
            excluir.Click += ExcluirSelecionados_Click;
            table.Controls.Add(excluir);
        }

        private void Selecionado_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check == null || !(check.Tag is int))
                return;

            int id = (int)check.Tag;
            if (check.Checked)
                _selecionados.Add(id);
            else
                _selecionados.Remove(id);

            if (check.Parent != null)
                check.Parent.BackColor = check.Checked ? Color.FromArgb(255, 244, 229) : Color.White;
        }

        private void ExcluirSelecionados_Click(object sender, EventArgs e)
        {
            if (_selecionados.Count == 0)
            {
                MessageBox.Show("Selecione uma ou mais CATs para excluir.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir as CATs selecionadas?", "CAT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                CadastrosRepository.DeleteCats(new List<int>(_selecionados));
                _selecionados.Clear();
                MontarConteudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível excluir no MySQL.\n\n" + ex.Message, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprimirCat_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)((Control)sender).Tag;
                CatRecord cat = CadastrosRepository.GetCat(id);
                if (cat == null)
                {
                    MessageBox.Show("CAT não encontrada.", "CAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string arquivo = CatPdfExporter.Exportar(cat);
                System.Diagnostics.Process.Start(arquivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível gerar o PDF da CAT.\n\n" + ex.Message, "CAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
