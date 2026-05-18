using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public enum CadastroBaseTipo
    {
        Medico,
        TipoExame,
        AmbienteTrabalho
    }

    public class CadastroBaseForm : Form
    {
        private readonly CadastroBaseTipo _tipo;
        private readonly bool _edicao;
        private readonly int _id;
        private readonly Dictionary<string, Control> _campos = new Dictionary<string, Control>();
        private RoundButton btnSalvar;
        private RoundButton btnCancelar;
        private RoundButton btnBuscarCep;
        private byte[] _anexoArquivo;
        private string _anexoNome;
        private string _anexoTipo;

        public CadastroBaseForm(CadastroBaseTipo tipo, bool edicao)
            : this(tipo, edicao, 0)
        {
        }

        public CadastroBaseForm(CadastroBaseTipo tipo, bool edicao, int id)
        {
            _tipo = tipo;
            _edicao = edicao;
            _id = id;
            InitializeComponent();
            CarregarCadastro();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = TituloJanela();
            ClientSize = _tipo == CadastroBaseTipo.Medico ? new Size(620, 840) : new Size(620, 520);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = _tipo == CadastroBaseTipo.Medico ? new Size(584, 804) : new Size(584, 484),
                Radius = 10,
                FillColor = Color.White,
                BorderColor = UiColors.Border
            };

            Controls.Add(card);

            card.Controls.Add(UiBuilder.Label(TituloFormulario(), 22, 18, 360, 25, 13F, FontStyle.Bold, UiColors.AccentBlue));
            card.Controls.Add(UiBuilder.Label(SubtituloFormulario(), 22, 43, 470, 18, 8F, FontStyle.Regular, UiColors.MutedText));

            Panel line = new Panel
            {
                Location = new Point(0, 76),
                Size = new Size(584, 1),
                BackColor = UiColors.Border
            };
            card.Controls.Add(line);

            MontarCampos(card);
            MontarRodape(card);

            ResumeLayout(false);
        }

        private void MontarCampos(RoundPanel card)
        {
            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    AddTextField(card, "N Registro", "Número do registro", 22, 100, 130, true);
                    AddComboField(card, "UF Exped.", new[] { "SP", "GO", "MG", "RJ", "PR", "SC", "RS", "BA", "PE", "CE", "DF" }, 182, 100, 95, true);
                    AddTextField(card, "Descrição", "Nome do médico/responsável", 307, 100, 245, true);
                    AddComboField(card, "Órgão de Classe", new[] { "Conselho Regional de Medicina (CRM)", "Conselho Regional de Engenharia e Agronomia (CREA)", "Outros" }, 22, 172, 530, true);
                    AddTextField(card, "Sigla", "Ex.: CRM, CREA", 22, 244, 530, false);
                    AddTextField(card, "Logradouro", "Rua, avenida ou endereço", 22, 316, 530, false);
                    AddTextField(card, "Bairro", "Bairro", 22, 388, 255, false);
                    AddTextField(card, "Número", "Número", 307, 388, 95, false);
                    AddTextField(card, "Cidade", "Cidade - UF", 22, 460, 255, false);
                    AddTextField(card, "CEP", "CEP", 307, 460, 120, false);
                    btnBuscarCep = UiBuilder.SmallButton("Buscar CEP", 437, 484, 90, UiColors.Orange, Color.White);
                    btnBuscarCep.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
                    btnBuscarCep.Click += BuscarCep_Click;
                    card.Controls.Add(btnBuscarCep);
                    AddTextField(card, "E-mail", "médico@empresa.com", 22, 532, 255, false);
                    AddTextField(card, "DDD", "DDD", 307, 532, 70, false);
                    AddTextField(card, "Telefone", "Número do telefone", 397, 532, 155, false);
                    AddTextField(card, "NIT", "NIT/PIS/PASEP", 22, 604, 255, false);
                    AddTextField(card, "CPF", "CPF", 307, 604, 245, false);
                    AddComboField(card, "Tipo telefone", new[] { "Celular", "Comercial", "Residencial" }, 22, 676, 255, false);
                    break;

                case CadastroBaseTipo.TipoExame:
                    AddTextField(card, "Código", "Ex.: EX005", 22, 100, 130, true);
                    AddTextField(card, "Nome do exame", "Nome do exame", 182, 100, 370, true);
                    AddComboField(card, "Tipo", new[] { "Laboratorial", "Clinico", "Especializado", "Imagem" }, 22, 172, 255, true);
                    AddComboField(card, "Periodicidade", new[] { "Anual", "Bienal", "Semestral", "Admissional", "Conforme risco" }, 307, 172, 245, true);
                    AddLookupField(card, "Paciente", CriarComboEmpregados(255), 22, 244, 255, true);
                    AddLookupField(card, "Médico responsável", CriarComboMedicos(245), 307, 244, 245, true);
                    AddAttachmentField(card, "Anexo PDF", 22, 316, 410);
                    break;

                case CadastroBaseTipo.AmbienteTrabalho:
                    AddTextField(card, "Código", "Ex.: ADM-01", 22, 100, 130, true);
                    AddTextField(card, "Ambiente", "Nome do ambiente", 182, 100, 370, true);
                    AddTextField(card, "Setor", "Setor responsável", 22, 172, 255, true);
                    AddComboField(card, "Status", new[] { "Ativo", "Revisar", "Inativo" }, 307, 172, 245, true);
                    break;
            }
        }

        private void AddTextField(Panel parent, string label, string cue, int x, int y, int width, bool required)
        {
            CueTextBox textBox = UiBuilder.TextBox(cue, x, y + 24, width);
            ApplyFieldMask(label, textBox);
            UiBuilder.AddField(parent, label, textBox, x, y, width, required);
            _campos[label] = textBox;
        }

        private void ApplyFieldMask(string label, CueTextBox textBox)
        {
            string key = label.ToLowerInvariant();

            if (key.Contains("cpf"))
                InputFormatHelper.ApplyCpfMask(textBox);
            else if (key.Contains("telefone"))
                InputFormatHelper.ApplyPhoneMask(textBox);
            else if (key.Contains("cep"))
                InputFormatHelper.ApplyCepMask(textBox);
        }

        private void AddComboField(Panel parent, string label, string[] items, int x, int y, int width, bool required)
        {
            ComboBox combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };
            combo.Items.AddRange(items);
            combo.SelectedIndex = 0;
            UiBuilder.AddField(parent, label, combo, x, y, width, required);
            _campos[label] = combo;
        }

        private void AddLookupField(Panel parent, string label, ComboBox combo, int x, int y, int width, bool required)
        {
            UiBuilder.AddField(parent, label, combo, x, y, width, required);
            _campos[label] = combo;
        }

        private ComboBox CriarComboEmpregados(int width)
        {
            ComboBox combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Width = width
            };

            combo.Items.Add(new ComboItem(0, "Selecione o paciente"));

            try
            {
                foreach (EmpregadoRecord empregado in CadastrosRepository.GetEmpregados())
                {
                    combo.Items.Add(new ComboItem(empregado.Id, empregado.Nome + " - " + empregado.Matricula));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponível"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private ComboBox CriarComboMedicos(int width)
        {
            ComboBox combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Width = width
            };

            combo.Items.Add(new ComboItem(0, "Selecione o médico"));

            try
            {
                foreach (MedicoRecord medico in CadastrosRepository.GetMedicos())
                {
                    combo.Items.Add(new ComboItem(medico.Id, medico.Nome + " - " + medico.Crm));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponível"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void AddAttachmentField(Panel parent, string label, int x, int y, int width)
        {
            CueTextBox textBox = UiBuilder.TextBox("Selecione o PDF do exame", x, y + 24, width);
            textBox.ReadOnly = true;
            UiBuilder.AddField(parent, label, textBox, x, y, width, false);
            _campos[label] = textBox;

            RoundButton selecionar = UiBuilder.SmallButton("Selecionar", x + width + 10, y + 24, 90, Color.White, UiColors.BodyText);
            selecionar.BorderColor = UiColors.Border;
            selecionar.Click += SelecionarAnexo_Click;
            parent.Controls.Add(selecionar);
        }

        private void MontarRodape(RoundPanel card)
        {
            int lineY = _tipo == CadastroBaseTipo.Medico ? 734 : 414;
            int buttonY = _tipo == CadastroBaseTipo.Medico ? 758 : 438;

            Panel line = new Panel
            {
                Location = new Point(0, lineY),
                Size = new Size(584, 1),
                BackColor = UiColors.Border
            };
            card.Controls.Add(line);

            btnCancelar = UiBuilder.SmallButton("Cancelar", 390, buttonY, 78, Color.White, UiColors.BodyText);
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            card.Controls.Add(btnCancelar);

            btnSalvar = UiBuilder.SmallButton(_edicao ? "Salvar" : "Cadastrar", 480, buttonY, 82, UiColors.AccentBlue, Color.White);
            btnSalvar.Click += BtnSalvar_Click;
            card.Controls.Add(btnSalvar);
        }

        private string TituloJanela()
        {
            if (_edicao)
                return "Editar cadastro";

            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    return "Novo médico";
                case CadastroBaseTipo.TipoExame:
                    return "Novo exame realizado";
                default:
                    return "Novo ambiente";
            }
        }

        private string TituloFormulario()
        {
            string acao = _edicao ? "Editar" : "Novo";

            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    return acao + " médico";
                case CadastroBaseTipo.TipoExame:
                    return acao + " exame realizado";
                default:
                    return acao + " ambiente de trabalho";
            }
        }

        private string SubtituloFormulario()
        {
            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    return "Preencha os dados do médico habilitado para ASO e laudos.";
                case CadastroBaseTipo.TipoExame:
                    return "Cadastre o exame realizado, vinculando paciente, médico e PDF do exame.";
                default:
                    return "Cadastre o local, setor e status do ambiente de trabalho.";
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarCadastro();
                MessageBox.Show("Cadastro salvo com sucesso.", TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar no MySQL.\n\n" + ex.Message, TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SelecionarAnexo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Selecionar PDF do exame";
                dialog.Filter = "Arquivo PDF|*.pdf";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    FileInfo info = new FileInfo(dialog.FileName);
                    if (info.Length > 10 * 1024 * 1024)
                    {
                        MessageBox.Show("Selecione um PDF de ate 10 MB.", "Anexo PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _anexoArquivo = File.ReadAllBytes(dialog.FileName);
                    _anexoNome = Path.GetFileName(dialog.FileName);
                    _anexoTipo = "application/pdf";
                    SetValue("Anexo PDF", _anexoNome);
                }
            }
        }

        private void BuscarCep_Click(object sender, EventArgs e)
        {
            try
            {
                CepLookupResult endereco = BrazilLookupApi.ConsultarCep(Value("CEP"));
                SetValue("CEP", endereco.Cep);
                SetValue("Logradouro", endereco.Logradouro);
                SetValue("Bairro", endereco.Bairro);
                SetValue("Cidade", endereco.Localidade + " - " + endereco.Uf);
                MessageBox.Show("Endereço preenchido pelo ViaCEP.", TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível consultar o CEP.\n\n" + ex.Message, TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarCadastro()
        {
            if (!_edicao || _id <= 0)
                return;

            try
            {
                switch (_tipo)
                {
                    case CadastroBaseTipo.Medico:
                        MedicoRecord medico = CadastrosRepository.GetMedico(_id);
                        if (medico != null)
                        {
                            SetValue("N Registro", medico.Crm);
                            SetValue("UF Exped.", string.IsNullOrWhiteSpace(medico.UfExpedidor) ? ExtrairUf(medico.OrgaoUf) : medico.UfExpedidor);
                            SetValue("Descrição", medico.Nome);
                            SetValue("Órgão de Classe", string.IsNullOrWhiteSpace(medico.OrgaoClasse) ? "Conselho Regional de Medicina (CRM)" : medico.OrgaoClasse);
                            SetValue("Sigla", medico.Sigla);
                            SetValue("Logradouro", medico.Logradouro);
                            SetValue("Bairro", medico.Bairro);
                            SetValue("Número", medico.Numero);
                            SetValue("Cidade", medico.Cidade);
                            SetValue("CEP", medico.Cep);
                            SetValue("E-mail", medico.Email);
                            SetValue("DDD", medico.Ddd);
                            SetValue("Telefone", medico.Telefone);
                            SetValue("NIT", medico.Nit);
                            SetValue("CPF", medico.Cpf);
                            SetValue("Tipo telefone", string.IsNullOrWhiteSpace(medico.TipoTelefone) ? "Celular" : medico.TipoTelefone);
                        }
                        break;

                    case CadastroBaseTipo.TipoExame:
                        TipoExameRecord exame = CadastrosRepository.GetTipoExame(_id);
                        if (exame != null)
                        {
                            SetValue("Código", exame.Codigo);
                            SetValue("Nome do exame", exame.Nome);
                            SetValue("Tipo", exame.Tipo);
                            SetValue("Periodicidade", exame.Periodicidade);
                            SetComboById("Paciente", exame.EmpregadoId);
                            SetComboById("Médico responsável", exame.MedicoId);
                            _anexoArquivo = exame.AnexoArquivo;
                            _anexoNome = exame.AnexoNome;
                            _anexoTipo = exame.AnexoTipo;
                            SetValue("Anexo PDF", string.IsNullOrWhiteSpace(exame.AnexoNome) ? exame.AnexoImagem : exame.AnexoNome);
                        }
                        break;

                    case CadastroBaseTipo.AmbienteTrabalho:
                        AmbienteTrabalhoRecord ambiente = CadastrosRepository.GetAmbiente(_id);
                        if (ambiente != null)
                        {
                            SetValue("Código", ambiente.Codigo);
                            SetValue("Ambiente", ambiente.Ambiente);
                            SetValue("Setor", ambiente.Setor);
                            SetValue("Status", ambiente.Status);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar o cadastro do MySQL.\n\n" + ex.Message, TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SalvarCadastro()
        {
            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    string email = Value("E-mail");
                    if (!string.IsNullOrWhiteSpace(email) && !ValidationHelper.IsValidEmail(email))
                        throw new InvalidOperationException("Informe um e-mail valido para o médico.");

                    if (!string.IsNullOrWhiteSpace(Value("CPF")) && !ValidationHelper.IsCompleteCpf(Value("CPF")))
                        throw new InvalidOperationException("Informe o CPF no formato 000.000.000-00.");

                    if (!string.IsNullOrWhiteSpace(Value("Telefone")) && !ValidationHelper.IsCompletePhone(Value("Telefone")))
                        throw new InvalidOperationException("Informe o telefone no formato (00) 00000-0000.");

                    if (!string.IsNullOrWhiteSpace(Value("CEP")) && !ValidationHelper.IsCompleteCep(Value("CEP")))
                        throw new InvalidOperationException("Informe o CEP no formato 00000-000.");

                    string orgaoClasse = Required("Órgão de Classe");
                    string ufExpedidor = Required("UF Exped.");

                    CadastrosRepository.SaveMedico(new MedicoRecord
                    {
                        Id = _id,
                        Nome = Required("Descrição"),
                        Crm = Required("N Registro"),
                        OrgaoUf = SiglaOrgao(orgaoClasse) + "-" + ufExpedidor,
                        Especialidade = "Médico/Responsável Tecnico",
                        Email = email,
                        UfExpedidor = ufExpedidor,
                        OrgaoClasse = orgaoClasse,
                        Sigla = Value("Sigla"),
                        Logradouro = Value("Logradouro"),
                        Bairro = Value("Bairro"),
                        Numero = Value("Número"),
                        Cidade = Value("Cidade"),
                        Cep = Value("CEP"),
                        Ddd = Value("DDD"),
                        Nit = Value("NIT"),
                        Cpf = Value("CPF"),
                        Telefone = Value("Telefone"),
                        TipoTelefone = Value("Tipo telefone")
                    });
                    break;

                case CadastroBaseTipo.TipoExame:
                    CadastrosRepository.SaveTipoExame(new TipoExameRecord
                    {
                        Id = _id,
                        Codigo = Required("Código"),
                        Nome = Required("Nome do exame"),
                        Tipo = Required("Tipo"),
                        Periodicidade = Required("Periodicidade"),
                        EmpregadoId = RequiredId("Paciente"),
                        MedicoId = RequiredId("Médico responsável"),
                        AnexoImagem = Value("Anexo PDF"),
                        AnexoNome = _anexoNome,
                        AnexoTipo = _anexoTipo,
                        AnexoArquivo = _anexoArquivo
                    });
                    break;

                case CadastroBaseTipo.AmbienteTrabalho:
                    CadastrosRepository.SaveAmbiente(new AmbienteTrabalhoRecord
                    {
                        Id = _id,
                        Codigo = Required("Código"),
                        Ambiente = Required("Ambiente"),
                        Setor = Required("Setor"),
                        Status = Required("Status")
                    });
                    break;
            }
        }

        private string Required(string key)
        {
            string value = Value(key);
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Preencha o campo: " + key);

            return value;
        }

        private int RequiredId(string key)
        {
            Control control = _campos[key];
            ComboBox combo = control as ComboBox;
            ComboItem item = combo == null ? null : combo.SelectedItem as ComboItem;
            if (item == null || item.Id <= 0)
                throw new InvalidOperationException("Preencha o campo: " + key);

            return item.Id;
        }

        private string Value(string key)
        {
            Control control = _campos[key];
            ComboBox combo = control as ComboBox;
            if (combo != null)
                return combo.Text.Trim();

            return control.Text.Trim();
        }

        private void SetValue(string key, string value)
        {
            Control control = _campos[key];
            ComboBox combo = control as ComboBox;
            if (combo != null)
            {
                int index = combo.Items.IndexOf(value);
                if (index < 0)
                {
                    combo.Items.Add(value);
                    index = combo.Items.IndexOf(value);
                }

                combo.SelectedIndex = index;
                return;
            }

            control.Text = value ?? string.Empty;
        }

        private void SetComboById(string key, int? id)
        {
            if (!id.HasValue || !_campos.ContainsKey(key))
                return;

            ComboBox combo = _campos[key] as ComboBox;
            if (combo == null)
                return;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                ComboItem item = combo.Items[i] as ComboItem;
                if (item != null && item.Id == id.Value)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private string SiglaOrgao(string orgaoClasse)
        {
            if (orgaoClasse.IndexOf("CREA", StringComparison.OrdinalIgnoreCase) >= 0)
                return "CREA";

            if (orgaoClasse.IndexOf("CRM", StringComparison.OrdinalIgnoreCase) >= 0)
                return "CRM";

            return "OUT";
        }

        private string ExtrairUf(string orgaoUf)
        {
            if (string.IsNullOrWhiteSpace(orgaoUf))
                return "SP";

            int index = orgaoUf.LastIndexOf("-");
            if (index >= 0 && index < orgaoUf.Length - 1)
                return orgaoUf.Substring(index + 1).Trim();

            return "SP";
        }
    }
}
