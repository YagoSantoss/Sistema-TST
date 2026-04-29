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
    }
}