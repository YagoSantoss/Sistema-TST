using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatMedicalForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnAdicionarMedico;

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

            MontarConteudoDadosMedicos();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoDadosMedicos();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoDadosMedicos()
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

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 400);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            BuildCatHeader(form, 2, larguraDisponivel);

            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "DADOS MÉDICOS COMPLEMENTARES",
                    0,
                    126,
                    larguraDisponivel,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            MontarCamposMedicos(form, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCamposMedicos(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 18;

            int metadeW = (largura - (margem * 2) - gap) / 2;

            if (metadeW < 340)
                metadeW = 340;

            int xEsq = margem;
            int xDir = margem + metadeW + gap;

            UiBuilder.AddField(
                form,
                "PARTE DO CORPO ATINGIDA",
                UiBuilder.Combo("— Selecione —", 0, 0, metadeW),
                xEsq,
                154,
                metadeW,
                true
            );

            UiBuilder.AddField(
                form,
                "AGENTE CAUSADOR",
                UiBuilder.Combo("— Selecione —", 0, 0, metadeW),
                xDir,
                154,
                metadeW,
                true
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "⚠ Obrigatório",
                    xEsq,
                    206,
                    120,
                    18,
                    7.2F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "⚠ Obrigatório",
                    xDir,
                    206,
                    120,
                    18,
                    7.2F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            UiBuilder.AddField(
                form,
                "CID-10",
                UiBuilder.TextBox("Ex: S60.0 — Contusão de dedo...", 0, 0, metadeW),
                xEsq,
                232,
                metadeW,
                true
            );

            UiBuilder.AddField(
                form,
                "NATUREZA DA LESÃO",
                UiBuilder.Combo("— Selecione —", 0, 0, metadeW),
                xDir,
                232,
                metadeW,
                true
            );

            int botaoW = 28;
            int medicoW = metadeW - botaoW - 8;

            UiBuilder.AddField(
                form,
                "MÉDICO ASSISTENTE",
                UiBuilder.Combo("— Selecione —", 0, 0, medicoW),
                xEsq,
                292,
                medicoW,
                false
            );

            btnAdicionarMedico = UiBuilder.Button(
                "+",
                xEsq + medicoW + 6,
                316,
                botaoW,
                30,
                Color.White,
                UiColors.AccentBlue
            );

            btnAdicionarMedico.BorderColor = UiColors.Border;
            btnAdicionarMedico.Click += BtnAdicionarMedico_Click;
            form.Controls.Add(btnAdicionarMedico);

            UiBuilder.AddField(
                form,
                "CÓDIGO SNT (SINAN)",
                UiBuilder.TextBox("Código do agravo (se aplicável)", 0, 0, metadeW),
                xDir,
                292,
                metadeW,
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