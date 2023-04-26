using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Identity;

namespace Vayu;

//[Authorize]
[HubRoute("MessagingHub")]
public class MessagingHub : AbpHub
{
    private readonly IHubContext<MessagingHub> _hubContext;

    public MessagingHub(IHubContext<MessagingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendReloadTask(Guid taskId, Guid senderId)
    {
        await _hubContext.Clients.All.SendAsync("ReloadTaskList", taskId, senderId);
    }

    public async Task SendTest(Guid taskId, Guid senderId)
    {
        await _hubContext.Clients.All.SendAsync("TestSignalR", taskId, senderId);
    }
}