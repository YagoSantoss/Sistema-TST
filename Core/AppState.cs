using System;
using System.Security.Cryptography;

namespace SistemaTstLargoTreze
{
    public sealed class UserAccount
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public static class AppState
    {
        public static UserAccount CurrentUser { get; private set; }
        public static string PendingRecoveryEmail { get; private set; }
        public static string VerificationCode { get; private set; } = "123456";
        public static DateTime? VerificationCodeExpiresAt { get; private set; }

        public static bool ValidateLogin(string login, string password)
        {
            UserAccount user = UserRepository.ValidateLogin(login, password);

            if (user == null)
            {
                return false;
            }

            CurrentUser = user;
            return true;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static void Register(string name, string email, string password)
        {
            UserRepository.Register(name, email, password);
        }

        public static void BeginPasswordRecovery(string email)
        {
            PendingRecoveryEmail = email;
            VerificationCode = GenerateVerificationCode();
            VerificationCodeExpiresAt = DateTime.Now.AddMinutes(10);
            EmailService.SendPasswordRecoveryCode(email, VerificationCode);
        }

        public static void ResendPasswordRecoveryCode()
        {
            if (string.IsNullOrWhiteSpace(PendingRecoveryEmail))
                throw new InvalidOperationException("Informe o e-mail novamente para reenviar o codigo.");

            VerificationCode = GenerateVerificationCode();
            VerificationCodeExpiresAt = DateTime.Now.AddMinutes(10);
            EmailService.SendPasswordRecoveryCode(PendingRecoveryEmail, VerificationCode);
        }

        public static bool CheckVerificationCode(string code)
        {
            return !string.IsNullOrWhiteSpace(PendingRecoveryEmail) &&
                   VerificationCodeExpiresAt.HasValue &&
                   VerificationCodeExpiresAt.Value >= DateTime.Now &&
                   code == VerificationCode;
        }

        public static void ResetPendingPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(PendingRecoveryEmail))
                throw new System.InvalidOperationException("Inicie a recuperacao de senha pelo e-mail cadastrado.");

            UserRepository.UpdatePassword(PendingRecoveryEmail, password);
            PendingRecoveryEmail = null;
            VerificationCode = null;
            VerificationCodeExpiresAt = null;
        }

        private static string GenerateVerificationCode()
        {
            byte[] bytes = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            int value = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return value.ToString("D6");
        }
    }
}
