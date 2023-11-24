using DevBook.Server.Common;
using DevBook.Server.Infrastructure;
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
		RuleFor(x => x.AppSetupIds).Must(x => x.All(x => Guid.TryParse(x, out _)));
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

			var update = new StartupProfile(request.Name, appSetupIds);
			
			_dbContext.StartupProfiles.Entry(existingItem).CurrentValues.SetValues(new { update.Name, update.AppSetupIds });
			await _dbContext.SaveChangesAsync(cancellationToken);
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
