using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using FluentValidation;

namespace DevBook.Server.Features.AppSetups;

public sealed record CreateAppSetupCommand(
	string Name,
	string Path,
	string? Arguments)
	: ICommand;

public sealed class CreateAppSetupCommandValidator : AbstractValidator<CreateAppSetupCommand>
{
	public CreateAppSetupCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Path).NotEmpty();
	}
}

internal sealed class CreateAppSetupCommandHandler(DevBookDbContext _dbContext) : ICommandHandler<CreateAppSetupCommand>
{
	public async Task Handle(CreateAppSetupCommand request, CancellationToken cancellationToken)
	{
		var newItem = new AppSetup(request.Name, request.Path, request.Arguments);
		await _dbContext.AppSetups.AddAsync(newItem, cancellationToken: cancellationToken);
	}
}
