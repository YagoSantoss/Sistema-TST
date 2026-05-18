using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class AsoListForm : DashboardFormBase
    {
        private CueTextBox txtBusca;
        private string _termoBusca = string.Empty;
        private bool _montandoConteudo;
        private readonly HashSet<int> _selecionados = new HashSet<int>();

        public AsoListForm()
        {
            BuildDashboardShell("Monitoramento da Saúde / ASO", "S-2220 - Consulta e registro de ASO", DashboardMenu.Aso);
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

            table.Controls.Add(UiBuilder.Label("ASOs registrados", 16, 12, largura - 240, 20, 9F, FontStyle.Bold, UiColors.AccentBlue));
            table.Controls.Add(UiBuilder.Label("Pesquise, consulte ou registre monitoramentos de saúde ocupacional", 16, 30, largura - 240, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            RoundButton novo = UiBuilder.SmallButton("+ Novo ASO", largura - 105, 16, 88, UiColors.AccentBlue, Color.White);
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (sender, e) => AppNavigator.Show(new AsoForm());
            table.Controls.Add(novo);

            txtBusca = UiBuilder.TextBox("Buscar por empregado, médico, exame ou resultado", 16, 62, largura - 130);
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
            MontarRodape(table, alturaTabela);

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
            int tipoW = 160;
            int medicoW = 210;
            int resultadoW = 120;
            int acoesW = 160;
            int empregadoW = largura - checkW - dataW - tipoW - medicoW - resultadoW - acoesW - 20;
            int x = 5;

            header.Controls.Add(UiBuilder.HeaderCell("SEL.", x, 0, checkW));
            x += checkW;
            header.Controls.Add(UiBuilder.HeaderCell("EMPREGADO", x, 0, empregadoW));
            x += empregadoW;
            header.Controls.Add(UiBuilder.HeaderCell("DATA", x, 0, dataW));
            x += dataW;
            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;
            header.Controls.Add(UiBuilder.HeaderCell("MÉDICO", x, 0, medicoW));
            x += medicoW;
            header.Controls.Add(UiBuilder.HeaderCell("RESULTADO", x, 0, resultadoW));
            x += resultadoW;
            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<AsoRecord> asos = CadastrosRepository.GetAsos(_termoBusca);

                if (asos.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum ASO registrado", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 138;
                foreach (AsoRecord aso in asos)
                {
                    AddAsoRow(table, largura, y, aso);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar os ASOs do MySQL", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void AddAsoRow(Panel table, int largura, int y, AsoRecord aso)
        {
            Panel row = new Panel { Location = new Point(0, y), Size = new Size(largura, 38), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(row);

            int dataW = 120;
            int checkW = 34;
            int tipoW = 160;
            int medicoW = 210;
            int resultadoW = 120;
            int acoesW = 160;
            int empregadoW = largura - checkW - dataW - tipoW - medicoW - resultadoW - acoesW - 20;
            int x = 5;

            CheckBox check = new CheckBox { Location = new Point(x + 9, 11), Size = new Size(16, 16), Checked = _selecionados.Contains(aso.Id), Tag = aso.Id, Cursor = Cursors.Hand };
            check.CheckedChanged += Selecionado_CheckedChanged;
            row.Controls.Add(check);
            x += checkW;

            row.Controls.Add(UiBuilder.Label(aso.EmpregadoNome, x, 2, empregadoW, 34, 8F, FontStyle.Bold, UiColors.BodyText));
            x += empregadoW;
            row.Controls.Add(UiBuilder.Cell(aso.DataAso, x, 2, dataW, UiColors.AccentBlue, FontStyle.Bold));
            x += dataW;
            row.Controls.Add(UiBuilder.Label(aso.TipoExame, x, 2, tipoW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += tipoW;
            row.Controls.Add(UiBuilder.Label(aso.MedicoNome, x, 2, medicoW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += medicoW;
            row.Controls.Add(UiBuilder.Pill(aso.Resultado, x + 8, 9, 90, ResultadoBack(aso.Resultado), ResultadoColor(aso.Resultado)));
            x += resultadoW;

            RoundButton abrir = UiBuilder.SmallButton("Abrir", x + 8, 7, 50, Color.White, UiColors.BodyText);
            abrir.BorderColor = UiColors.Border;
            abrir.Tag = aso.EmpregadoId;
            abrir.Click += (sender, e) => AppNavigator.Show(new AsoHistoryForm((int)((Control)sender).Tag));
            row.Controls.Add(abrir);

            RoundButton imprimir = UiBuilder.SmallButton("Imprimir", x + 64, 7, 76, UiColors.Orange, Color.White);
            imprimir.Tag = aso.Id;
            imprimir.Click += ImprimirAso_Click;
            row.Controls.Add(imprimir);
        }

        private int CalcularAlturaTabela()
        {
            try
            {
                int registros = CadastrosRepository.GetAsos(_termoBusca).Count;
                int altura = 138 + (registros * 38) + 70;
                return altura < 460 ? 460 : altura;
            }
            catch
            {
                return 460;
            }
        }

        private void MontarRodape(RoundPanel table, int alturaTabela)
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
                MessageBox.Show("Selecione um ou mais ASOs para excluir.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir os ASOs selecionados?", "ASO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                CadastrosRepository.DeleteAsos(new List<int>(_selecionados));
                _selecionados.Clear();
                MontarConteudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível excluir no MySQL.\n\n" + ex.Message, "ASO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprimirAso_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)((Control)sender).Tag;
                AsoRecord aso = CadastrosRepository.GetAso(id);
                if (aso == null)
                {
                    MessageBox.Show("ASO não encontrado.", "ASO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string arquivo = OccupationalPdfExporter.ExportarAso(aso);
                System.Diagnostics.Process.Start(arquivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível gerar o PDF do ASO.\n\n" + ex.Message, "ASO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Color ResultadoColor(string resultado)
        {
            return resultado == "Inapto" ? UiColors.Red : UiColors.Green;
        }

        private Color ResultadoBack(string resultado)
        {
            return resultado == "Inapto" ? Color.FromArgb(255, 233, 232) : Color.FromArgb(217, 248, 234);
        }
    }
}
