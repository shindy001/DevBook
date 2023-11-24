using DevBook.Server.Common;
using DevBook.Server.Infrastructure;
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
		var existingItem = await _dbContext.AppSetups.FindAsync(Guid.Parse(request.Id));
		if (existingItem is not null)
		{
			var update = new AppSetup(
				request.Name ?? existingItem.Name,
				request.Path ?? existingItem.Path,
				request.Arguments ?? existingItem.Arguments);

			_dbContext.AppSetups.Entry(existingItem).CurrentValues.SetValues(new { update.Name, update.Path, update.Arguments });
			await _dbContext.SaveChangesAsync();
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
