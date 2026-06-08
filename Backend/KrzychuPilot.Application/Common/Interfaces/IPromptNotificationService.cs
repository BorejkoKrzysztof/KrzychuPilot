namespace KrzychuPilot.Application.Common.Interfaces
{
    public interface IPromptNotificationService
    {
        Task NotifyStatusChanged(Guid taskId, string status, string? result = null);
    }
}
