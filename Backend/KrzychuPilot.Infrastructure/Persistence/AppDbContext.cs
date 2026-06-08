using KrzychuPilot.Application.Common.Interfaces;
using KrzychuPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KrzychuPilot.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<PromptTask> PromptTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PromptTask>().Property(p => p.Status).HasConversion<string>();
        }
    }
}
