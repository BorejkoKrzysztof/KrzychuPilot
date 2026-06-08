using FluentValidation;

namespace KrzychuPilot.Application.Prompts.Commands.CreatePrompt
{
    public class CreatePromptGroupValidator : AbstractValidator<CreatePromptGroupCommand>
    {
        public CreatePromptGroupValidator()
        {
            RuleFor(x => x.contents).NotNull().WithMessage("Prompt List cannot be empty.");

            RuleFor(x => x.contents).Must(c => c != null && c.Any()).WithMessage("At least one prompt required.");

            RuleFor(x => x.contents).ForEach(str => str.MinimumLength(2)).WithMessage("Each prompt must be at least 2 characters long.");
        }
    }
}
