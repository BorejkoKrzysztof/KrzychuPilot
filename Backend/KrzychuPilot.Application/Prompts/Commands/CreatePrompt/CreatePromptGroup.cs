using KrzychuPilot.Application.Common.Interfaces;
using KrzychuPilot.Application.Common.Models;
using KrzychuPilot.Domain.Entities;
using MediatR;

namespace KrzychuPilot.Application.Prompts.Commands.CreatePrompt
{
    public record CreatePromptGroupCommand(IEnumerable<string> contents) : IRequest<ResponseResult<IEnumerable<Guid>>>;

    public class CreatePromptGroupCommandHandler : IRequestHandler<CreatePromptGroupCommand, ResponseResult<IEnumerable<Guid>>>
    {
        private readonly IApplicationDbContext _dbContext;
        public CreatePromptGroupCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;


        public async Task<ResponseResult<IEnumerable<Guid>>> Handle(CreatePromptGroupCommand request, CancellationToken cancellationToken)
        {
            var addedIds = new List<Guid>();

            foreach (var content in request.contents)
            {
                var task = new PromptTask
                {
                    Id = Guid.NewGuid(),
                    Content = content,
                    Status = PromptStatus.Awaiting,
                };

                _dbContext.PromptTasks.Add(task);
                addedIds.Add(task.Id);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResponseResult<IEnumerable<Guid>>.Success(addedIds);
        }
    }
}
