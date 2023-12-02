using DevBook.Server.Features.AppSetups;
using DevBook.Server.Features.StartupProfiles;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Infrastructure;

public sealed class DevBookDbContext(DbContextOptions<DevBookDbContext> _options) : DbContext(_options)
{
	public DbSet<AppSetup> AppSetups { get; set; }
	public DbSet<StartupProfile> StartupProfiles { get; set; }
}
