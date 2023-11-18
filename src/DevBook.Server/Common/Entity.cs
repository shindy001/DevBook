namespace DevBook.Server.Common;

internal abstract record Entity
{
	public Guid Id { get; init; }

	protected Entity(Guid id)
	{
		Id = id;
	}
}
