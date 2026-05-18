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
                "Exames Realizados",
                "Lançamento de exames complementares por empregado e médico",
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

            if (larguraDisponivel < 980)
                larguraDisponivel = 980;

            int alturaTabela = CalcularAlturaTabela();
            RoundPanel table = UiBuilder.Card(margem, 18, larguraDisponivel, alturaTabela);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            MontarCabecalho(table, larguraDisponivel);
            MontarCabecalhoTabela(table, larguraDisponivel);
            MontarLinhas(table, larguraDisponivel);
            MontarRodape(table, larguraDisponivel, alturaTabela);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel table, int largura)
        {
            table.Controls.Add(
                UiBuilder.Label(
                    "Exames realizados",
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
                    "Registre exames feitos por empregado, médico responsável e anexo quando houver",
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
                "+ Lancar Exame",
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

            int codigoW = 80;
            int checkW = 34;
            int tipoW = 110;
            int periodicidadeW = 105;
            int pacienteW = 150;
            int medicoW = 150;
            int anexoW = 70;
            int acoesW = 90;
            int nomeW = largura - checkW - codigoW - tipoW - periodicidadeW - pacienteW - medicoW - anexoW - acoesW - 24;

            if (nomeW < 190)
                nomeW = 190;

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

            header.Controls.Add(UiBuilder.HeaderCell("PACIENTE", x, 0, pacienteW));
            x += pacienteW;

            header.Controls.Add(UiBuilder.HeaderCell("MÉDICO", x, 0, medicoW));
            x += medicoW;

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
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum exame realizado cadastrado", 0, 126, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 83;
                foreach (TipoExameRecord exame in exames)
                {
                    AddExamRow(table, largura, y, exame.Id, exame.Codigo, exame.Nome, exame.Tipo, exame.Periodicidade, exame.PacienteNome, exame.MedicoNome, exame.AnexoNome);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Não foi possível carregar os exames do MySQL", 0, 126, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private int CalcularAlturaTabela()
        {
            try
            {
                int registros = CadastrosRepository.GetTiposExames().Count;
                int altura = 83 + (registros * 38) + 70;
                return altura < 275 ? 275 : altura;
            }
            catch
            {
                return 275;
            }
        }

        private void MontarRodape(RoundPanel table, int largura, int alturaTabela)
        {
            btnExcluir = UiBuilder.SmallButton("Excluir", 16, alturaTabela - 40, 78, Color.White, UiColors.Red);
            btnExcluir.BorderColor = UiColors.Border;
            btnExcluir.Click += BtnExcluir_Click;
            table.Controls.Add(btnExcluir);

            Label total = UiBuilder.Label(
                TotalExamesTexto(),
                largura - 170,
                alturaTabela - 38,
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
            string paciente,
            string medico,
            string anexoNome)
        {
            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(largura, 38),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(row);

            int codigoW = 80;
            int checkW = 34;
            int tipoW = 110;
            int periodicidadeW = 105;
            int pacienteW = 150;
            int medicoW = 150;
            int anexoW = 70;
            int acoesW = 90;
            int nomeW = largura - checkW - codigoW - tipoW - periodicidadeW - pacienteW - medicoW - anexoW - acoesW - 24;

            if (nomeW < 190)
                nomeW = 190;

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

            row.Controls.Add(
                UiBuilder.Label(
                    string.IsNullOrWhiteSpace(paciente) ? "-" : paciente,
                    x,
                    2,
                    pacienteW,
                    34,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += pacienteW;

            row.Controls.Add(
                UiBuilder.Label(
                    string.IsNullOrWhiteSpace(medico) ? "-" : medico,
                    x,
                    2,
                    medicoW,
                    34,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += medicoW;

            if (!string.IsNullOrWhiteSpace(anexoNome))
            {
                RoundButton verAnexo = UiBuilder.SmallButton(
                    "PDF",
                    x + 8,
                    8,
                    48,
                    Color.White,
                    UiColors.AccentBlue
                );

                verAnexo.BorderColor = UiColors.Border;
                verAnexo.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
                verAnexo.Tag = id;
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
            int id = control != null && control.Tag is int ? (int)control.Tag : 0;

            TipoExameRecord exame = CadastrosRepository.GetTipoExameAnexo(id);
            if (exame == null || exame.AnexoArquivo == null || exame.AnexoArquivo.Length == 0)
            {
                MessageBox.Show("O PDF anexado não foi encontrado no banco de dados.", "Anexo do exame", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nome = string.IsNullOrWhiteSpace(exame.AnexoNome) ? "exame_" + id + ".pdf" : exame.AnexoNome;
            string pasta = Path.Combine(Path.GetTempPath(), "SistemaTST", "AnexosExames");
            Directory.CreateDirectory(pasta);
            string caminho = Path.Combine(pasta, nome);
            File.WriteAllBytes(caminho, exame.AnexoArquivo);
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
                return "MySQL indisponível";
            }
        }
    }
}
