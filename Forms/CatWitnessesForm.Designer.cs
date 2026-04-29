using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatWitnessesForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarTestemunha;

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

            MontarConteudoTestemunhas();

            ContentPanel.Resize += (sender, e) =>
            {
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

            int margem = 18;
            int larguraDisponivel = ContentPanel.ClientSize.Width - (margem * 2);

            if (larguraDisponivel < 790)
                larguraDisponivel = 790;

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 290);
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

            MontarBlocoTestemunha(form, larguraDisponivel);

            btnAdicionarTestemunha = UiBuilder.SmallButton(
                "+ Adicionar Testemunha",
                18,
                252,
                160,
                Color.White,
                UiColors.BodyText
            );

            btnAdicionarTestemunha.BorderColor = UiColors.Border;
            btnAdicionarTestemunha.Click += BtnAdicionarTestemunha_Click;
            form.Controls.Add(btnAdicionarTestemunha);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarBlocoTestemunha(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 16;

            RoundPanel witness = new RoundPanel
            {
                Location = new Point(margem, 160),
                Size = new Size(largura - (margem * 2), 82),
                Radius = 8,
                FillColor = Color.FromArgb(248, 251, 254),
                BorderColor = UiColors.Border,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            form.Controls.Add(witness);

            witness.Controls.Add(
                UiBuilder.Label(
                    "TESTEMUNHA 1",
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

            UiBuilder.AddField(
                witness,
                "NOME COMPLETO",
                UiBuilder.TextBox("Nome da testemunha", 0, 0, nomeW),
                x,
                32,
                nomeW,
                false
            );

            x += nomeW + gap;

            UiBuilder.AddField(
                witness,
                "CPF",
                UiBuilder.TextBox("000.000.000-00", 0, 0, cpfW),
                x,
                32,
                cpfW,
                false
            );

            x += cpfW + gap;

            UiBuilder.AddField(
                witness,
                "TELEFONE",
                UiBuilder.TextBox("(11) 99000-0000", 0, 0, telefoneW),
                x,
                32,
                telefoneW,
                false
            );
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
            Button tab = new Button
            {
                Text = text,
                Location = new Point(x, 72),
                Size = new Size(165, 28),
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
                        Size = new Size(165, 2),
                        BackColor = UiColors.Orange
                    }
                );
            }
        }
    }
}