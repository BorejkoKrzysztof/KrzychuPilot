using FluentValidation;
using MediatR;

namespace KrzychuPilot.Application.Common.Models.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                                            where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                                .SelectMany(r => r.Errors)
                                .Where(f => f != null)
                                .ToList();

                if (failures.Any())
                {
                    var errorMessages = string.Join(", ", failures.Select(f => f.ErrorMessage));

                    if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(ResponseResult<>))
                    {
                        var resultType = typeof(TResponse).GetGenericArguments()[0];
                        var failureMethod = typeof(TResponse).MakeGenericType(resultType).GetMethod("Failure");
                        return (TResponse)failureMethod!.Invoke(null, new object[] { errorMessages })!;
                    }

                    if (typeof(TResponse) == typeof(ResponseResult))
                    {
                        return (TResponse)(object)ResponseResult.Failure(errorMessages);
                    }

                    throw new ValidationException(failures);
                }
            }


            return await next();
        }
    }
}
