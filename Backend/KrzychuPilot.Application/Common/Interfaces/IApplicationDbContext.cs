using KrzychuPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KrzychuPilot.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PromptTask> PromptTasks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
