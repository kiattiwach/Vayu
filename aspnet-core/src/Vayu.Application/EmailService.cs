using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Vayu;

public class EmailService : ITransientDependency
{
    public async Task SendAsync(MailMessage mail)
    {
        SmtpClient smtpClient = new SmtpClient
        {
            Credentials = new NetworkCredential("kiattiwat.katanyuchanon@atom-outsource.co.th", "naotoe9*"),
            Host = "smtp.office365.com",
            Port = 587,
            EnableSsl = true
        };
        await smtpClient.SendMailAsync(mail);
    }
}