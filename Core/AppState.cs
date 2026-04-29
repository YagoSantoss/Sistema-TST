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
            VerificationCode = "123456";
        }

        public static bool CheckVerificationCode(string code)
        {
            return code == VerificationCode;
        }

        public static void ResetPendingPassword(string password)
        {
            UserRepository.UpdatePassword(PendingRecoveryEmail, password);
        }
    }
}
