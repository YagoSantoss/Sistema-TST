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
        private ComboBox cmbArea;
        private ComboBox cmbFiliacaoPrevSocial;
        private ComboBox cmbTipoAcidente;
        private ComboBox cmbCatEmitidaPor;
        private ComboBox cmbTipoLogradouro;
        private ComboBox cmbTipoInscricao;
        private CueTextBox txtDataAcidente;
        private CueTextBox txtHoraAcidente;
        private CueTextBox txtDataComunicacao;
        private CueTextBox txtDataObito;
        private CueTextBox txtHorasTrabalhadasAntes;
        private CueTextBox txtUltimoDiaTrabalho;
        private CueTextBox txtCodificacaoAcidente;
        private CueTextBox txtSituacaoGeradora;
        private CueTextBox txtEspecificacaoLocal;
        private CueTextBox txtNumero;
        private CueTextBox txtInscricaoEstabelecimento;
        private CueTextBox txtLogradouro;
        private CueTextBox txtMunicipio;
        private CueTextBox txtUf;
        private CueTextBox txtBairro;
        private CueTextBox txtComplemento;
        private CueTextBox txtCep;
        private CueTextBox txtCodigoPostal;
        private CueTextBox txtDescricao;
        private CheckBox chkAposentado;
        private CheckBox chkHouveObito;
        private CheckBox chkHouveAfastamento;
        private CheckBox chkRegistroPolicia;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "CAT - Acidente de Trabalho",
                "S-2210 - Comunicacao de Acidente",
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

            if (larguraDisponivel < 1000)
                larguraDisponivel = 1000;

            RoundPanel form = UiBuilder.Card(margem, 18, larguraDisponivel, 845);
            form.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(form);

            BuildCatHeader(form, 0, larguraDisponivel);

            AddSectionTitle(form, "DADOS CADASTRAIS", 126, larguraDisponivel);
            MontarDadosCadastrais(form, larguraDisponivel);

            AddSectionTitle(form, "COMUNICACAO DE ACIDENTE DE TRABALHO", 250, larguraDisponivel);
            MontarComunicacaoAcidente(form, larguraDisponivel);

            AddSectionTitle(form, "LOCAL DO ACIDENTE", 472, larguraDisponivel);
            MontarLocalAcidente(form, larguraDisponivel);

            MontarObservacao(form, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void AddSectionTitle(Panel form, string text, int y, int largura)
        {
            form.Controls.Add(
                UiBuilder.CenterLabel(
                    text,
                    0,
                    y,
                    largura,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );
        }

        private void MontarDadosCadastrais(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 12;
            int y = 154;
            int col = (largura - (margem * 2) - (gap * 4)) / 5;
            if (col < 170)
                col = 170;

            int empregadoW = (col * 2) + gap - 40;

            UiBuilder.AddField(
                form,
                "EMPREGADO",
                cmbEmpregado = CriarComboEmpregados(empregadoW),
                margem,
                y,
                empregadoW,
                true
            );

            RoundButton btnNovoEmpregado = UiBuilder.Button("+", margem + empregadoW + 8, y + 24, 32, 32, Color.White, UiColors.AccentBlue);
            btnNovoEmpregado.BorderColor = UiColors.Border;
            btnNovoEmpregado.Click += NovoEmpregado_Click;
            form.Controls.Add(btnNovoEmpregado);

            int x = margem + (col * 2) + (gap * 2);
            UiBuilder.AddField(
                form,
                "DATA DO COMUNICADO",
                txtDataComunicacao = UiBuilder.TextBox(System.DateTime.Today.ToString("dd/MM/yyyy"), 0, 0, col),
                x,
                y,
                col,
                true
            );
            InputFormatHelper.ApplyDateMask(txtDataComunicacao);

            chkAposentado = CriarCheck("Aposentado", x + col + gap, y + 30, 110);
            form.Controls.Add(chkAposentado);

            UiBuilder.AddField(
                form,
                "AREA",
                cmbArea = UiBuilder.Combo("Urbana", 0, 0, col),
                x + col + gap + 120,
                y,
                col,
                false
            );
            cmbArea.Items.Add("Rural");

            y = 207;
            int metade = (largura - (margem * 2) - gap) / 2;
            UiBuilder.AddField(
                form,
                "FILIACAO A PREV. SOCIAL",
                cmbFiliacaoPrevSocial = UiBuilder.Combo("Empregado", 0, 0, metade),
                margem,
                y,
                metade,
                false
            );
            cmbFiliacaoPrevSocial.Items.Add("Trabalhador avulso");
            cmbFiliacaoPrevSocial.Items.Add("Segurado especial");
            cmbFiliacaoPrevSocial.Items.Add("Medico residente");

            UiBuilder.AddField(
                form,
                "EMITENTE DA CAT",
                cmbEmitente = UiBuilder.Combo("Empregador", 0, 0, metade),
                margem + metade + gap,
                y,
                metade,
                true
            );
            cmbEmitente.Items.Add("Sindicato");
            cmbEmitente.Items.Add("Medico");
            cmbEmitente.Items.Add("Segurado ou dependente");
            cmbEmitente.Items.Add("Autoridade publica");
        }

        private void MontarComunicacaoAcidente(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 12;
            int col = (largura - (margem * 2) - (gap * 4)) / 5;
            if (col < 170)
                col = 170;

            int y = 278;
            int x = margem;

            UiBuilder.AddField(form, "DATA DO ACIDENTE", txtDataAcidente = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, col), x, y, col, true);
            InputFormatHelper.ApplyDateMask(txtDataAcidente);
            x += col + gap;
            UiBuilder.AddField(form, "TIPO DO ACIDENTE", cmbTipoAcidente = UiBuilder.Combo("Acidente tipico", 0, 0, col), x, y, col, true);
            cmbTipoAcidente.Items.Add("Doenca");
            cmbTipoAcidente.Items.Add("Trajeto");
            x += col + gap;
            UiBuilder.AddField(form, "HORA DO ACIDENTE", txtHoraAcidente = UiBuilder.TextBox("hh:mm", 0, 0, col), x, y, col, true);
            InputFormatHelper.ApplyTimeMask(txtHoraAcidente);
            x += col + gap;
            UiBuilder.AddField(form, "HORAS TRAB. ANTES", txtHorasTrabalhadasAntes = UiBuilder.TextBox("ex: 04:30", 0, 0, col), x, y, col, false);
            InputFormatHelper.ApplyTimeMask(txtHorasTrabalhadasAntes);
            x += col + gap;
            UiBuilder.AddField(form, "TIPO DA CAT", cmbTipoCat = UiBuilder.Combo("Inicial", 0, 0, col), x, y, col, true);
            cmbTipoCat.Items.Add("Reabertura");
            cmbTipoCat.Items.Add("Comunicacao de obito");

            y = 333;
            chkHouveObito = CriarCheck("Houve obito", margem, y + 28, 115);
            form.Controls.Add(chkHouveObito);

            UiBuilder.AddField(form, "DATA DO OBITO", txtDataObito = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, col), margem + 130, y, col, false);
            InputFormatHelper.ApplyDateMask(txtDataObito);

            chkHouveAfastamento = CriarCheck("Houve afastamento", margem + 130 + col + gap, y + 28, 150);
            form.Controls.Add(chkHouveAfastamento);

            chkRegistroPolicia = CriarCheck("Registro policia", margem + 130 + col + gap + 160, y + 28, 140);
            form.Controls.Add(chkRegistroPolicia);

            UiBuilder.AddField(form, "ULTIMO DIA TRABALHO", txtUltimoDiaTrabalho = UiBuilder.TextBox("dd/mm/yyyy", 0, 0, col), largura - margem - (col * 2) - gap, y, col, false);
            InputFormatHelper.ApplyDateMask(txtUltimoDiaTrabalho);
            UiBuilder.AddField(form, "CODIFICACAO ACIDENTE", txtCodificacaoAcidente = UiBuilder.TextBox("Codigo", 0, 0, col), largura - margem - col, y, col, false);

            y = 389;
            int amplo = ((largura - (margem * 2) - gap) / 2);
            UiBuilder.AddField(form, "SITUACAO GERADORA DO ACIDENTE OU DOENCA", txtSituacaoGeradora = UiBuilder.TextBox("Descreva ou informe o codigo", 0, 0, amplo), margem, y, amplo, false);
            UiBuilder.AddField(form, "CAT EMITIDA POR", cmbCatEmitidaPor = UiBuilder.Combo("1 - Iniciativa do empregador", 0, 0, amplo), margem + amplo + gap, y, amplo, false);
            cmbCatEmitidaPor.Items.Add("2 - Ordem judicial");
            cmbCatEmitidaPor.Items.Add("3 - Determinacao de orgao fiscalizador");
        }

        private void MontarLocalAcidente(RoundPanel form, int largura)
        {
            int margem = 18;
            int gap = 12;
            int col = (largura - (margem * 2) - (gap * 3)) / 4;
            if (col < 200)
                col = 200;

            int y = 500;
            int x = margem;

            UiBuilder.AddField(form, "TIPO DO LOCAL DO ACIDENTE", cmbLocalAcidente = UiBuilder.Combo("Estabelecimento do empregador no Brasil", 0, 0, col), x, y, col, true);
            cmbLocalAcidente.Items.Add("Estabelecimento de terceiros");
            cmbLocalAcidente.Items.Add("Via publica");
            cmbLocalAcidente.Items.Add("Area rural");
            cmbLocalAcidente.Items.Add("Trajeto");
            x += col + gap;
            UiBuilder.AddField(form, "ESPECIFICACAO DO LOCAL", txtEspecificacaoLocal = UiBuilder.TextBox("Ex: setor, sala, maquina ou referencia", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "TIPO DE LOGRADOURO", cmbTipoLogradouro = UiBuilder.Combo("Avenida", 0, 0, col), x, y, col, false);
            cmbTipoLogradouro.Items.Add("Rua");
            cmbTipoLogradouro.Items.Add("Rodovia");
            cmbTipoLogradouro.Items.Add("Estrada");
            cmbTipoLogradouro.Items.Add("Travessa");
            x += col + gap;
            UiBuilder.AddField(form, "NUMERO", txtNumero = UiBuilder.TextBox("Numero", 0, 0, col), x, y, col, false);

            y = 556;
            x = margem;
            UiBuilder.AddField(form, "TIPO INSCRICAO", cmbTipoInscricao = UiBuilder.Combo("CNPJ", 0, 0, col), x, y, col, false);
            cmbTipoInscricao.Items.Add("CPF");
            cmbTipoInscricao.Items.Add("CAEPF");
            cmbTipoInscricao.Items.Add("CNO");
            x += col + gap;
            UiBuilder.AddField(form, "INSCRICAO DO ESTABELECIMENTO", txtInscricaoEstabelecimento = UiBuilder.TextBox("CNPJ/CPF/CAEPF", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "LOGRADOURO", txtLogradouro = UiBuilder.TextBox("Logradouro", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "MUNICIPIO", txtMunicipio = UiBuilder.TextBox("Municipio", 0, 0, col), x, y, col, false);

            y = 612;
            x = margem;
            UiBuilder.AddField(form, "UF", txtUf = UiBuilder.TextBox("UF", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "BAIRRO", txtBairro = UiBuilder.TextBox("Bairro", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "COMPLEMENTO", txtComplemento = UiBuilder.TextBox("Complemento", 0, 0, col), x, y, col, false);
            x += col + gap;
            UiBuilder.AddField(form, "CEP", txtCep = UiBuilder.TextBox("CEP", 0, 0, col), x, y, col, false);
            InputFormatHelper.ApplyCepMask(txtCep);

            UiBuilder.AddField(form, "CODIGO POSTAL", txtCodigoPostal = UiBuilder.TextBox("Codigo postal", 0, 0, col), margem, 668, col, false);
        }

        private void MontarObservacao(RoundPanel form, int largura)
        {
            int margem = 18;
            form.Controls.Add(
                UiBuilder.Label(
                    "OBSERVACAO DA CAT *",
                    margem,
                    720,
                    240,
                    20,
                    8F,
                    FontStyle.Bold,
                    UiColors.Red
                )
            );

            txtDescricao = UiBuilder.TextBox(
                "Descreva como o acidente ocorreu, as circunstancias e outras observacoes...",
                margem,
                746,
                largura - (margem * 2)
            );

            txtDescricao.Multiline = true;
            txtDescricao.Height = 65;
            txtDescricao.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form.Controls.Add(txtDescricao);

            CarregarCat();
            CarregarRascunho();
        }

        private CheckBox CriarCheck(string text, int x, int y, int width)
        {
            return new CheckBox
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 22),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UiColors.BodyText
            };
        }

        private void BuildCatHeader(Panel form, int activeTab, int largura)
        {
            form.Controls.Add(UiBuilder.Pill("S-2210", 18, 18, 58, UiColors.Red, Color.White));

            form.Controls.Add(
                UiBuilder.Label(
                    "Comunicacao de Acidente de Trabalho (CAT)",
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
                    "Registro de novo acidente ou doenca ocupacional",
                    88,
                    31,
                    largura - 320,
                    16,
                    7.5F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            btnSalvar = UiBuilder.SmallButton("Salvar CAT", largura - 110, 16, 92, UiColors.AccentBlue, Color.White);
            btnSalvar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalvar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnSalvar.Click += BtnSalvar_Click;
            form.Controls.Add(btnSalvar);

            btnCancelar = UiBuilder.SmallButton("Cancelar", largura - 195, 16, 75, Color.White, UiColors.BodyText);
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

            AddTab(form, "DADOS CADASTRAIS", 18, activeTab == 0, TabDados_Click);
            AddTab(form, "TESTEMUNHAS", 190, activeTab == 1, TabTestemunhas_Click);
            AddTab(form, "DADOS COMPLEMENTARES", 320, activeTab == 2, TabComplementares_Click);
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
                form.Controls.Add(new Panel { Location = new Point(x, 101), Size = new Size(165, 2), BackColor = UiColors.Orange });
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
            txtDataComunicacao.Text = cat.DataComunicacao;
            chkAposentado.Checked = cat.Aposentado;
            cmbArea.Text = string.IsNullOrWhiteSpace(cat.Area) ? "Urbana" : cat.Area;
            cmbFiliacaoPrevSocial.Text = string.IsNullOrWhiteSpace(cat.FiliacaoPrevSocial) ? "Empregado" : cat.FiliacaoPrevSocial;
            cmbEmitente.Text = string.IsNullOrWhiteSpace(cat.Emitente) ? "Empregador" : cat.Emitente;
            txtDataAcidente.Text = cat.DataAcidente;
            cmbTipoAcidente.Text = string.IsNullOrWhiteSpace(cat.TipoAcidente) ? "Acidente tipico" : cat.TipoAcidente;
            txtHoraAcidente.Text = cat.HoraAcidente;
            txtHorasTrabalhadasAntes.Text = cat.HorasTrabalhadasAntes;
            cmbTipoCat.Text = string.IsNullOrWhiteSpace(cat.TipoCat) ? "Inicial" : cat.TipoCat;
            chkHouveObito.Checked = cat.HouveObito;
            txtDataObito.Text = cat.DataObito;
            chkHouveAfastamento.Checked = cat.HouveAfastamento;
            chkRegistroPolicia.Checked = cat.RegistroPolicia;
            txtUltimoDiaTrabalho.Text = cat.UltimoDiaTrabalho;
            txtCodificacaoAcidente.Text = cat.CodificacaoAcidente;
            txtSituacaoGeradora.Text = cat.SituacaoGeradora;
            cmbCatEmitidaPor.Text = string.IsNullOrWhiteSpace(cat.CatEmitidaPor) ? "1 - Iniciativa do empregador" : cat.CatEmitidaPor;
            cmbLocalAcidente.Text = cat.LocalAcidente;
            txtEspecificacaoLocal.Text = cat.EspecificacaoLocal;
            cmbTipoLogradouro.Text = string.IsNullOrWhiteSpace(cat.TipoLogradouro) ? "Avenida" : cat.TipoLogradouro;
            txtNumero.Text = cat.Numero;
            cmbTipoInscricao.Text = string.IsNullOrWhiteSpace(cat.TipoInscricao) ? "CNPJ" : cat.TipoInscricao;
            txtInscricaoEstabelecimento.Text = cat.InscricaoEstabelecimento;
            txtLogradouro.Text = cat.Logradouro;
            txtMunicipio.Text = cat.Municipio;
            txtUf.Text = cat.Uf;
            txtBairro.Text = cat.Bairro;
            txtComplemento.Text = cat.Complemento;
            txtCep.Text = cat.Cep;
            txtCodigoPostal.Text = cat.CodigoPostal;
            txtDescricao.Text = string.IsNullOrWhiteSpace(cat.ObservacaoCat) ? cat.Descricao : cat.ObservacaoCat;
        }

        private void CarregarRascunho()
        {
            if (_catId > 0 || CatDraftState.Current == null || cmbEmpregado == null)
                return;

            CatDraft draft = CatDraftState.Current;
            txtDataComunicacao.Text = string.IsNullOrWhiteSpace(draft.DataComunicacao) ? System.DateTime.Today.ToString("dd/MM/yyyy") : draft.DataComunicacao;
            txtDataAcidente.Text = draft.DataAcidente ?? string.Empty;
            txtHoraAcidente.Text = draft.HoraAcidente ?? string.Empty;
            txtDataObito.Text = draft.DataObito ?? string.Empty;
            cmbTipoCat.Text = string.IsNullOrWhiteSpace(draft.TipoCat) ? "Inicial" : draft.TipoCat;
            SelectComboItem(cmbEmpregado, draft.EmpregadoId);
            chkAposentado.Checked = draft.Aposentado;
            cmbArea.Text = string.IsNullOrWhiteSpace(draft.Area) ? "Urbana" : draft.Area;
            cmbFiliacaoPrevSocial.Text = string.IsNullOrWhiteSpace(draft.FiliacaoPrevSocial) ? "Empregado" : draft.FiliacaoPrevSocial;
            cmbEmitente.Text = string.IsNullOrWhiteSpace(draft.Emitente) ? "Empregador" : draft.Emitente;
            cmbTipoAcidente.Text = string.IsNullOrWhiteSpace(draft.TipoAcidente) ? "Acidente tipico" : draft.TipoAcidente;
            txtHorasTrabalhadasAntes.Text = draft.HorasTrabalhadasAntes ?? string.Empty;
            chkHouveObito.Checked = draft.HouveObito;
            chkHouveAfastamento.Checked = draft.HouveAfastamento;
            chkRegistroPolicia.Checked = draft.RegistroPolicia;
            txtUltimoDiaTrabalho.Text = draft.UltimoDiaTrabalho ?? string.Empty;
            txtCodificacaoAcidente.Text = draft.CodificacaoAcidente ?? string.Empty;
            txtSituacaoGeradora.Text = draft.SituacaoGeradora ?? string.Empty;
            cmbCatEmitidaPor.Text = string.IsNullOrWhiteSpace(draft.CatEmitidaPor) ? "1 - Iniciativa do empregador" : draft.CatEmitidaPor;
            cmbLocalAcidente.Text = string.IsNullOrWhiteSpace(draft.LocalAcidente) ? "Estabelecimento do empregador no Brasil" : draft.LocalAcidente;
            txtEspecificacaoLocal.Text = draft.EspecificacaoLocal ?? string.Empty;
            cmbTipoLogradouro.Text = string.IsNullOrWhiteSpace(draft.TipoLogradouro) ? "Avenida" : draft.TipoLogradouro;
            txtNumero.Text = draft.Numero ?? string.Empty;
            cmbTipoInscricao.Text = string.IsNullOrWhiteSpace(draft.TipoInscricao) ? "CNPJ" : draft.TipoInscricao;
            txtInscricaoEstabelecimento.Text = draft.InscricaoEstabelecimento ?? string.Empty;
            txtLogradouro.Text = draft.Logradouro ?? string.Empty;
            txtMunicipio.Text = draft.Municipio ?? string.Empty;
            txtUf.Text = draft.Uf ?? string.Empty;
            txtBairro.Text = draft.Bairro ?? string.Empty;
            txtComplemento.Text = draft.Complemento ?? string.Empty;
            txtCep.Text = draft.Cep ?? string.Empty;
            txtCodigoPostal.Text = draft.CodigoPostal ?? string.Empty;
            txtDescricao.Text = string.IsNullOrWhiteSpace(draft.ObservacaoCat) ? (draft.Descricao ?? string.Empty) : draft.ObservacaoCat;
        }

        private void SalvarRascunho()
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;
            System.Collections.Generic.List<CatTestemunhaRecord> testemunhas = CatDraftState.Current == null
                ? new System.Collections.Generic.List<CatTestemunhaRecord>()
                : CatDraftState.Current.Testemunhas;

            CatDraftState.Current = new CatDraft
            {
                DataComunicacao = txtDataComunicacao.Text.Trim(),
                DataAcidente = txtDataAcidente.Text.Trim(),
                HoraAcidente = txtHoraAcidente.Text.Trim(),
                DataObito = txtDataObito.Text.Trim(),
                TipoCat = cmbTipoCat.Text.Trim(),
                EmpregadoId = empregado == null ? 0 : empregado.Id,
                Aposentado = chkAposentado.Checked,
                Area = cmbArea.Text.Trim(),
                FiliacaoPrevSocial = cmbFiliacaoPrevSocial.Text.Trim(),
                Emitente = cmbEmitente.Text.Trim(),
                TipoAcidente = cmbTipoAcidente.Text.Trim(),
                HorasTrabalhadasAntes = txtHorasTrabalhadasAntes.Text.Trim(),
                HouveObito = chkHouveObito.Checked,
                HouveAfastamento = chkHouveAfastamento.Checked,
                RegistroPolicia = chkRegistroPolicia.Checked,
                UltimoDiaTrabalho = txtUltimoDiaTrabalho.Text.Trim(),
                CodificacaoAcidente = txtCodificacaoAcidente.Text.Trim(),
                SituacaoGeradora = txtSituacaoGeradora.Text.Trim(),
                CatEmitidaPor = cmbCatEmitidaPor.Text.Trim(),
                LocalAcidente = cmbLocalAcidente.Text.Trim(),
                EspecificacaoLocal = txtEspecificacaoLocal.Text.Trim(),
                TipoLogradouro = cmbTipoLogradouro.Text.Trim(),
                Numero = txtNumero.Text.Trim(),
                TipoInscricao = cmbTipoInscricao.Text.Trim(),
                InscricaoEstabelecimento = txtInscricaoEstabelecimento.Text.Trim(),
                Logradouro = txtLogradouro.Text.Trim(),
                Municipio = txtMunicipio.Text.Trim(),
                Uf = txtUf.Text.Trim(),
                Bairro = txtBairro.Text.Trim(),
                Complemento = txtComplemento.Text.Trim(),
                Cep = txtCep.Text.Trim(),
                CodigoPostal = txtCodigoPostal.Text.Trim(),
                Descricao = txtDescricao.Text.Trim(),
                ObservacaoCat = txtDescricao.Text.Trim(),
                Testemunhas = testemunhas
            };
        }

        private bool ValidarObrigatorios(out string mensagem)
        {
            ComboItem empregado = cmbEmpregado.SelectedItem as ComboItem;

            if (string.IsNullOrWhiteSpace(txtDataComunicacao.Text) ||
                string.IsNullOrWhiteSpace(txtDataAcidente.Text) ||
                string.IsNullOrWhiteSpace(txtHoraAcidente.Text) ||
                string.IsNullOrWhiteSpace(cmbTipoAcidente.Text) ||
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

            System.DateTime dataComunicacao;
            if (!System.DateTime.TryParseExact(txtDataComunicacao.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataComunicacao))
            {
                mensagem = "Informe a data do comunicado no formato dd/mm/yyyy.";
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

            if (!string.IsNullOrWhiteSpace(txtHorasTrabalhadasAntes.Text) && !ValidationHelper.IsValidTime(txtHorasTrabalhadasAntes.Text))
            {
                mensagem = "Informe as horas trabalhadas antes do acidente no formato hh:mm.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtDataObito.Text) && !ValidationHelper.IsValidDate(txtDataObito.Text))
            {
                mensagem = "Informe a data do obito no formato dd/mm/aaaa.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtUltimoDiaTrabalho.Text) && !ValidationHelper.IsValidDate(txtUltimoDiaTrabalho.Text))
            {
                mensagem = "Informe o ultimo dia de trabalho no formato dd/mm/aaaa.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtCep.Text) && !ValidationHelper.IsCompleteCep(txtCep.Text))
            {
                mensagem = "Informe o CEP no formato 00000-000.";
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

            CatRecord catExistente = _catId > 0 ? CadastrosRepository.GetCat(_catId) : null;

            int catId = CadastrosRepository.SaveCat(new CatRecord
            {
                Id = _catId,
                EmpregadoId = empregado.Id,
                DataComunicacao = txtDataComunicacao.Text.Trim(),
                Aposentado = chkAposentado.Checked,
                Area = cmbArea.Text.Trim(),
                FiliacaoPrevSocial = cmbFiliacaoPrevSocial.Text.Trim(),
                Emitente = cmbEmitente.Text.Trim(),
                DataAcidente = txtDataAcidente.Text.Trim(),
                TipoAcidente = cmbTipoAcidente.Text.Trim(),
                HoraAcidente = txtHoraAcidente.Text.Trim(),
                HorasTrabalhadasAntes = txtHorasTrabalhadasAntes.Text.Trim(),
                TipoCat = cmbTipoCat.Text.Trim(),
                HouveObito = chkHouveObito.Checked,
                DataObito = txtDataObito.Text.Trim(),
                HouveAfastamento = chkHouveAfastamento.Checked,
                RegistroPolicia = chkRegistroPolicia.Checked,
                UltimoDiaTrabalho = txtUltimoDiaTrabalho.Text.Trim(),
                CodificacaoAcidente = txtCodificacaoAcidente.Text.Trim(),
                SituacaoGeradora = txtSituacaoGeradora.Text.Trim(),
                CatEmitidaPor = cmbCatEmitidaPor.Text.Trim(),
                LocalAcidente = cmbLocalAcidente.Text.Trim(),
                EspecificacaoLocal = txtEspecificacaoLocal.Text.Trim(),
                TipoLogradouro = cmbTipoLogradouro.Text.Trim(),
                Numero = txtNumero.Text.Trim(),
                TipoInscricao = cmbTipoInscricao.Text.Trim(),
                InscricaoEstabelecimento = txtInscricaoEstabelecimento.Text.Trim(),
                Logradouro = txtLogradouro.Text.Trim(),
                Municipio = txtMunicipio.Text.Trim(),
                Uf = txtUf.Text.Trim(),
                Bairro = txtBairro.Text.Trim(),
                Complemento = txtComplemento.Text.Trim(),
                Cep = txtCep.Text.Trim(),
                CodigoPostal = txtCodigoPostal.Text.Trim(),
                Descricao = txtDescricao.Text.Trim(),
                ObservacaoCat = txtDescricao.Text.Trim(),
                Situacao = catExistente == null ? "Aberta" : catExistente.Situacao,
                ResultadoAso = catExistente == null ? "Aguardando ASO" : catExistente.ResultadoAso,
                ParteCorpoAtingida = catExistente == null ? string.Empty : catExistente.ParteCorpoAtingida,
                Lateralidade = catExistente == null ? string.Empty : catExistente.Lateralidade,
                AgenteCausador = catExistente == null ? string.Empty : catExistente.AgenteCausador,
                Cid10 = catExistente == null ? string.Empty : catExistente.Cid10,
                NaturezaLesao = catExistente == null ? string.Empty : catExistente.NaturezaLesao,
                DuracaoTratamento = catExistente == null ? string.Empty : catExistente.DuracaoTratamento,
                MedicoAssistente = catExistente == null ? string.Empty : catExistente.MedicoAssistente,
                ObservacaoMedica = catExistente == null ? string.Empty : catExistente.ObservacaoMedica
            });

            if (CatDraftState.Current != null && CatDraftState.Current.Testemunhas != null)
                CadastrosRepository.SaveCatTestemunhas(catId, CatDraftState.Current.Testemunhas);

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
