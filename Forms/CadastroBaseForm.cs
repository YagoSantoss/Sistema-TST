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
            ClientSize = new Size(620, 410);
            Font = new Font("Segoe UI", 9F);
            BackColor = UiColors.PageBg;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            RoundPanel card = new RoundPanel
            {
                Location = new Point(18, 18),
                Size = new Size(584, 374),
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

        private void MontarRodape(RoundPanel card)
        {
            Panel line = new Panel
            {
                Location = new Point(0, 304),
                Size = new Size(584, 1),
                BackColor = UiColors.Border
            };
            card.Controls.Add(line);

            btnCancelar = UiBuilder.SmallButton("Cancelar", 390, 328, 78, Color.White, UiColors.BodyText);
            btnCancelar.BorderColor = UiColors.Border;
            btnCancelar.Click += BtnCancelar_Click;
            card.Controls.Add(btnCancelar);

            btnSalvar = UiBuilder.SmallButton(_edicao ? "Salvar" : "Cadastrar", 480, 328, 82, UiColors.AccentBlue, Color.White);
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
                    CadastrosRepository.SaveMedico(new MedicoRecord
                    {
                        Id = _id,
                        Nome = Required("Nome"),
                        Crm = Required("CRM"),
                        OrgaoUf = Required("Orgao / UF"),
                        Especialidade = Required("Especialidade"),
                        Email = Value("E-mail")
                    });
                    break;

                case CadastroBaseTipo.TipoExame:
                    CadastrosRepository.SaveTipoExame(new TipoExameRecord
                    {
                        Id = _id,
                        Codigo = Required("Codigo"),
                        Nome = Required("Nome do exame"),
                        Tipo = Required("Tipo"),
                        Periodicidade = Required("Periodicidade")
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
    }
}
