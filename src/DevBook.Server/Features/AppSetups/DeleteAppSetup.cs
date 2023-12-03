using DevBook.Server.Infrastructure;
using DevBook.Server.Validation;
using DevBook.Shared.Contracts;
using FluentValidation;

namespace DevBook.Server.Features.AppSetups;

public sealed record DeleteAppSetupCommand(string Id) : ICommand;

public sealed class DeleteAppSetupCommandValidator : AbstractValidator<DeleteAppSetupCommand>
{
	public DeleteAppSetupCommandValidator() => RuleFor(x => x.Id).IsValidId();
}

internal sealed class DeleteAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<DeleteAppSetupCommand>
{
	public async Task Handle(DeleteAppSetupCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);
		if (existingItem is not null)
		{
			_dbContext.AppSetups.Remove(existingItem);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
