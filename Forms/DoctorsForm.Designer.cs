using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public partial class DoctorsForm
    {
        private RoundButton btnNovo;

        private bool _montandoConteudo = false;

        private void InitializeComponent()
        {
            SuspendLayout();

            BuildDashboardShell(
                "Médicos",
                "Cadastros Base · Médicos habilitados",
                DashboardMenu.Doctors
            );

            ContentPanel.AutoScroll = true;

            MontarConteudoMedicos();

            ContentPanel.Resize += (sender, e) =>
            {
                MontarConteudoMedicos();
            };

            ResumeLayout(false);
        }

        private void MontarConteudoMedicos()
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

            RoundPanel table = UiBuilder.Card(margem, 18, larguraDisponivel, 210);
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ContentPanel.Controls.Add(table);

            MontarCabecalho(table, larguraDisponivel);
            MontarCabecalhoTabela(table, larguraDisponivel);
            MontarLinhas(table, larguraDisponivel);
            MontarRodape(table, larguraDisponivel);

            ContentPanel.ResumeLayout(false);

            _montandoConteudo = false;
        }

        private void MontarCabecalho(RoundPanel table, int largura)
        {
            table.Controls.Add(
                UiBuilder.Label(
                    "Cadastro de Médicos",
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
                    "Médicos habilitados para emissão de ASO e laudos",
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
                "+ Novo Médico",
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

            int nomeW = (int)(largura * 0.25);
            int crmW = 120;
            int orgaoW = 130;
            int especialidadeW = (int)(largura * 0.23);
            int acoesW = 90;
            int emailW = largura - nomeW - crmW - orgaoW - especialidadeW - acoesW - 20;

            if (emailW < 150)
                emailW = 150;

            int x = 12;

            header.Controls.Add(UiBuilder.HeaderCell("NOME", x, 0, nomeW));
            x += nomeW;

            header.Controls.Add(UiBuilder.HeaderCell("CRM", x, 0, crmW));
            x += crmW;

            header.Controls.Add(UiBuilder.HeaderCell("ÓRGÃO / UF", x, 0, orgaoW));
            x += orgaoW;

            header.Controls.Add(UiBuilder.HeaderCell("ESPECIALIDADE", x, 0, especialidadeW));
            x += especialidadeW;

            header.Controls.Add(UiBuilder.HeaderCell("E-MAIL", x, 0, emailW));
            x += emailW;

            header.Controls.Add(UiBuilder.HeaderCell("AÇÕES", x, 0, acoesW));
        }

        private void MontarLinhas(RoundPanel table, int largura)
        {
            try
            {
                List<MedicoRecord> medicos = CadastrosRepository.GetMedicos();

                if (medicos.Count == 0)
                {
                    table.Controls.Add(UiBuilder.CenterLabel("Nenhum medico cadastrado", 0, 98, largura, 34, 8.5F, FontStyle.Regular, UiColors.MutedText));
                    return;
                }

                int y = 83;
                foreach (MedicoRecord medico in medicos)
                {
                    AddDoctorRow(table, largura, y, medico.Id, medico.Nome, medico.Crm, medico.OrgaoUf, medico.Especialidade, medico.Email);
                    y += 38;
                }
            }
            catch
            {
                table.Controls.Add(UiBuilder.CenterLabel("Nao foi possivel carregar os medicos do MySQL", 0, 98, largura, 34, 8.5F, FontStyle.Regular, UiColors.Red));
            }
        }

        private void MontarRodape(RoundPanel table, int largura)
        {
            Label total = UiBuilder.Label(
                TotalMedicosTexto(),
                largura - 170,
                167,
                150,
                20,
                7.5F,
                FontStyle.Regular,
                UiColors.MutedText
            );

            total.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            table.Controls.Add(total);
        }

        private void AddDoctorRow(
            Panel table,
            int largura,
            int y,
            int id,
            string nome,
            string crm,
            string orgao,
            string especialidade,
            string email)
        {
            Panel row = new Panel
            {
                Location = new Point(0, y),
                Size = new Size(largura, 38),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            table.Controls.Add(row);

            int nomeW = (int)(largura * 0.25);
            int crmW = 120;
            int orgaoW = 130;
            int especialidadeW = (int)(largura * 0.23);
            int acoesW = 90;
            int emailW = largura - nomeW - crmW - orgaoW - especialidadeW - acoesW - 20;

            if (emailW < 150)
                emailW = 150;

            int x = 12;

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
                    crm,
                    x,
                    2,
                    crmW,
                    34,
                    8F,
                    FontStyle.Bold,
                    UiColors.AccentBlue
                )
            );

            x += crmW;

            row.Controls.Add(
                UiBuilder.Label(
                    orgao,
                    x,
                    2,
                    orgaoW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += orgaoW;

            row.Controls.Add(
                UiBuilder.Label(
                    especialidade,
                    x,
                    2,
                    especialidadeW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.BodyText
                )
            );

            x += especialidadeW;

            row.Controls.Add(
                UiBuilder.Label(
                    email,
                    x,
                    2,
                    emailW,
                    34,
                    8F,
                    FontStyle.Regular,
                    UiColors.MutedText
                )
            );

            x += emailW;

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

        private string TotalMedicosTexto()
        {
            try
            {
                int total = CadastrosRepository.GetMedicos().Count;
                return total + (total == 1 ? " medico cadastrado" : " medicos cadastrados");
            }
            catch
            {
                return "MySQL indisponivel";
            }
        }
    }
}
