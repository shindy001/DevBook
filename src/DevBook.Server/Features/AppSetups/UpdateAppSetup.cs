using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

public sealed record UpdateAppSetupCommand(
	string Id,
	string Name,
	string Path,
	string? Arguments)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class UpdateAppSetupCommandValidator : AbstractValidator<UpdateAppSetupCommand>
{
	public UpdateAppSetupCommandValidator()
	{
		RuleFor(x => x.Id).IsValidId();
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Path).NotEmpty();
	}
}

internal sealed class UpdateAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<UpdateAppSetupCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateAppSetupCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);
		if (existingItem is not null)
		{
			var update = new Dictionary<string, object?> 
			{
				[nameof(AppSetup.Name)] = request.Name,
				[nameof(AppSetup.Path)] = request.Path,
				[nameof(AppSetup.Arguments)] = request?.Arguments
			};

			_dbContext.AppSetups.Entry(existingItem).CurrentValues.SetValues(update);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
