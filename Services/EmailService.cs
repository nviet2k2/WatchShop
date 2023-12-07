using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Helpter;

namespace Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }
        public async Task SendEmailAsync(Mailrequest mailrequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();


            byte[] fileBytes;
            if (System.IO.File.Exists("NgocViet/NgocViet.pdf"))
            {
                FileStream file = new FileStream("NgocViet/NgocViet.pdf", FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add("Test.pdf", fileBytes, ContentType.Parse("NgocViet/ChuyenDoi"));
                builder.Attachments.Add("Test1.pdf", fileBytes, ContentType.Parse("NgocViet/ChuyenDoi1"));
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}