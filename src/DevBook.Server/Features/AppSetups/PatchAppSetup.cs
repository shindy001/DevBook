using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

public sealed record PatchAppSetupCommand(
	string Id,
	string? Name,
	string? Path,
	string? Arguments)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class PatchAppSetupCommandValidator : AbstractValidator<PatchAppSetupCommand>
{
	public PatchAppSetupCommandValidator() => RuleFor(x => x.Id).IsValidId();
}

internal sealed class PatchAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<PatchAppSetupCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(PatchAppSetupCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);
		if (existingItem is not null)
		{
			var update = new Dictionary<string, object?>
			{
				[nameof(AppSetup.Name)] = request.Name ?? existingItem.Name,
				[nameof(AppSetup.Path)] = request.Path ?? existingItem.Path,
				[nameof(AppSetup.Arguments)] = request.Arguments ?? existingItem.Arguments
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
