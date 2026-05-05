using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class CatBasicForm
    {
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private ComboBox cmbEmpregado;
        private ComboBox cmbTipoCat;
        private ComboBox cmbLocalAcidente;
        private ComboBox cmbEmitente;
        private CueTextBox txtDataAcidente;
        private CueTextBox txtHoraAcidente;
        private CueTextBox txtDataObito;
        private CueTextBox txtEntradaTrabalho;
        private CueTextBox txtSaidaTrabalho;
        private CueTextBox txtDescricao;

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

            MontarConteudoCat();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoCat();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoCat()
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

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 560);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            BuildCatHeader(form, 0, larguraDisponivel);

            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "IDENTIFICAÇÃO DO ACIDENTE",
                    0,
                    126,
                    larguraDisponivel,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            MontarIdentificacaoAcidente(form, larguraDisponivel);
            MontarLocalHorarioTrabalho(form, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarIdentificacaoAcidente(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 14;

            int campoW = (largura - (margem * 2) - (gap * 3)) / 4;

            if (campoW < 160)
                campoW = 160;

            int x = margem;

            UiBuilder.AddField(
                form,
                "DATA DO ACIDENTE",
                txtDataAcidente = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, campoW),
                x,
                154,
                campoW,
                true
            );

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "HORA DO ACIDENTE",
                txtHoraAcidente = UiBuilder.TextBox("hh:mm", 0, 0, campoW),
                x,
                154,
                campoW,
                true
            );

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "DATA DO ÓBITO",
                txtDataObito = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, campoW),
                x,
                154,
                campoW,
                false
            );

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "TIPO DE CAT",
                cmbTipoCat = UiBuilder.Combo("Inicial", 0, 0, campoW),
                x,
                154,
                campoW,
                true
            );
            cmbTipoCat.Items.Add("Reabertura");
            cmbTipoCat.Items.Add("Comunicacao de obito");

            form.Controls.Add(
                UiBuilder.Label(
                    "⚠ Obrigatório",
                    margem,
                    206,
                    120,
                    18,
                    7.2F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            int metadeW = (largura - (margem * 2) - gap) / 2;

            int empregadoW = metadeW - 40;

            UiBuilder.AddField(
                form,
                "EMPREGADO",
                cmbEmpregado = CriarComboEmpregados(empregadoW),
                margem,
                232,
                empregadoW,
                true
            );

            RoundButton btnNovoEmpregado = UiBuilder.Button("+", margem + empregadoW + 8, 256, 32, 32, Color.White, UiColors.AccentBlue);
            btnNovoEmpregado.BorderColor = UiColors.Border;
            btnNovoEmpregado.Click += NovoEmpregado_Click;
            form.Controls.Add(btnNovoEmpregado);

            UiBuilder.AddField(
                form,
                "EMITENTE DA CAT",
                cmbEmitente = UiBuilder.Combo("Empregador", 0, 0, metadeW),
                margem + metadeW + gap,
                232,
                metadeW,
                true
            );
        }

        private void MontarLocalHorarioTrabalho(RoundPanel form, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    "LOCAL E HORÁRIO DE TRABALHO",
                    0,
                    292,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            int margem = 18;
            int gap = 14;

            int campoW = (largura - (margem * 2) - (gap * 2)) / 3;

            if (campoW < 200)
                campoW = 200;

            int x = margem;

            UiBuilder.AddField(
                form,
                "LOCAL DO ACIDENTE",
                cmbLocalAcidente = UiBuilder.Combo("No estabelecimento do empregador", 0, 0, campoW),
                x,
                320,
                campoW,
                true
            );
            cmbLocalAcidente.Items.Add("Em via publica");
            cmbLocalAcidente.Items.Add("Em area rural");
            cmbLocalAcidente.Items.Add("Em estabelecimento de terceiros");
            cmbLocalAcidente.Items.Add("Em trajeto");

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "ENTRADA NO TRABALHO",
                txtEntradaTrabalho = UiBuilder.TextBox("hh:mm", 0, 0, campoW),
                x,
                320,
                campoW,
                false
            );

            x += campoW + gap;

            UiBuilder.AddField(
                form,
                "SAÍDA DO TRABALHO",
                txtSaidaTrabalho = UiBuilder.TextBox("hh:mm", 0, 0, campoW),
                x,
                320,
                campoW,
                false
            );

            form.Controls.Add(
                UiBuilder.Label(
                    "DESCRIÇÃO DO ACIDENTE *",
                    margem,
                    374,
                    240,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            txtDescricao = UiBuilder.TextBox(
                "Descreva como o acidente ocorreu, as circunstâncias e o que estava sendo realizado...",
                margem,
                402,
                largura - (margem * 2)
            );

            txtDescricao.Multiline = true;
            txtDescricao.Height = 65;
            txtDescricao.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            form.Controls.Add(txtDescricao);

            int rodapeW = (largura - (margem * 2) - gap) / 2;
            UiBuilder.AddField(
                form,
                "DATA DO COMUNICADO",
                UiBuilder.TextBox(System.DateTime.Today.ToString("dd/MM/yyyy"), 0, 0, rodapeW),
                margem,
                478,
                rodapeW,
                true
            );

            CheckBox aposentado = new CheckBox
            {
                Text = "Funcionario aposentado",
                Location = new Point(margem + rodapeW + gap, 505),
                Size = new Size(220, 22),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UiColors.BodyText
            };
            form.Controls.Add(aposentado);

            CarregarCat();
            CarregarRascunho();
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

        private ComboBox CriarComboEmpregados(int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(0, 0),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            combo.Items.Add(new ComboItem(0, "-- Selecione o empregado --"));

            try
            {
                foreach (EmpregadoRecord empregado in CadastrosRepository.GetEmpregados())
                {
                    combo.Items.Add(new ComboItem(empregado.Id, empregado.Nome + " - " + empregado.Matricula));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void CarregarCat()
        {
            if (_catId <= 0 || cmbEmpregado == null)
                return;

            CatRecord cat = CadastrosRepository.GetCat(_catId);
            if (cat == null)
                return;

            SelectComboItem(cmbEmpregado, cat.EmpregadoId);
            txtDataAcidente.Text = cat.DataAcidente;
            txtHoraAcidente.Text = cat.HoraAcidente;
            cmbTipoCat.Text = string.IsNullOrWhiteSpace(cat.TipoCat) ? "Inicial" : cat.TipoCat;
            cmbLocalAcidente.Text = cat.LocalAcidente;
            txtDescricao.Text = cat.Descricao;
        }

        private void CarregarRascunho()
        {
            if (_catId > 0 || CatDraftState.Current == null || cmbEmpregado == null)
                return;

            CatDraft draft = CatDraftState.Current;
            txtDataAcidente.Text = draft.DataAcidente ?? string.Empty;
            txtHoraAcidente.Text = draft.HoraAcidente ?? string.Empty;
            txtDataObito.Text = draft.DataObito ?? string.Empty;
            cmbTipoCat.Text = string.IsNullOrWhiteSpace(draft.TipoCat) ? "Inicial" : draft.TipoCat;
            SelectComboItem(cmbEmpregado, draft.EmpregadoId);
            cmbEmitente.Text = string.IsNullOrWhiteSpace(draft.Emitente) ? "Empregador" : draft.Emitente;
            cmbLocalAcidente.Text = string.IsNullOrWhiteSpace(draft.LocalAcidente) ? "No estabelecimento do empregador" : draft.LocalAcidente;
            txtEntradaTrabalho.Text = draft.EntradaTrabalho ?? string.Empty;
            txtSaidaTrabalho.Text = draft.SaidaTrabalho ?? string.Empty;
            txtDescricao.Text = draft.Descricao ?? string.Empty;
        }

        private void SalvarRascunho()
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;

            CatDraftState.Current = new CatDraft
            {
                DataAcidente = txtDataAcidente.Text.Trim(),
                HoraAcidente = txtHoraAcidente.Text.Trim(),
                DataObito = txtDataObito.Text.Trim(),
                TipoCat = cmbTipoCat.Text.Trim(),
                EmpregadoId = empregado == null ? 0 : empregado.Id,
                Emitente = cmbEmitente.Text.Trim(),
                LocalAcidente = cmbLocalAcidente.Text.Trim(),
                EntradaTrabalho = txtEntradaTrabalho.Text.Trim(),
                SaidaTrabalho = txtSaidaTrabalho.Text.Trim(),
                Descricao = txtDescricao.Text.Trim()
            };
        }

        private bool ValidarObrigatorios(out string mensagem)
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;

            if (string.IsNullOrWhiteSpace(txtDataAcidente.Text) ||
                string.IsNullOrWhiteSpace(txtHoraAcidente.Text) ||
                string.IsNullOrWhiteSpace(cmbTipoCat.Text) ||
                empregado == null ||
                empregado.Id <= 0 ||
                string.IsNullOrWhiteSpace(cmbEmitente.Text) ||
                string.IsNullOrWhiteSpace(cmbLocalAcidente.Text) ||
                string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                mensagem = "Preencha todos os campos obrigatorios marcados com asterisco antes de avancar.";
                return false;
            }

            System.DateTime dataAcidente;
            if (!System.DateTime.TryParseExact(txtDataAcidente.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataAcidente))
            {
                mensagem = "Informe a data do acidente no formato dd/mm/yyyy.";
                return false;
            }

            System.TimeSpan horaAcidente;
            if (!System.TimeSpan.TryParseExact(txtHoraAcidente.Text.Trim(), @"hh\:mm", null, out horaAcidente))
            {
                mensagem = "Informe a hora do acidente no formato hh:mm.";
                return false;
            }

            mensagem = string.Empty;
            return true;
        }

        private void SalvarCat()
        {
            string mensagem;
            if (!ValidarObrigatorios(out mensagem))
                throw new System.InvalidOperationException(mensagem);

            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            if (empregado == null || empregado.Id <= 0)
                throw new System.InvalidOperationException("Selecione o empregado.");

            System.DateTime dataAcidente;
            if (!System.DateTime.TryParseExact(txtDataAcidente.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataAcidente))
                throw new System.InvalidOperationException("Informe a data do acidente no formato dd/mm/yyyy.");

            CatRecord catExistente = _catId > 0 ? CadastrosRepository.GetCat(_catId) : null;

            CadastrosRepository.SaveCat(new CatRecord
            {
                Id = _catId,
                EmpregadoId = empregado.Id,
                DataAcidente = txtDataAcidente.Text.Trim(),
                HoraAcidente = txtHoraAcidente.Text.Trim(),
                DataComunicacao = System.DateTime.Today.ToString("dd/MM/yyyy"),
                LocalAcidente = cmbLocalAcidente.Text.Trim(),
                Descricao = txtDescricao.Text.Trim(),
                TipoCat = cmbTipoCat.Text.Trim(),
                Situacao = catExistente == null ? "Aberta" : catExistente.Situacao,
                ResultadoAso = catExistente == null ? "Aguardando ASO" : catExistente.ResultadoAso
            });

            CatDraftState.Clear();
        }

        private void SelectComboItem(ComboBox combo, int id)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                ComboItem item = combo.Items[i] as ComboItem;
                if (item != null && item.Id == id)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }
    }
}
