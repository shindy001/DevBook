using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;

namespace DevBook.Server.Features.StartupProfiles;

public sealed record DeleteStartupProfileCommand(string Id) : ICommand;

public sealed class DeleteStartupProfileCommandValidator : AbstractValidator<DeleteStartupProfileCommand>
{
	public DeleteStartupProfileCommandValidator()
	{
		RuleFor(x => x.Id).IsValidId();
	}
}

internal sealed class DeleteStartupProfileCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<DeleteStartupProfileCommand>
{
	public async Task Handle(DeleteStartupProfileCommand request, CancellationToken cancellationToken)
	{
		StartupProfile? startupProfile = null;
		if (Guid.TryParse(request.Id, out var guid))
		{
			startupProfile = await _dbContext.StartupProfiles.FindAsync([guid], cancellationToken: cancellationToken);
		}

		if (startupProfile is not null)
		{
			_dbContext.StartupProfiles.Remove(startupProfile);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
