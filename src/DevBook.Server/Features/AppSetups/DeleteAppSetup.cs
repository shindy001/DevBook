using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.AppSetups;

internal sealed record DeleteAppSetupCommand(string Id) : ICommand;

internal sealed class DeleteAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<DeleteAppSetupCommand>
{
	public async Task Handle(DeleteAppSetupCommand request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync(Guid.Parse(request.Id));
		if (existingItem is not null)
		{
			_dbContext.AppSetups.Remove(existingItem);
			await _dbContext.SaveChangesAsync();
		}
	}
}
