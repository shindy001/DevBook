using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

public sealed record UpdateAppSetupCommand(
	string Id,
	string Name,
	string Path,
	string? Arguments)
	: ICommand<OneOf<Success, NotFound>>;

public sealed class UpdateAppSetupCommandValidator : AbstractValidator<UpdateAppSetupCommand>
{
	public UpdateAppSetupCommandValidator()
	{
		RuleFor(x => x.Id).Must(IsValidId).WithMessage("Invalid Id value '{PropertyValue}'");
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Path).NotEmpty();
	}

	private static bool IsValidId(string id)
	{
		return Guid.TryParse(id, out _);
	}
}

internal sealed class UpdateAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<UpdateAppSetupCommand, OneOf<Success, NotFound>>
{
	public async Task<OneOf<Success, NotFound>> Handle(UpdateAppSetupCommand request, CancellationToken cancellationToken)
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
