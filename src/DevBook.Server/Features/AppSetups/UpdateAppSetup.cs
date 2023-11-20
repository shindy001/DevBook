using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

internal record UpdateAppSetup(
	string Id,
	string Name,
	string Path,
	string? Arguments)
	: ICommand<OneOf<Success, NotFound>>;

internal class UpdateAppSetupHandler(DevBookDbContext _dbContext) : ICommandHandler<UpdateAppSetup, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateAppSetup request, CancellationToken cancellationToken)
	{
		var existingItem = await _dbContext.AppSetups.FindAsync(Guid.Parse(request.Id));
		if (existingItem is not null)
		{
			_dbContext.AppSetups.Entry(existingItem).CurrentValues.SetValues(new { request.Name, request.Path, request.Arguments });
			await _dbContext.SaveChangesAsync();
			return new Success();
		}
		else
		{
			return new NotFound();
		}
	}
}
