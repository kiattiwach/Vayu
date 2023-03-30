using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using MimeKit.Text;
using MimeKit;
using Vayu.Localization;

namespace Vayu;

public class EmailService : ITransientDependency
{
    private readonly ISettingProvider _settingProvider;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;

    public EmailService(ISettingProvider settingProvider, IStringLocalizer<VayuResource> stringLocalize)
    {
        _settingProvider = settingProvider;
        _stringLocalize = stringLocalize;
    }
    public async Task SendAsync(List<string> toList, string subject, string message)
    {
        var sender = await _settingProvider.GetOrNullAsync("Abp.Mailing.DefaultFromAddress");
        var senderName = await _settingProvider.GetOrNullAsync("Abp.Mailing.DefaultFromDisplayName");
        var userName = await _settingProvider.GetOrNullAsync("Abp.Mailing.Smtp.UserName");
        var password = await _settingProvider.GetOrNullAsync("Abp.Mailing.Smtp.Password");
        var host = await _settingProvider.GetOrNullAsync("Abp.Mailing.Smtp.Host");
        var port = Convert.ToInt16(await _settingProvider.GetOrNullAsync("Abp.Mailing.Smtp.Port"));
        var enableSsl = bool.Parse(await _settingProvider.GetOrNullAsync("Abp.Mailing.Smtp.EnableSsl"));
        
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(sender);
        email.Sender.Name = _stringLocalize[senderName];
        email.From.Add(email.Sender);
        email.To.AddRange(toList.Select(MailboxAddress.Parse));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = message };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, enableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}