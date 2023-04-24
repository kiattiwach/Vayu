using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using Vayu.Localization;
using Serilog;

namespace Vayu;

public class EmailService : ITransientDependency
{
    private readonly ISettingProvider _settingProvider;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;
    private readonly VayuSetting _vayuSetting;

    public EmailService(ISettingProvider settingProvider, IStringLocalizer<VayuResource> stringLocalize,
        IOptions<VayuSetting> options)
    {
        _settingProvider = settingProvider;
        _stringLocalize = stringLocalize;
        _vayuSetting = options.Value;
    }
    public async Task SendCreateStaffAsync(List<string> toList, string subject, string message)
    {
        await SendAsync(toList, subject, message, bccList:_vayuSetting.Bcc);
    }

    private async Task SendAsync(List<string> toList, string subject, string message, List<string> ccList=null, List<string> bccList=null)
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
        if (ccList != null && ccList.Any())
        {
            email.Cc.AddRange(ccList.Select(MailboxAddress.Parse));
        }

        if (bccList != null && bccList.Any())
        {
            email.Bcc.AddRange(bccList.Select(MailboxAddress.Parse));
        }
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = message };

        Log.Information(@$"From: {sender}
                                        To: {string.Join(",", toList)}
                                        bcc: {string.Join(",", bccList ?? new List<string>())}
                                        subject: {subject}
                                        message: {message}");

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, enableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendCreateUpdateTaskAsync(List<string> toList, List<string> ccList, string subject, string message)
    {
        await SendAsync(toList, subject, message, ccList, bccList:_vayuSetting.Bcc);
    }
}