using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public enum DashboardMenu
    {
        Risk,
        Cat,
        Aso,
        Employees,
        Doctors,
        ExamTypes,
        WorkEnvironments,
        Esocial
    }

    public class DashboardFormBase : PrototypeFormBase
    {
        private const int SidebarWidth = 205;
        private const int HeaderHeight = 45;

        protected RoundPanel AppFrame;
        protected Panel SidebarPanel;
        protected Panel ContentPanel;
        protected Panel HeaderPanel;

        protected void BuildDashboardShell(string title, string subtitle, DashboardMenu active)
        {
            BackColor = UiColors.PageBg;
            WindowState = FormWindowState.Maximized;

            AppFrame = new RoundPanel
            {
                Dock = DockStyle.Fill,
                Radius = 0,
                FillColor = UiColors.PageBg,
                BorderColor = Color.Transparent
            };
            Controls.Add(AppFrame);

            BuildSidebar(active);
            BuildHeader(title, subtitle);

            ContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiColors.PageBg,
                Padding = new Padding(20)
            };

            AppFrame.Controls.Add(ContentPanel);
            ContentPanel.BringToFront();
        }

        private void BuildSidebar(DashboardMenu active)
        {
            SidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = SidebarWidth,
                BackColor = UiColors.DarkNavy
            };

            AppFrame.Controls.Add(SidebarPanel);
            SidebarPanel.BringToFront();

            SidebarPanel.Controls.Add(new LogoControl { Location = new Point(15, 14), Size = new Size(42, 42) });
            SidebarPanel.Controls.Add(UiBuilder.Label("Sistema TST - Largo", 62, 14, 140, 22, 8F, FontStyle.Bold, Color.White));
            SidebarPanel.Controls.Add(UiBuilder.Label("Medicina do Trabalho - SENAC", 62, 35, 142, 18, 6.5F, FontStyle.Bold, UiColors.Orange));

            Panel divider = new Panel
            {
                Location = new Point(0, 68),
                Size = new Size(SidebarWidth, 1),
                BackColor = Color.FromArgb(28, 78, 120)
            };
            SidebarPanel.Controls.Add(divider);

            SidebarPanel.Controls.Add(UiBuilder.Label("ESOCIAL — EVENTOS", 16, 82, 170, 20, 7F, FontStyle.Bold, Color.FromArgb(199, 154, 96)));
            AddMenuButton("☢  Fatores de Risco (S-2240)", DashboardMenu.Risk, active, 106, string.Empty);
            AddMenuButton("🚨  CAT — Acidente (S-2210)", DashboardMenu.Cat, active, 136, string.Empty);
            AddMenuButton("🩺  Monit. Saúde / ASO (S-2220)", DashboardMenu.Aso, active, 166, string.Empty);
            AddMenuButton("👥  Empregados", DashboardMenu.Employees, active, 196, string.Empty);

            SidebarPanel.Controls.Add(UiBuilder.Label("CADASTROS BASE", 16, 242, 170, 20, 7F, FontStyle.Bold, Color.FromArgb(199, 154, 96)));
            AddMenuButton("👨‍⚕  Médicos", DashboardMenu.Doctors, active, 266, string.Empty);
            AddMenuButton("🔬  Tipos de Exame", DashboardMenu.ExamTypes, active, 296, string.Empty);
            AddMenuButton("🏢  Ambientes de Trabalho", DashboardMenu.WorkEnvironments, active, 326, string.Empty);
            AddMenuButton("📊  Integração eSocial", DashboardMenu.Esocial, active, 356, string.Empty);

            Panel footer = new Panel
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(0, SidebarPanel.Height - 70),
                Size = new Size(SidebarWidth, 1),
                BackColor = Color.FromArgb(28, 78, 120)
            };
            SidebarPanel.Controls.Add(footer);

            PillLabel avatar = new PillLabel
            {
                Text = UsuarioIniciais(),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(15, SidebarPanel.Height - 58),
                Size = new Size(28, 28),
                FillColor = UiColors.DarkNavy,
                ForeColor = Color.White,
                BorderColor = UiColors.Orange,
                Radius = 14
            };
            SidebarPanel.Controls.Add(avatar);

            Label nome = UiBuilder.Label(UsuarioNome(), 50, SidebarPanel.Height - 62, 125, 18, 8F, FontStyle.Bold, Color.White);
            nome.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            SidebarPanel.Controls.Add(nome);

            Label cargo = UiBuilder.Label("Usuario logado", 50, SidebarPanel.Height - 46, 130, 15, 7F, FontStyle.Regular, UiColors.MutedText);
            cargo.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            SidebarPanel.Controls.Add(cargo);

            RoundButton sairConta = UiBuilder.SmallButton("Sair da conta", 50, SidebarPanel.Height - 27, 100, Color.White, UiColors.Red);
            sairConta.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            sairConta.BorderColor = UiColors.Border;
            sairConta.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            sairConta.Click += Logout_Click;
            SidebarPanel.Controls.Add(sairConta);
        }

        private void AddMenuButton(string text, DashboardMenu menu, DashboardMenu active, int top, string badge)
        {
            Button button = new Button
            {
                Text = text,
                Tag = menu,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(0, top),
                Size = new Size(SidebarWidth, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, active == menu ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = active == menu ? Color.White : Color.FromArgb(145, 178, 207),
                BackColor = active == menu ? Color.FromArgb(46, 70, 87) : UiColors.DarkNavy,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.Click += MenuButton_Click;
            SidebarPanel.Controls.Add(button);

            if (!string.IsNullOrWhiteSpace(badge))
            {
                PillLabel pill = new PillLabel
                {
                    Text = badge,
                    Location = new Point(172, top + 8),
                    Size = new Size(20, 14),
                    FillColor = menu == DashboardMenu.Cat ? UiColors.Red : UiColors.Orange,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 6.5F, FontStyle.Bold),
                    Radius = 7
                };

                SidebarPanel.Controls.Add(pill);
                pill.BringToFront();
            }
        }

        private string UsuarioNome()
        {
            if (AppState.CurrentUser != null && !string.IsNullOrWhiteSpace(AppState.CurrentUser.Name))
                return AppState.CurrentUser.Name.Trim();

            return "Usuario";
        }

        private string UsuarioIniciais()
        {
            string nome = UsuarioNome();
            string[] partes = nome.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
                return "U";

            if (partes.Length == 1)
                return partes[0].Substring(0, 1).ToUpperInvariant();

            return (partes[0].Substring(0, 1) + partes[partes.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private void BuildHeader(string title, string subtitle)
        {
            HeaderPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = HeaderHeight,
                BackColor = Color.White
            };

            AppFrame.Controls.Add(HeaderPanel);
            HeaderPanel.BringToFront();

            HeaderPanel.Controls.Add(UiBuilder.Label(title, 18, 8, 480, 20, 11F, FontStyle.Bold, UiColors.AccentBlue));
            HeaderPanel.Controls.Add(UiBuilder.Label(subtitle, 18, 25, 480, 16, 7.5F, FontStyle.Bold, UiColors.MutedText));

            Label competenciaTexto = UiBuilder.Label("Competência:", 585, 13, 75, 18, 7.5F, FontStyle.Bold, UiColors.MutedText);
            competenciaTexto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            HeaderPanel.Controls.Add(competenciaTexto);

            Label competenciaValor = UiBuilder.Label("03/2026", 655, 13, 60, 18, 8F, FontStyle.Bold, UiColors.AccentBlue);
            competenciaValor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            HeaderPanel.Controls.Add(competenciaValor);

            RoundButton settings = UiBuilder.SmallButton("⚙ Configurações", 708, 10, 112, Color.White, UiColors.BodyText);
            settings.BorderColor = UiColors.Border;
            settings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            settings.Click += Settings_Click;
            HeaderPanel.Controls.Add(settings);

            RoundButton logout = UiBuilder.SmallButton("Sair", 828, 10, 58, Color.White, UiColors.Red);
            logout.BorderColor = UiColors.Border;
            logout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            logout.Click += Logout_Click;
            HeaderPanel.Controls.Add(logout);

            Panel line = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 2,
                BackColor = UiColors.Orange
            };

            HeaderPanel.Controls.Add(line);
        }

        private void MenuButton_Click(object sender, System.EventArgs e)
        {
            DashboardMenu menu = (DashboardMenu)((Control)sender).Tag;

            switch (menu)
            {
                case DashboardMenu.Risk:
                    AppNavigator.Show(new RiskFactorsListForm());
                    break;

                case DashboardMenu.Cat:
                    AppNavigator.Show(new CatListForm());
                    break;

                case DashboardMenu.Aso:
                    AppNavigator.Show(new AsoListForm());
                    break;

                case DashboardMenu.Employees:
                    AppNavigator.Show(new EmployeesForm());
                    break;

                case DashboardMenu.Doctors:
                    AppNavigator.Show(new DoctorsForm());
                    break;

                case DashboardMenu.ExamTypes:
                    AppNavigator.Show(new ExamTypesForm());
                    break;

                case DashboardMenu.WorkEnvironments:
                    AppNavigator.Show(new WorkEnvironmentsForm());
                    break;

                case DashboardMenu.Esocial:
                    AppNavigator.Show(new EsocialIntegrationForm());
                    break;
            }
        }

        private void Settings_Click(object sender, System.EventArgs e)
        {
            using (SettingsForm form = new SettingsForm())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }

        private void Logout_Click(object sender, System.EventArgs e)
        {
            AppState.Logout();
            AppNavigator.Show(new LoginForm());
        }
    }
}
