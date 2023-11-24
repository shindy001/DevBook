using DevBook.Server.Common;
using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
		RuleFor(x => x.AppSetupIds).Must(x => x.All(x => Guid.TryParse(x, out _)));
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

			var update = new StartupProfile(
				request.Name ?? existingItem.Name,
				request.AppSetupIds.Length != 0
					? await _dbContext.AppSetups
						.Where(x => guids.Contains(x.Id))
						.Select(x => x.Id)
						.ToArrayAsync(cancellationToken: cancellationToken)
					: existingItem.AppSetupIds);

			_dbContext.StartupProfiles.Entry(existingItem).CurrentValues.SetValues(new { update.Name, update.AppSetupIds });
			await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
