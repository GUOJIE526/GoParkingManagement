using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;


namespace MyGoParking
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();
            var mail = new MailMessage();
            mail.From = new MailAddress("jiunyi2281@gmail.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.UseDefaultCredentials =false;
            client.Credentials = new NetworkCredential("jiunyi2281@gmail.com", "jjra vieg whhf aomh");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}
