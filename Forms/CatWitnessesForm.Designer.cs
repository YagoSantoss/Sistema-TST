using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatWitnessesForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarTestemunha;
        private RoundButton btnVoltar;
        private List<CatTestemunhaRecord> _testemunhas = new List<CatTestemunhaRecord>();
        private readonly List<WitnessInputs> _witnessInputs = new List<WitnessInputs>();

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "CAT — Acidente de Trabalho",
                "S-2210 · Comunicação de Acidente",
                DashboardMenu.Cat
            );

            ContentPanel.AutoScroll = true;
            CarregarTestemunhasIniciais();

            MontarConteudoTestemunhas();

            ContentPanel.Resize += (sender, e) =>
            {
                if (!_montandoConteudo)
                    _testemunhas = ColetarTestemunhas();

                MontarConteudoTestemunhas();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoTestemunhas()
        {
            if (_montandoConteudo)
                return;

            _montandoConteudo = true;

            ContentPanel.SuspendLayout();
            ContentPanel.Controls.Clear();
            _witnessInputs.Clear();

            int margem = 18;
            int larguraDisponivel = ContentPanel.ClientSize.Width - (margem * 2);

            if (larguraDisponivel < 790)
                larguraDisponivel = 790;

            int quantidade = _testemunhas.Count == 0 ? 1 : _testemunhas.Count;
            int alturaForm = 205 + (quantidade * 154) + 55;
            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, alturaForm);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            BuildCatHeader(form, 1, larguraDisponivel);

            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "TESTEMUNHAS DO ACIDENTE",
                    0,
                    126,
                    larguraDisponivel,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int y = 160;
            for (int i = 0; i < quantidade; i++)
            {
                CatTestemunhaRecord testemunha = i < _testemunhas.Count ? _testemunhas[i] : new CatTestemunhaRecord();
                MontarBlocoTestemunha(form, larguraDisponivel, i, y, testemunha);
                y += 154;
            }

            btnAdicionarTestemunha = UiBuilder.SmallButton(
                "+ Adicionar Testemunha",
                18,
                y + 4,
                160,
                Color.White,
                UiColors.BodyText
            );

            btnAdicionarTestemunha.BorderColor = UiColors.Border;
            btnAdicionarTestemunha.Click += BtnAdicionarTestemunha_Click;
            form.Controls.Add(btnAdicionarTestemunha);

            btnVoltar = UiBuilder.SmallButton(
                "Voltar",
                190,
                y + 4,
                75,
                Color.White,
                UiColors.BodyText
            );

            btnVoltar.BorderColor = UiColors.Border;
            btnVoltar.Click += BtnVoltar_Click;
            form.Controls.Add(btnVoltar);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarBlocoTestemunha(RoundPanel form, int largura, int indice, int y, CatTestemunhaRecord testemunhaAtual)
        {
            int margem = 18;
            int gap = 16;

            RoundPanel witness = new RoundPanel
            {
                Location = new Point(margem, y),
                Size = new Size(largura - (margem * 2), 142),
                Radius = 8,
                FillColor = Color.FromArgb(248, 251, 254),
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(witness);

            witness.Controls.Add(
                UiBuilder.Label(
                    "TESTEMUNHA " + (indice + 1),
                    12,
                    10,
                    180,
                    18,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int larguraInterna = witness.Width - 24;
            int nomeW = (int)(larguraInterna * 0.42);
            int cpfW = (int)(larguraInterna * 0.28);
            int telefoneW = larguraInterna - nomeW - cpfW - (gap * 2);

            if (nomeW < 240)
                nomeW = 240;

            if (cpfW < 200)
                cpfW = 200;

            if (telefoneW < 200)
                telefoneW = 200;

            int x = 12;

            CueTextBox txtNome = UiBuilder.TextBox("Nome da testemunha", 0, 0, nomeW);
            txtNome.Text = testemunhaAtual == null ? string.Empty : testemunhaAtual.Nome;
            UiBuilder.AddField(
                witness,
                "NOME COMPLETO",
                txtNome,
                x,
                32,
                nomeW,
                false
            );

            x += nomeW + gap;

            CueTextBox txtCpf = CriarCpfTextBox(cpfW);
            txtCpf.Text = testemunhaAtual == null ? string.Empty : testemunhaAtual.Cpf;
            UiBuilder.AddField(
                witness,
                "CPF",
                txtCpf,
                x,
                32,
                cpfW,
                false
            );

            x += cpfW + gap;

            CueTextBox txtTelefone = CriarTelefoneTextBox(telefoneW);
            txtTelefone.Text = testemunhaAtual == null ? string.Empty : testemunhaAtual.Telefone;
            UiBuilder.AddField(
                witness,
                "TELEFONE",
                txtTelefone,
                x,
                32,
                telefoneW,
                false
            );

            CueTextBox txtEndereco = UiBuilder.TextBox("Endereco da testemunha", 0, 0, larguraInterna);
            txtEndereco.Text = testemunhaAtual == null ? string.Empty : testemunhaAtual.Endereco;
            UiBuilder.AddField(
                witness,
                "ENDERECO",
                txtEndereco,
                12,
                90,
                larguraInterna,
                false
            );

            _witnessInputs.Add(new WitnessInputs
            {
                Nome = txtNome,
                Cpf = txtCpf,
                Telefone = txtTelefone,
                Endereco = txtEndereco
            });
        }

        private CueTextBox CriarCpfTextBox(int width)
        {
            CueTextBox textBox = UiBuilder.TextBox("000.000.000-00", 0, 0, width);
            InputFormatHelper.ApplyCpfMask(textBox);
            return textBox;
        }

        private CueTextBox CriarTelefoneTextBox(int width)
        {
            CueTextBox textBox = UiBuilder.TextBox("(00) 00000-0000", 0, 0, width);
            InputFormatHelper.ApplyPhoneMask(textBox);
            return textBox;
        }

        private void BuildCatHeader(Panel form, int activeTab, int largura)
        {
            form.Controls.Add(
                UiBuilder.Pill(
                    "S-2210",
                    18,
                    18,
                    58,
                    UiColors.Red,
                    Color.White
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Comunicação de Acidente de Trabalho (CAT)",
                    88,
                    14,
                    largura - 320,
                    20,
                    9.5F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "Registro de novo acidente ou doença ocupacional",
                    88,
                    31,
                    largura - 320,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnSalvar = UiBuilder.SmallButton(
                "💾 Salvar CAT",
                largura - 110,
                16,
                92,
                UiColors.AccentBlue,
                Color.White
            );

            btnSalvar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalvar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            form.Controls.Add(btnSalvar);

            btnCancelar = UiBuilder.SmallButton(
                "Cancelar",
                largura - 195,
                16,
                75,
                Color.White,
                UiColors.BodyText
            );

            btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            form.Controls.Add(btnCancelar);

            Panel line = new Panel
            {
                Location = new Point(0, 58),
                Size = new Size(largura, 1),
                BackColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(line);

            AddTab(form, "📋 DADOS CADASTRAIS", 18, activeTab == 0, TabDados_Click);
            AddTab(form, "👤 TESTEMUNHAS", 190, activeTab == 1, TabTestemunhas_Click);
            AddTab(form, "🩹 DADOS COMPLEMENTARES", 320, activeTab == 2, TabComplementares_Click);
        }

        private void AddTab(Panel form, string text, int x, bool active, System.EventHandler handler)
        {
            int larguraTab = text.Contains("COMPLEMENTARES") ? 245 : (text.Contains("TESTEMUNHAS") ? 120 : 165);
            Button tab = new Button
            {
                Text = text,
                Location = new Point(x, 72),
                Size = new Size(larguraTab, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = active ? UiColors.AccentBlue : UiColors.MutedText,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            tab.FlatAppearance.BorderSize = 0;
            tab.Click += handler;

            form.Controls.Add(tab);

            if (active)
            {
                form.Controls.Add(
                    new Panel
                    {
                        Location = new Point(x, 101),
                        Size = new Size(larguraTab, 2),
                        BackColor = UiColors.Orange
                    }
                );
            }
        }
    }
}
