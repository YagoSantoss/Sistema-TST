using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class EmployeesForm
    {
        private RoundButton btnExportar;
        private RoundButton btnImportar;
        private RoundButton btnAtualizar;
        private RoundButton btnBuscarEmpregados;
        private RoundButton btnNovoFuncionario;
        private RoundButton btnInserir;
        private RoundButton btnEditar;
        private RoundButton btnExcluir;
        private CueTextBox txtBuscaEmpregados;
        private CheckBox chkSelecionarTodos;
        private Panel pnlListaEmpregados;
        private string _termoBuscaEmpregados = string.Empty;
        private readonly HashSet<int> _empregadosSelecionados = new HashSet<int>();

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell("Empregados", "Listagem geral de empregados", DashboardMenu.Employees);
            ContentPanel.AutoScroll = true;

            MontarConteudoEmpregados();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoEmpregados();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoEmpregados()
        {
            if (_montandoConteudo)
                return;

            _montandoConteudo = true;

            ContentPanel.SuspendLayout();
            ContentPanel.Controls.Clear();

            int margem = 18;

            int larguraDisponivel = ContentPanel.ClientSize.Width - (margem * 2);

            if (larguraDisponivel < 880)
                larguraDisponivel = 880;

            int alturaTabela = CalcularAlturaTabela();

            RoundPanel table = UiBuilder.Card(margem, 18, larguraDisponivel, alturaTabela);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            MontarFiltros(table, larguraDisponivel);
            MontarCabecalhoTabela(table, larguraDisponivel);
            MontarLinhasTabela(table, larguraDisponivel, alturaTabela);
            MontarRodapeTabela(table, larguraDisponivel, alturaTabela);

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
                UiBuilder.Label(titulo, 18, 18, width - 36, 18, 8F, FontStyle.Bold, UiColors.MutedText)
            );

            card.Controls.Add(
                UiBuilder.Label(valor, 18, 37, width - 36, 30, 18F, FontStyle.Bold, corPrincipal)
            );

            PillLabel pill = new PillLabel
            {
                Text = detalhe,
                Location = new Point(18, 62),
                Size = new Size(110, 18),
                FillColor = corFundoDetalhe,
                ForeColor = corPrincipal,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                Radius = 9
            };

            card.Controls.Add(pill);
        }

        private void MontarFiltros(RoundPanel table, int larguraTabela)
        {
            int margem = 15;
            int gap = 10;

            int exportarW = 86;
            int importarW = 92;
            int atualizarW = 82;
            int novoW = 126;
            int buscarW = 82;

            int searchW = larguraTabela - margem - buscarW - atualizarW - importarW - exportarW - novoW - (gap * 6) - margem;

            if (searchW < 260)
                searchW = 260;

            txtBuscaEmpregados = UiBuilder.TextBox(
                "Buscar por nome, CPF ou matricula...",
                margem,
                14,
                searchW
            );
            txtBuscaEmpregados.Text = _termoBuscaEmpregados;
            txtBuscaEmpregados.KeyDown += TxtBuscaEmpregados_KeyDown;
            table.Controls.Add(txtBuscaEmpregados);

            int x = margem + searchW + gap;

            btnBuscarEmpregados = UiBuilder.SmallButton("Buscar", x, 14, buscarW, UiColors.Orange, Color.White);
            btnBuscarEmpregados.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnBuscarEmpregados.Click += BtnBuscarEmpregados_Click;
            table.Controls.Add(btnBuscarEmpregados);

            x += buscarW + gap;

            btnAtualizar = UiBuilder.SmallButton("Atualizar", x, 14, atualizarW, Color.White, UiColors.BodyText);
            btnAtualizar.BorderColor = UiColors.Border;
            btnAtualizar.Click += BtnAtualizar_Click;
            table.Controls.Add(btnAtualizar);

            x += atualizarW + gap;

            btnImportar = UiBuilder.SmallButton("Importar CSV", x, 14, importarW, Color.White, UiColors.BodyText);
            btnImportar.BorderColor = UiColors.Border;
            btnImportar.Click += BtnImportar_Click;
            table.Controls.Add(btnImportar);

            x += importarW + gap;

            btnExportar = UiBuilder.SmallButton("Exportar", x, 14, exportarW, UiColors.Orange, Color.White);
            btnNovoFuncionario = UiBuilder.SmallButton("+ Novo Empregado", x, 14, novoW, UiColors.AccentBlue, Color.White);
            btnNovoFuncionario.Click += BtnInserir_Click;
            table.Controls.Add(btnNovoFuncionario);

            x += novoW + gap;

            btnExportar.Location = new Point(x, 14);
            btnExportar.Click += BtnExportar_Click;
            table.Controls.Add(btnExportar);
        }

        private void MontarCabecalhoTabela(RoundPanel table, int larguraTabela)
        {
            Panel header = new Panel
            {
                Location = new Point(0, 58),
                Size = new Size(larguraTabela, 32),
                BackColor = UiColors.HeaderBlue,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(header);

            int checkW = 34;
            int matriculaW = 90;
            int cpfW = 130;
            int admissaoW = 90;
            int vencimentoW = 90;
            int statusW = 90;
            int asoW = 80;

            int larguraRestante = larguraTabela - checkW - matriculaW - cpfW - admissaoW - vencimentoW - statusW - asoW;

            int nomeW = larguraRestante / 2;
            int setorW = larguraRestante - nomeW;

            int x = 5;

            chkSelecionarTodos = new CheckBox
            {
                Location = new Point(x + 9, 8),
                Size = new Size(16, 16),
                BackColor = UiColors.HeaderBlue,
                Cursor = Cursors.Hand,
                Checked = TodosEmpregadosFiltradosSelecionados()
            };
            chkSelecionarTodos.CheckedChanged += SelecionarTodos_CheckedChanged;
            header.Controls.Add(chkSelecionarTodos);
            x += checkW;

            header.Controls.Add(UiBuilder.HeaderCell("MATRICULA", x, 2, matriculaW));
            x += matriculaW;

            header.Controls.Add(UiBuilder.HeaderCell("NOME COMPLETO", x, 2, nomeW));
            x += nomeW;

            header.Controls.Add(UiBuilder.HeaderCell("CPF", x, 2, cpfW));
            x += cpfW;

            header.Controls.Add(UiBuilder.HeaderCell("SETOR / CARGO", x, 2, setorW));
            x += setorW;

            header.Controls.Add(UiBuilder.HeaderCell("ADMISSAO", x, 2, admissaoW));
            x += admissaoW;

            header.Controls.Add(UiBuilder.HeaderCell("VENC. ASO", x, 2, vencimentoW));
            x += vencimentoW;

            header.Controls.Add(UiBuilder.HeaderCell("STATUS", x, 2, statusW));
            x += statusW;

            header.Controls.Add(UiBuilder.HeaderCell("ASO", x, 2, asoW));
        }

        private void MontarLinhasTabela(RoundPanel table, int larguraTabela, int alturaTabela)
        {
            int alturaLista = alturaTabela - 142;
            if (alturaLista < 120)
                alturaLista = 120;

            pnlListaEmpregados = new Panel
            {
                Location = new Point(0, 90),
                Size = new Size(larguraTabela, alturaLista),
                AutoScroll = true,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            table.Controls.Add(pnlListaEmpregados);

            try
            {
                List<EmpregadoRecord> empregados = ObterEmpregadosFiltrados();

                if (empregados.Count == 0)
                {
                    pnlListaEmpregados.Controls.Add(UiBuilder.CenterLabel("Nenhum empregado cadastrado", 0, 36, larguraTabela, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int larguraLinha = larguraTabela - SystemInformation.VerticalScrollBarWidth;
                int y = 0;
                foreach (EmpregadoRecord empregado in empregados)
                {
                    Color statusColor = StatusColor(empregado.StatusAso);
                    AddEmployeeRow(
                        pnlListaEmpregados,
                        larguraLinha,
                        y,
                        empregado.Id,
                        _empregadosSelecionados.Contains(empregado.Id),
                        empregado.Matricula,
                        empregado.Nome,
                        empregado.Cpf,
                        empregado.Setor + " / " + empregado.Cargo,
                        empregado.DataAdmissao,
                        empregado.DataVencimentoAso,
                        "â— " + empregado.StatusAso,
                        statusColor,
                        StatusBack(statusColor),
                        "Ver ASO");
                    y += 32;
                }
            }
            catch
            {
                pnlListaEmpregados.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar empregados do MySQL", 0, 36, larguraTabela, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void MontarRodapeTabela(RoundPanel table, int larguraTabela, int alturaTabela)
        {
            int y = alturaTabela - 40;

            Panel divisor = new Panel
            {
                Location = new Point(0, y - 8),
                Size = new Size(larguraTabela, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            table.Controls.Add(divisor);

            btnInserir = UiBuilder.SmallButton("+ Inserir", 15, y, 70, UiColors.AccentBlue, Color.White);
            btnInserir.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnInserir.Click += BtnInserir_Click;
            table.Controls.Add(btnInserir);

            btnEditar = UiBuilder.SmallButton("Editar", 92, y, 70, Color.White, UiColors.BodyText);
            btnEditar.BorderColor = UiColors.Border;
            btnEditar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnEditar.Click += BtnEditar_Click;
            table.Controls.Add(btnEditar);

            btnExcluir = UiBuilder.SmallButton("Excluir", 169, y, 78, Color.White, UiColors.BodyText);
            btnExcluir.BorderColor = UiColors.Border;
            btnExcluir.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnExcluir.Click += BtnExcluir_Click;
            table.Controls.Add(btnExcluir);

            Label info = UiBuilder.Label(
                TotalEmpregadosTexto(),
                larguraTabela - 360,
                y + 1,
                270,
                22,
                7.5F,
                FontStyle.Regular,
                UiColors.MutedText
            );
            info.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            table.Controls.Add(info);

            RoundButton voltar = UiBuilder.SmallButton("â€¹", larguraTabela - 75, y, 26, Color.White, UiColors.AccentBlue);
            voltar.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            table.Controls.Add(voltar);

            RoundButton proximo = UiBuilder.SmallButton("â€º", larguraTabela - 42, y, 26, Color.White, UiColors.AccentBlue);
            proximo.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            table.Controls.Add(proximo);
        }

        private int CalcularAlturaTabela()
        {
            int altura = ContentPanel.ClientSize.Height - 36;
            if (altura < 460)
                altura = 460;

            return altura;
        }

        private void AddEmployeeRow(
            Panel table,
            int larguraTabela,
            int y,
            int empregadoId,
            bool selected,
            string matricula,
            string nome,
            string cpf,
            string setor,
            string admissao,
            string vencimento,
            string status,
            Color statusColor,
            Color statusBack,
            string action)
        {
            Color back = selected ? Color.FromArgb(255, 244, 229) : Color.White;

            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(larguraTabela, 32),
                BackColor = back,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(row);

            int checkW = 34;
            int matriculaW = 90;
            int cpfW = 130;
            int admissaoW = 90;
            int vencimentoW = 90;
            int statusW = 90;
            int asoW = 80;

            int larguraRestante = larguraTabela - checkW - matriculaW - cpfW - admissaoW - vencimentoW - statusW - asoW;

            int nomeW = larguraRestante / 2;
            int setorW = larguraRestante - nomeW;

            int x = 5;

            CheckBox check = new CheckBox
            {
                Location = new Point(x + 9, 8),
                Size = new Size(16, 16),
                Checked = selected,
                Tag = empregadoId,
                Cursor = Cursors.Hand
            };
            check.CheckedChanged += EmployeeCheck_CheckedChanged;
            row.Controls.Add(check);
            x += checkW;

            row.Controls.Add(UiBuilder.Cell(matricula, x, 0, matriculaW, UiColors.AccentBlue, FontStyle.Bold));
            x += matriculaW;

            row.Controls.Add(UiBuilder.Label(nome, x + 8, 0, nomeW - 8, 32, 8F, FontStyle.Bold, UiColors.BodyText));
            x += nomeW;

            row.Controls.Add(UiBuilder.Cell(cpf, x, 0, cpfW, UiColors.AccentBlue, FontStyle.Bold));
            x += cpfW;

            row.Controls.Add(UiBuilder.Label(setor, x, 0, setorW, 32, 7.5F, FontStyle.Regular, UiColors.BodyText));
            x += setorW;

            row.Controls.Add(UiBuilder.Cell(admissao, x, 0, admissaoW, UiColors.AccentBlue, FontStyle.Bold));
            x += admissaoW;

            row.Controls.Add(UiBuilder.Cell(vencimento, x, 0, vencimentoW, statusColor, FontStyle.Bold));
            x += vencimentoW;

            row.Controls.Add(UiBuilder.Pill(status, x, 6, statusW - 10, statusBack, statusColor));
            x += statusW;

            RoundButton button = UiBuilder.SmallButton(
                action,
                x + 4,
                5,
                asoW - 10,
                action == "Registrar" ? UiColors.AccentBlue : Color.White,
                action == "Registrar" ? Color.White : UiColors.BodyText
            );

            button.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            button.BorderColor = action == "Registrar" ? Color.Transparent : UiColors.Border;
            button.Tag = empregadoId;

            if (action == "Registrar")
            {
                button.Click += BtnRegistrarAso_Click;
            }
            else if (action == "Ver ASO")
            {
                button.Click += BtnRegistrarAso_Click;
            }
            else if (action == "Agendar")
            {
                button.Click += BtnAgendar_Click;
            }

            row.Controls.Add(button);
        }

        private string TotalEmpregadosTexto()
        {
            try
            {
                int total = CadastrosRepository.GetEmpregados().Count;
                int exibidos = ObterEmpregadosFiltrados().Count;
                return "Exibindo " + exibidos + " de " + total + " registros";
            }
            catch
            {
                return "MySQL indisponivel";
            }
        }

        private List<EmpregadoRecord> ObterEmpregadosFiltrados()
        {
            List<EmpregadoRecord> todos = CadastrosRepository.GetEmpregados();
            string termo = (_termoBuscaEmpregados ?? string.Empty).Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            List<EmpregadoRecord> filtrados = new List<EmpregadoRecord>();
            foreach (EmpregadoRecord empregado in todos)
            {
                if (Contem(empregado.Nome, termo)
                    || Contem(empregado.Cpf, termo)
                    || Contem(empregado.Matricula, termo))
                {
                    filtrados.Add(empregado);
                }
            }

            return filtrados;
        }

        private bool Contem(string valor, string termo)
        {
            return (valor ?? string.Empty).ToLowerInvariant().Contains(termo);
        }

        private Color StatusColor(string status)
        {
            if (status == "Vencido")
                return UiColors.Red;

            if (status == "A vencer")
                return UiColors.Orange;

            return UiColors.Green;
        }

        private Color StatusBack(Color color)
        {
            if (color == UiColors.Red)
                return Color.FromArgb(255, 233, 232);

            if (color == UiColors.Orange)
                return Color.FromArgb(255, 246, 206);

            return Color.FromArgb(217, 248, 234);
        }
    }
}
