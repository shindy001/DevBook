using System.Collections.Immutable;

namespace DevBook.Server.Exceptions;

internal class CommandValidationException : Exception
{
	public IDictionary<string, string> Errors { get; private set; }

	public CommandValidationException(string message) : this(message, ImmutableDictionary<string, string>.Empty)
	{

	}

	public CommandValidationException(IDictionary<string, string> errors) : base("Validation Errors")
	{
		Errors = errors;
	}

	public CommandValidationException(string message, IDictionary<string, string> errors) : base(message)
	{
		Errors = errors;
	}
}
