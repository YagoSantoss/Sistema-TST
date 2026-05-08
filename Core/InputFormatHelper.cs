using System;
using System.Linq;
using System.Windows.Forms;

namespace SistemaTstLargoTreze
{
    public static class InputFormatHelper
    {
        public static void ApplyDateMask(TextBox textBox)
        {
            ApplyMask(textBox, 8, digits =>
            {
                if (digits.Length <= 2)
                    return digits;
                if (digits.Length <= 4)
                    return digits.Substring(0, 2) + "/" + digits.Substring(2);
                return digits.Substring(0, 2) + "/" + digits.Substring(2, 2) + "/" + digits.Substring(4);
            });
        }

        public static void ApplyTimeMask(TextBox textBox)
        {
            ApplyMask(textBox, 4, digits =>
            {
                if (digits.Length <= 2)
                    return digits;
                return digits.Substring(0, 2) + ":" + digits.Substring(2);
            });
        }

        public static void ApplyCpfMask(TextBox textBox)
        {
            ApplyMask(textBox, 11, digits =>
            {
                if (digits.Length <= 3)
                    return digits;
                if (digits.Length <= 6)
                    return digits.Substring(0, 3) + "." + digits.Substring(3);
                if (digits.Length <= 9)
                    return digits.Substring(0, 3) + "." + digits.Substring(3, 3) + "." + digits.Substring(6);
                return digits.Substring(0, 3) + "." + digits.Substring(3, 3) + "." + digits.Substring(6, 3) + "-" + digits.Substring(9);
            });
        }

        public static void ApplyPhoneMask(TextBox textBox)
        {
            ApplyMask(textBox, 11, digits =>
            {
                if (digits.Length <= 2)
                    return digits.Length == 0 ? string.Empty : "(" + digits;
                if (digits.Length <= 7)
                    return "(" + digits.Substring(0, 2) + ") " + digits.Substring(2);
                return "(" + digits.Substring(0, 2) + ") " + digits.Substring(2, 5) + "-" + digits.Substring(7);
            });
        }

        public static void ApplyCepMask(TextBox textBox)
        {
            ApplyMask(textBox, 8, digits =>
            {
                if (digits.Length <= 5)
                    return digits;
                return digits.Substring(0, 5) + "-" + digits.Substring(5);
            });
        }

        private static void ApplyMask(TextBox textBox, int maxDigits, Func<string, string> formatter)
        {
            bool changing = false;

            textBox.TextChanged += (sender, e) =>
            {
                if (changing)
                    return;

                changing = true;
                string digits = OnlyDigits(textBox.Text);
                if (digits.Length > maxDigits)
                    digits = digits.Substring(0, maxDigits);

                textBox.Text = formatter(digits);
                textBox.SelectionStart = textBox.Text.Length;
                changing = false;
            };
        }

        public static string OnlyDigits(string value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }
    }
}
