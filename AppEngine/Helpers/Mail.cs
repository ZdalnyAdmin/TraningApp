using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Configuration;

namespace AppEngine.Helpers
{
    public static class Mail
    {
        public static void Send(MailMessage message, MailAccount account)
        {
            try
            {
                using (var client = new SmtpClient())
                {
#if DEBUG
                    // On production should be properly configured IIS.
                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate(object s, X509Certificate certificate,
                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
#endif
                    var smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                    client.Port = smtp.Network.Port;
                    client.Host = smtp.Network.Host;
                    client.EnableSsl = smtp.Network.EnableSsl;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = getNetworkCredential(account);
                    client.Send(message);
                }

                Log.Info(string.Format("Wiadomość została wysłana do {0}", message.To.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Wystąpił błąd podczas wysyłania wiadomości do {0}", message.To.ToString()));
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        private static NetworkCredential getNetworkCredential(MailAccount account)
        {
            switch (account)
            {

                case MailAccount.REGISTER:
                    return new NetworkCredential(AppSettings.Setting<string>("registerMail"), AppSettings.Setting<string>("registerPassword"));

                case MailAccount.INVITATION:
                    return new NetworkCredential(AppSettings.Setting<string>("invitationMail"), AppSettings.Setting<string>("invitationPassword"));

                case MailAccount.ACTIVATION:
                    return new NetworkCredential(AppSettings.Setting<string>("activationMail"), AppSettings.Setting<string>("activationPassword"));

                case MailAccount.EVENT:
                    return new NetworkCredential(AppSettings.Setting<string>("eventMail"), AppSettings.Setting<string>("eventPrassword"));

                case MailAccount.CONFIRMATION:
                    return new NetworkCredential(AppSettings.Setting<string>("confirmationMail"), AppSettings.Setting<string>("confirmationPassword"));

                case MailAccount.DELETE:
                    return new NetworkCredential(AppSettings.Setting<string>("deleteMail"), AppSettings.Setting<string>("deletePassword"));

                case MailAccount.ADMIN:
                default:
                    return new NetworkCredential(AppSettings.Setting<string>("adminMail"), AppSettings.Setting<string>("adminPassword"));
            }
        }
    }

    public enum MailAccount
    {
        REGISTER = 0,

        INVITATION = 1,

        ACTIVATION = 2,

        EVENT = 3,

        CONFIRMATION = 4,

        DELETE = 5,

        ADMIN = 6
    }
}
