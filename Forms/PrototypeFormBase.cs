using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public class PrototypeFormBase : Form
    {
        public PrototypeFormBase()
        {
            Font = new Font("Segoe UI", 9F);
            DoubleBuffered = true;
            Text = "Sistema TST - Largo Treze";
            BackColor = Color.White;
            ApplyAppIcon(this);

            // Permite maximizar a janela
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;

            // Abre a tela ocupando 100%
            WindowState = FormWindowState.Maximized;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            WindowState = FormWindowState.Maximized;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        internal static void ApplyAppIcon(Form form)
        {
            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (icon != null)
                    form.Icon = icon;
            }
            catch
            {
                // Se o ícone não puder ser carregado, o Windows usa o padrão do executável.
            }
        }
    }
}
