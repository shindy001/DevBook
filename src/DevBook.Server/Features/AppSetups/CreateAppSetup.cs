using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.AppSetups;

internal record CreateAppSetup(
	string Name,
	string Path,
	string? Arguments)
	: ICommand;

internal class CreateAppSetupHandler(DevBookDbContext _dbContext) : ICommandHandler<CreateAppSetup>
{
	public async Task Handle(CreateAppSetup request, CancellationToken cancellationToken)
	{
		var newItem = new AppSetup(request.Name, request.Path, request.Arguments);
		await _dbContext.AppSetups.AddAsync(newItem, cancellationToken: cancellationToken);
	}
}
