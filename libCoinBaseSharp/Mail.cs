using System;
using System.Collections.Generic;
using System.Text;

namespace libCoinBaseSharp
{

    // https://stackoverflow.com/questions/39083372/how-to-read-connection-string-in-net-core/40844342#40844342
    public class SmtpConfig
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public int Port { get; set; }
    }


    public class Mail
    {


        public static void SendMail(SmtpConfig smtpConfig, string strContactName, string strContactEmail, string strContactMessage)
        {
            MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            message.From.Add(new MimeKit.MailboxAddress(strContactEmail, strContactEmail));
            message.To.Add(new MimeKit.MailboxAddress("Noob", "info@noob.com"));
            message.To.Add(new MimeKit.MailboxAddress("Noob2", "noobie@noob.com"));


            message.Subject = "Neue Nachricht von www.daniel-steiger.ch";
            message.Body = new MimeKit.TextPart("plain") { Text = strContactMessage };

            string pw = CoinBaseSharp.StringHelper.ReverseGraphemeClusters(smtpConfig.Pass);
            
            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                // client.Connect("smtp.gmail.com", 587);
                ////Note: only needed if the SMTP server requires authentication
                // client.Authenticate("mymail@gmail.com", "mypassword");

                client.Connect(smtpConfig.Server, smtpConfig.Port, false);
                client.Authenticate(smtpConfig.User, pw);

                client.Send(message);
                client.Disconnect(true);
            }


        }


    }


}
