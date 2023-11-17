using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Infrastructure;

internal sealed class DevBookDbContext : DbContext
{
	public DevBookDbContext(DbContextOptions<DevBookDbContext> options)
		: base(options)
	{ }
}
