using FluentValidation;

namespace DevBook.Server.Validation;

public static class IdRuleBuilderOptions
{
	public static IRuleBuilderOptions<T, TElement> IsValidId<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
	{
		return ruleBuilder.Must(x => Guid.TryParse(x?.ToString(), out _)).WithMessage("Invalid Id value {PropertyValue}.");
	}
}
