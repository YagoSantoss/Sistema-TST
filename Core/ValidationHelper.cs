using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace SistemaTstLargoTreze
{
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
            RegexOptions.Compiled
        );

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());
        }

        public static bool IsValidDate(string value)
        {
            DateTime date;
            return !string.IsNullOrWhiteSpace(value) &&
                DateTime.TryParseExact(value.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out date);
        }

        public static bool IsValidTime(string value)
        {
            TimeSpan time;
            return !string.IsNullOrWhiteSpace(value) &&
                TimeSpan.TryParseExact(value.Trim(), @"hh\:mm", null, out time) &&
                time.Hours <= 23 &&
                time.Minutes <= 59;
        }

        public static bool IsCompleteCpf(string value)
        {
            return InputFormatHelper.OnlyDigits(value).Length == 11;
        }

        public static bool IsCompletePhone(string value)
        {
            return InputFormatHelper.OnlyDigits(value).Length == 11;
        }

        public static bool IsCompleteCep(string value)
        {
            return InputFormatHelper.OnlyDigits(value).Length == 8;
        }
    }
}
