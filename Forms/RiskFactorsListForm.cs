using System.Collections.Generic;
using System.Drawing;
using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class RiskFactorsListForm : DashboardFormBase
    {
        private CueTextBox txtBusca;
        private string _termoBusca = string.Empty;
        private bool _montandoConteudo;
        private readonly HashSet<int> _selecionados = new HashSet<int>();

        public RiskFactorsListForm()
        {
            BuildDashboardShell("Fatores de Risco Ambiental", "S-2240 - Consulta e cadastro de fatores", DashboardMenu.Risk);
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

            table.Controls.Add(UiBuilder.Label("Fatores de risco cadastrados", 16, 12, largura - 260, 20, 9F, FontStyle.Bold, UiColors.AccentBlue));
            table.Controls.Add(UiBuilder.Label("Pesquise, consulte ou cadastre condições ambientais de trabalho", 16, 30, largura - 260, 16, 7.5F, FontStyle.Regular, UiColors.MutedText));

            RoundButton novo = UiBuilder.SmallButton("+ Novo S-2240", largura - 120, 16, 102, UiColors.AccentBlue, Color.White);
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (sender, e) => AppNavigator.Show(new RiskFactorsForm());
            table.Controls.Add(novo);

            txtBusca = UiBuilder.TextBox("Buscar por empregado, ambiente, tipo ou agente", 16, 62, largura - 130);
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
            Panel header = new Panel { Location = new Point(0, 110), Size = new Size(largura, 28), BackColor = UiColors.HeaderBlue, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(header);

            int dataW = 120;
            int checkW = 34;
            int tipoW = 140;
            int ambienteW = 190;
            int acoesW = 160;
            int empregadoW = (int)(largura * 0.24);
            int agenteW = largura - checkW - empregadoW - ambienteW - tipoW - dataW - acoesW - 20;
            int x = 5;

            header.Controls.Add(UiBuilder.HeaderCell("SEL.", x, 0, checkW));
            x += checkW;
            header.Controls.Add(UiBuilder.HeaderCell("EMPREGADO", x, 0, empregadoW));
            x += empregadoW;
            header.Controls.Add(UiBuilder.HeaderCell("AMBIENTE", x, 0, ambienteW));
            x += ambienteW;
            header.Controls.Add(UiBuilder.HeaderCell("TIPO", x, 0, tipoW));
            x += tipoW;
            header.Controls.Add(UiBuilder.HeaderCell("AGENTE", x, 0, agenteW));
            x += agenteW;
            header.Controls.Add(UiBuilder.HeaderCell("AVALIAÇÃO", x, 0, dataW));
            x += dataW;
            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<RiskFactorRecord> riscos = CadastrosRepository.GetFatoresRisco(_termoBusca);

                if (riscos.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum fator de risco cadastrado", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 138;
                foreach (RiskFactorRecord risco in riscos)
                {
                    AddRiskRow(table, largura, y, risco);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar fatores de risco do MySQL", 0, 190, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void AddRiskRow(Panel table, int largura, int y, RiskFactorRecord risco)
        {
            Panel row = new Panel { Location = new Point(0, y), Size = new Size(largura, 38), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(row);

            int dataW = 120;
            int checkW = 34;
            int tipoW = 140;
            int ambienteW = 190;
            int acoesW = 160;
            int empregadoW = (int)(largura * 0.24);
            int agenteW = largura - checkW - empregadoW - ambienteW - tipoW - dataW - acoesW - 20;
            int x = 5;

            CheckBox check = new CheckBox { Location = new Point(x + 9, 11), Size = new Size(16, 16), Checked = _selecionados.Contains(risco.Id), Tag = risco.Id, Cursor = Cursors.Hand };
            check.CheckedChanged += Selecionado_CheckedChanged;
            row.Controls.Add(check);
            x += checkW;

            row.Controls.Add(UiBuilder.Label(risco.EmpregadoNome, x, 2, empregadoW, 34, 8F, FontStyle.Bold, UiColors.BodyText));
            x += empregadoW;
            row.Controls.Add(UiBuilder.Label(risco.AmbienteNome, x, 2, ambienteW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += ambienteW;
            row.Controls.Add(UiBuilder.Cell(risco.TipoFator, x, 2, tipoW, UiColors.AccentBlue, FontStyle.Bold));
            x += tipoW;
            row.Controls.Add(UiBuilder.Label(risco.Agente, x, 2, agenteW, 34, 8F, FontStyle.Regular, UiColors.BodyText));
            x += agenteW;
            row.Controls.Add(UiBuilder.Cell(risco.DataAvaliacao, x, 2, dataW, UiColors.AccentBlue, FontStyle.Bold));
            x += dataW;

            RoundButton abrir = UiBuilder.SmallButton("Abrir", x + 8, 7, 50, Color.White, UiColors.BodyText);
            abrir.BorderColor = UiColors.Border;
            abrir.Tag = risco.Id;
            abrir.Click += (sender, e) => AppNavigator.Show(new RiskFactorsForm((int)((Control)sender).Tag));
            row.Controls.Add(abrir);

            RoundButton imprimir = UiBuilder.SmallButton("Imprimir", x + 64, 7, 76, UiColors.Orange, Color.White);
            imprimir.Tag = risco.Id;
            imprimir.Click += ImprimirFatorRisco_Click;
            row.Controls.Add(imprimir);
        }

        private int CalcularAlturaTabela()
        {
            try
            {
                int registros = CadastrosRepository.GetFatoresRisco(_termoBusca).Count;
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
                MessageBox.Show("Selecione um ou mais fatores de risco para excluir.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir os fatores de risco selecionados?", "Fatores de Risco", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                CadastrosRepository.DeleteFatoresRisco(new List<int>(_selecionados));
                _selecionados.Clear();
                MontarConteudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível excluir no MySQL.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImprimirFatorRisco_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)((Control)sender).Tag;
                RiskFactorRecord risco = CadastrosRepository.GetFatorRisco(id);
                if (risco == null)
                {
                    MessageBox.Show("Fator de risco não encontrado.", "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string arquivo = OccupationalPdfExporter.ExportarFatorRisco(risco);
                System.Diagnostics.Process.Start(arquivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível gerar o PDF do fator de risco.\n\n" + ex.Message, "Fatores de Risco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
