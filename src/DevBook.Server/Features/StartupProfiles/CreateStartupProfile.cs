using DevBook.Server.Common;
using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Features.StartupProfiles;

public sealed record CreateStartupProfileCommand(
	string Name,
	string[] AppSetupIds)
	: ICommand;

public sealed class CreateStartupProfileCommandValidator : AbstractValidator<CreateStartupProfileCommand>
{
	public CreateStartupProfileCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleForEach(x => x.AppSetupIds).IsValidId();
	}
}

internal sealed class CreateStartupProfileCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<CreateStartupProfileCommand>
{
	public async Task Handle(CreateStartupProfileCommand request, CancellationToken cancellationToken)
	{
		var guids = request.AppSetupIds.Select(Guid.Parse);

		var appSetupIds = await _dbContext.AppSetups
			.Where(x => guids.Contains(x.Id))
			.Select(x => x.Id)
			.ToArrayAsync(cancellationToken: cancellationToken);

		var newItem = new StartupProfile(request.Name, appSetupIds);
		await _dbContext.StartupProfiles.AddAsync(newItem, cancellationToken: cancellationToken);
	}
}
