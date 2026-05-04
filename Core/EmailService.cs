using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace SistemaTstLargoTreze
{
    public static class EmailService
    {
        public static void SendPasswordRecoveryCode(string toEmail, string code)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string user = ConfigurationManager.AppSettings["SmtpUser"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string from = ConfigurationManager.AppSettings["SmtpFrom"];
            string portText = ConfigurationManager.AppSettings["SmtpPort"];
            string sslText = ConfigurationManager.AppSettings["SmtpEnableSsl"];

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(from))
            {
                throw new InvalidOperationException("Configure o SMTP no App.config para enviar codigo por e-mail.");
            }

            int port;
            if (!int.TryParse(portText, out port))
                port = 587;

            bool enableSsl;
            if (!bool.TryParse(sslText, out enableSsl))
                enableSsl = true;

            using (MailMessage message = new MailMessage())
            using (SmtpClient client = new SmtpClient(host, port))
            {
                message.From = new MailAddress(from, "Sistema TST");
                message.To.Add(toEmail);
                message.Subject = "Codigo de recuperacao de senha";
                message.Body =
                    "Ola,\n\n" +
                    "Seu codigo de recuperacao de senha e: " + code + "\n\n" +
                    "Se voce nao solicitou a recuperacao, ignore este e-mail.\n\n" +
                    "Sistema TST Largo Treze";

                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(user, password);
                client.Send(message);
            }
        }
    }
}
