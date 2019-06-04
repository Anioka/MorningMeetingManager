using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MMM
{
    public static class EMailClass
    {

        public static void sendEmail(string recipient)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient("mail.lmbsoft.com");
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("aleksandra.tosic@lmbsoft.com");
                mail.To.Add(recipient);
                mail.Subject = "Password recovery";
                mail.Body = "Daily report";

                SmtpServer.Port = 465;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                SmtpServer.Credentials = new System.Net.NetworkCredential("aleksandra.tosic@lmbsoft.com", "Lmb#123");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                //MessageBox.Show("E-mail with your password has been sent");
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                //MessageBox.Show("Something went wrong while recovering the password");
            }
        }
}
}
