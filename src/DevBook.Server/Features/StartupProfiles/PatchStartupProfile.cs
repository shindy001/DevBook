using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.StartupProfiles;

public sealed record PatchStartupProfileCommand(
	string Id,
	string? Name,
	string[] AppSetupIds)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class PatchStartupProfileCommandValidator : AbstractValidator<PatchStartupProfileCommand>
{
	public PatchStartupProfileCommandValidator()
	{
		RuleFor(x => x.Id).IsValidId();
		RuleForEach(x => x.AppSetupIds).IsValidId();
	}
}

internal sealed class PatchStartupProfileCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<PatchStartupProfileCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(PatchStartupProfileCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.StartupProfiles.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);
		if (existingItem is not null)
		{
			var guids = request.AppSetupIds.Select(Guid.Parse);
			var update = new Dictionary<string, object>
			{
				[nameof(StartupProfile.Name)] = request.Name ?? existingItem.Name,
				[nameof(StartupProfile.AppSetupIds)] = request.AppSetupIds.Length != 0
					? await _dbContext.GetExistingAppSetupGuids(guids, cancellationToken)
					: existingItem.AppSetupIds
			};

			_dbContext.StartupProfiles.Entry(existingItem).CurrentValues.SetValues(update);
			await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
