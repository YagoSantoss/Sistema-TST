using System;
using System.Collections.Generic;
using System.Drawing;
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
            ClientSize = new Size(620, 520);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = new Size(584, 484),
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
                    AddTextField(card, "Nome", "Nome completo do medico", 22, 100, 255, true);
                    AddTextField(card, "CRM", "Ex.: 12345-SP", 307, 100, 120, true);
                    AddTextField(card, "Orgao / UF", "Ex.: CRM-SP", 457, 100, 95, true);
                    AddTextField(card, "Especialidade", "Medicina do Trabalho", 22, 172, 255, true);
                    AddTextField(card, "E-mail", "medico@empresa.com", 307, 172, 245, false);
                    break;

                case CadastroBaseTipo.TipoExame:
                    AddTextField(card, "Codigo", "Ex.: EX005", 22, 100, 130, true);
                    AddTextField(card, "Nome do exame", "Nome do exame", 182, 100, 370, true);
                    AddComboField(card, "Tipo", new[] { "Laboratorial", "Clinico", "Especializado", "Imagem" }, 22, 172, 255, true);
                    AddComboField(card, "Periodicidade", new[] { "Anual", "Bienal", "Semestral", "Admissional", "Conforme risco" }, 307, 172, 245, true);
                    AddLookupField(card, "Paciente", CriarComboEmpregados(255), 22, 244, 255, true);
                    AddLookupField(card, "Medico responsavel", CriarComboMedicos(245), 307, 244, 245, true);
                    AddAttachmentField(card, "Anexo imagem", 22, 316, 410);
                    break;

                case CadastroBaseTipo.AmbienteTrabalho:
                    AddTextField(card, "Codigo", "Ex.: ADM-01", 22, 100, 130, true);
                    AddTextField(card, "Ambiente", "Nome do ambiente", 182, 100, 370, true);
                    AddTextField(card, "Setor", "Setor responsavel", 22, 172, 255, true);
                    AddComboField(card, "Status", new[] { "Ativo", "Revisar", "Inativo" }, 307, 172, 245, true);
                    break;
            }
        }

        private void AddTextField(Panel parent, string label, string cue, int x, int y, int width, bool required)
        {
            CueTextBox textBox = UiBuilder.TextBox(cue, x, y + 24, width);
            UiBuilder.AddField(parent, label, textBox, x, y, width, required);
            _campos[label] = textBox;
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
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
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

            combo.Items.Add(new ComboItem(0, "Selecione o medico"));

            try
            {
                foreach (MedicoRecord medico in CadastrosRepository.GetMedicos())
                {
                    combo.Items.Add(new ComboItem(medico.Id, medico.Nome + " - " + medico.Crm));
                }
            }
            catch
            {
                combo.Items.Add(new ComboItem(0, "MySQL indisponivel"));
            }

            combo.SelectedIndex = 0;
            return combo;
        }

        private void AddAttachmentField(Panel parent, string label, int x, int y, int width)
        {
            CueTextBox textBox = UiBuilder.TextBox("Selecione uma imagem ou foto", x, y + 24, width);
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
            Panel line = new Panel
            {
                Location = new Point(0, 414),
                Size = new Size(584, 1),
                BackColor = UiColors.Border
            };
            card.Controls.Add(line);

            btnCancelar = UiBuilder.SmallButton("Cancelar", 390, 438, 78, Color.White, UiColors.BodyText);
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            card.Controls.Add(btnCancelar);

            btnSalvar = UiBuilder.SmallButton(_edicao ? "Salvar" : "Cadastrar", 480, 438, 82, UiColors.AccentBlue, Color.White);
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
                    return "Novo medico";
                case CadastroBaseTipo.TipoExame:
                    return "Novo exame";
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
                    return acao + " medico";
                case CadastroBaseTipo.TipoExame:
                    return acao + " exame";
                default:
                    return acao + " ambiente de trabalho";
            }
        }

        private string SubtituloFormulario()
        {
            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    return "Preencha os dados do medico habilitado para ASO e laudos.";
                case CadastroBaseTipo.TipoExame:
                    return "Cadastre o exame complementar com tipo e periodicidade.";
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
                MessageBox.Show("Nao foi possivel salvar no MySQL.\n\n" + ex.Message, TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dialog.Title = "Selecionar imagem do exame";
                dialog.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos os arquivos|*.*";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SetValue("Anexo imagem", dialog.FileName);
                }
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
                            SetValue("Nome", medico.Nome);
                            SetValue("CRM", medico.Crm);
                            SetValue("Orgao / UF", medico.OrgaoUf);
                            SetValue("Especialidade", medico.Especialidade);
                            SetValue("E-mail", medico.Email);
                        }
                        break;

                    case CadastroBaseTipo.TipoExame:
                        TipoExameRecord exame = CadastrosRepository.GetTipoExame(_id);
                        if (exame != null)
                        {
                            SetValue("Codigo", exame.Codigo);
                            SetValue("Nome do exame", exame.Nome);
                            SetValue("Tipo", exame.Tipo);
                            SetValue("Periodicidade", exame.Periodicidade);
                            SetComboById("Paciente", exame.EmpregadoId);
                            SetComboById("Medico responsavel", exame.MedicoId);
                            SetValue("Anexo imagem", exame.AnexoImagem);
                        }
                        break;

                    case CadastroBaseTipo.AmbienteTrabalho:
                        AmbienteTrabalhoRecord ambiente = CadastrosRepository.GetAmbiente(_id);
                        if (ambiente != null)
                        {
                            SetValue("Codigo", ambiente.Codigo);
                            SetValue("Ambiente", ambiente.Ambiente);
                            SetValue("Setor", ambiente.Setor);
                            SetValue("Status", ambiente.Status);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel carregar o cadastro do MySQL.\n\n" + ex.Message, TituloJanela(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SalvarCadastro()
        {
            switch (_tipo)
            {
                case CadastroBaseTipo.Medico:
                    string email = Value("E-mail");
                    if (!string.IsNullOrWhiteSpace(email) && !ValidationHelper.IsValidEmail(email))
                        throw new InvalidOperationException("Informe um e-mail valido para o medico.");

                    CadastrosRepository.SaveMedico(new MedicoRecord
                    {
                        Id = _id,
                        Nome = Required("Nome"),
                        Crm = Required("CRM"),
                        OrgaoUf = Required("Orgao / UF"),
                        Especialidade = Required("Especialidade"),
                        Email = email
                    });
                    break;

                case CadastroBaseTipo.TipoExame:
                    CadastrosRepository.SaveTipoExame(new TipoExameRecord
                    {
                        Id = _id,
                        Codigo = Required("Codigo"),
                        Nome = Required("Nome do exame"),
                        Tipo = Required("Tipo"),
                        Periodicidade = Required("Periodicidade"),
                        EmpregadoId = RequiredId("Paciente"),
                        MedicoId = RequiredId("Medico responsavel"),
                        AnexoImagem = Value("Anexo imagem")
                    });
                    break;

                case CadastroBaseTipo.AmbienteTrabalho:
                    CadastrosRepository.SaveAmbiente(new AmbienteTrabalhoRecord
                    {
                        Id = _id,
                        Codigo = Required("Codigo"),
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
    }
}
