using System;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public sealed class AppNavigator : ApplicationContext
    {
        private static AppNavigator current;

        private AppNavigator(Form startForm)
        {
            Switch(startForm);
        }

        public static void Start(Form startForm)
        {
            current = new AppNavigator(startForm);
            Application.Run(current);
        }

        public static void Show(Form nextForm)
        {
            if (current == null)
            {
                nextForm.Show();
                return;
            }

            current.Switch(nextForm);
        }

        public static void Exit()
        {
            if (current == null)
            {
                Application.Exit();
                return;
            }

            current.ExitThread();
        }

        private void Switch(Form nextForm)
        {
            Form previous = MainForm;
            if (previous != null)
            {
                previous.FormClosed -= OnMainFormClosed;
            }

            MainForm = nextForm;
            nextForm.StartPosition = FormStartPosition.CenterScreen;
            nextForm.FormClosed += OnMainFormClosed;
            nextForm.Show();

            if (previous != null && !previous.IsDisposed)
            {
                previous.Close();
            }
        }

        private void OnMainFormClosed(object sender, FormClosedEventArgs e)
        {
            ExitThread();
        }
    }
}
