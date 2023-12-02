using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.StartupProfiles;

public sealed record UpdateStartupProfileCommand(
	string Id,
	string Name,
	string[] AppSetupIds)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class UpdateStartupProfileCommandValidator : AbstractValidator<UpdateStartupProfileCommand>
{
	public UpdateStartupProfileCommandValidator()
	{
		RuleFor(x => x.Id).IsValidId();
		RuleFor(x => x.Name).NotEmpty();
		RuleForEach(x => x.AppSetupIds).IsValidId();
	}
}

internal sealed class UpdateStartupProfileCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<UpdateStartupProfileCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateStartupProfileCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.StartupProfiles.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);
		if (existingItem is not null)
		{
			var guids = request.AppSetupIds.Select(Guid.Parse);
			var appSetupIds = await _dbContext.AppSetups
				.Where(x => guids.Contains(x.Id))
				.Select(x => x.Id)
				.ToArrayAsync(cancellationToken: cancellationToken);

			var update = new Dictionary<string, object>
			{
				[nameof(StartupProfile.Name)] = request.Name,
				[nameof(StartupProfile.AppSetupIds)] = request.AppSetupIds.Length != 0
					? await _dbContext.GetExistingAppSetupGuids(guids, cancellationToken)
					: request.AppSetupIds
			};
			
			_dbContext.StartupProfiles.Entry(existingItem).CurrentValues.SetValues(update);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
