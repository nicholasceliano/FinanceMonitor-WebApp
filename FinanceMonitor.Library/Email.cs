using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using FinanceMonitor.Config;
using FinanceMonitor.Library.Helpers;
using SendGridMail;

namespace FinanceMonitor.Library
{
    public class Email
    {
        #region Private Valiables
        
        private string Subject { get; set; }
        private string Message { get; set; }
        private EmailType EmailToBeSent { get; set; }

        private new List<string> To = new List<string>();
        private readonly string sendgridUsername = AccountConfiguration.Current.SendGrid.Username;
        private readonly string sendgridPassword = AccountConfiguration.Current.SendGrid.Password;
        private const string fromEmail = "no-reply@FinanceMonitor.com";
        private const string fromName = "Finance Monitor";

        #endregion

        public enum EmailType
        {
            AccountValueStatusDailyEmail,
            NoHTML
        }

        public Email(string recipient, EmailType emailTypeToBeSent, string subject, string message)
        {
            To.Add(recipient);
            EmailToBeSent = emailTypeToBeSent;
            Subject = subject;
            Message = message;
        }

        public void AddRecipient(string user)
        {
            To.Add(user);
        }

        public void SendEmail()
        {
            SendGrid msg = SendGrid.GetInstance();

            foreach (string user in To)
            {
                if (Helper.IsValidEmail(user))
                    msg.AddTo(user);
            }
            
            msg.From = new MailAddress(fromEmail, fromName);
            msg.Subject = Subject;
            
            switch (EmailToBeSent)//Populate Email Text
            {
                case EmailType.AccountValueStatusDailyEmail:
                    msg.Html = BuildEmailContent(Message, Subject, "Emails/AccountValueStatusDailyEmail.htm");
                    break;
                case EmailType.NoHTML:
                    msg.Text = Message;
                    break;
            }
            
            Web.GetInstance(new NetworkCredential(sendgridUsername, sendgridPassword)).Deliver(msg);
        }

        private string BuildEmailContent(string message, string subject, string emailLocation)
        {
            return new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + emailLocation)).ReadToEnd().Replace("[SUBJECT]", subject).Replace("[MESSAGE]", message);
        }
    }
}
