using DevBook.Shared.Contracts;
using FluentValidation;
using MediatR;
using System.Collections.Immutable;

namespace DevBook.Server.Common;

internal sealed class CommandValidationPipelineBehavior<TRequest, TResponse>
	: ICommandPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommandBase
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public CommandValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
	
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);
		var errorsDictionary = _validators
			.Select(x => x.Validate(context))
			.SelectMany(x => x.Errors)
			.Where(x => x != null)
			.GroupBy(
				x => x.PropertyName,
				x => x.ErrorMessage,
				(propertyName, errorMessages) => new
				{
					Key = propertyName,
					Values = errorMessages.Distinct()
				})
			.ToDictionary(x => x.Key, x => string.Join(" ", x.Values));
		if (errorsDictionary.Count != 0)
		{
			throw new CommandValidationException(errorsDictionary);
		}
		return await next();
	}
}

internal class CommandValidationException : Exception
{
	public IDictionary<string, string> Errors { get; private set; }

	public CommandValidationException(string message) : this(message, ImmutableDictionary<string, string>.Empty)
	{

	}

	public CommandValidationException(IDictionary<string, string> errors) : base("Validation Errors")
	{
		Errors = errors;
	}

	public CommandValidationException(string message, IDictionary<string, string> errors) : base(message)
	{
		Errors = errors;
	}
}
