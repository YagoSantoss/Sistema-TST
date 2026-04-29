using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppNavigator.Start(new LoginForm());
        }
    }
}
