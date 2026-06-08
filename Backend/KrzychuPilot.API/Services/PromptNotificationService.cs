using KrzychuPilot.API.Hubs;
using KrzychuPilot.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KrzychuPilot.API.Services
{
    public class PromptNotificationService : IPromptNotificationService
    {
        private readonly IHubContext<PromptHub> _hubContext;
        public PromptNotificationService(IHubContext<PromptHub> hubContext) => _hubContext = hubContext;
        public async Task NotifyStatusChanged(Guid taskId, string status, string? result = null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", new { taskId, status, result });
        }
    }
}
