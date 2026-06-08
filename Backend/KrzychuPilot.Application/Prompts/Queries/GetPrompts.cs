using KrzychuPilot.Application.Common.Interfaces;
using KrzychuPilot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KrzychuPilot.Application.Prompts.Queries
{
    public record GetPromptsQuery : IRequest<ResponseResult<IEnumerable<PromptTaskDto>>>;
    public record PromptTaskDto(Guid Id, string Content, string Status, string? Result, DateTime CreatedAt);

    public class GetPromptsQueryHandler : IRequestHandler<GetPromptsQuery, ResponseResult<IEnumerable<PromptTaskDto>>>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetPromptsQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<ResponseResult<IEnumerable<PromptTaskDto>>> Handle(GetPromptsQuery request, CancellationToken cancellationToken)
        { 
            var list = await _dbContext.PromptTasks
                        .OrderBy(t => t.CreatedAt)
                        .Select(t => new PromptTaskDto(t.Id, t.Content, t.Status.ToString(), t.Result, t.CreatedAt))
                        .ToListAsync(cancellationToken);



            return ResponseResult<IEnumerable<PromptTaskDto>>.Success(list);
        }
    }
}
