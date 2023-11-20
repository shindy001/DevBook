using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.AppSetups;

internal record DeleteAppSetup(string Id) : ICommand;

internal class DeleteAppSetupHandler(DevBookDbContext _dbContext) : ICommandHandler<DeleteAppSetup>
{
	public async Task Handle(DeleteAppSetup request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync(Guid.Parse(request.Id));
		if (existingItem is not null)
		{
			_dbContext.AppSetups.Remove(existingItem);
			await _dbContext.SaveChangesAsync();
		}
	}
}
