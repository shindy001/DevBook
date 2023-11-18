using DevBook.Server.Features.AppSetups;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Infrastructure;

internal sealed class DevBookDbContext(DbContextOptions<DevBookDbContext> _options) : DbContext(_options)
{
	public DbSet<AppSetup> AppSetups { get; set; }
}
