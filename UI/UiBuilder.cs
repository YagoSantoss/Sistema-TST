using System.Drawing;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public static class UiBuilder
    {
        public static Label Label(string text, int x, int y, int width, int height, float size, FontStyle style, Color color)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", size, style),
                ForeColor = color,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        public static Label CenterLabel(string text, int x, int y, int width, int height, float size, FontStyle style, Color color)
        {
            Label label = Label(text, x, y, width, height, size, style, color);
            label.TextAlign = ContentAlignment.MiddleCenter;
            return label;
        }

        public static CueTextBox TextBox(string cue, int x, int y, int width)
        {
            return new CueTextBox
            {
                Cue = cue,
                Location = new Point(x, y),
                Size = new Size(width, 32),
                Font = new Font("Segoe UI", 9F),
                ForeColor = UiColors.BodyText
            };
        }

        public static ComboBox Combo(string text, int x, int y, int width)
        {
            ComboBox combo = new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 30),
                Font = new Font("Segoe UI", 9F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.Add(text);
            combo.SelectedIndex = 0;
            return combo;
        }

        public static RoundButton Button(string text, int x, int y, int width, int height, Color back, Color fore)
        {
            return new RoundButton
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = back,
                HoverColor = ControlPaint.Dark(back),
                ForeColor = fore,
                Radius = 8
            };
        }

        public static LinkLabel Link(string text, int x, int y, int width, int height)
        {
            LinkLabel link = new LinkLabel
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                LinkColor = UiColors.Navy,
                ActiveLinkColor = UiColors.Orange,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            return link;
        }

        public static void AddField(Panel parent, string labelText, Control field, int x, int y, int width, bool required)
        {
            Color labelColor = required ? UiColors.Red : UiColors.BodyText;
            parent.Controls.Add(Label(labelText + (required ? " *" : ""), x, y, width, 20, 8.5F, FontStyle.Bold, labelColor));
            field.Location = new Point(x, y + 24);
            field.Size = new Size(width, 32);
            parent.Controls.Add(field);
        }

        public static PillLabel Pill(string text, int x, int y, int width, Color fill, Color fore)
        {
            return new PillLabel
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 20),
                FillColor = fill,
                ForeColor = fore
            };
        }

        public static RoundPanel Card(int x, int y, int width, int height)
        {
            return new RoundPanel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Radius = 10,
                FillColor = Color.White,
                BorderColor = UiColors.Border
            };
        }

        public static void AddMetric(Panel parent, int x, int y, int width, string title, string value, string pill, Color accent, Color pillBack)
        {
            RoundPanel card = Card(x, y, width, 88);
            card.BorderColor = Color.FromArgb(220, 228, 237);
            parent.Controls.Add(card);
            card.Controls.Add(new Panel { Location = new Point(0, 0), Size = new Size(width, 3), BackColor = accent });
            card.Controls.Add(Label(title, 16, 14, width - 32, 18, 8.5F, FontStyle.Bold, UiColors.MutedText));
            card.Controls.Add(Label(value, 16, 30, 90, 34, 19F, FontStyle.Bold, accent));
            card.Controls.Add(Pill(pill, 16, 64, 105, pillBack, accent));
        }

        public static RoundButton SmallButton(string text, int x, int y, int width, Color back, Color fore)
        {
            return Button(text, x, y, width, 25, back, fore);
        }

        public static Label HeaderCell(string text, int x, int y, int width)
        {
            return CenterLabel(text, x, y, width, 28, 8.5F, FontStyle.Bold, Color.White);
        }

        public static Label Cell(string text, int x, int y, int width, Color color, FontStyle style)
        {
            return CenterLabel(text, x, y, width, 34, 8.5F, style, color);
        }
    }
}
