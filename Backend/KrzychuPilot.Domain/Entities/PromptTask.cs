namespace KrzychuPilot.Domain.Entities
{
    public enum PromptStatus { Awaiting, Processing, Finished, Failed }
    public class PromptTask
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public PromptStatus Status { get; set; }
        public string? Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
    }
}
