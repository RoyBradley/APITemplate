using Microsoft.EntityFrameworkCore;


namespace Persistence.AppContext;


public class ApiDbContext : Microsoft.EntityFrameworkCore.DbContext
{
	public ApiDbContext(DbContextOptions options) : base(options) { }

	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default) =>
		await SaveChangesAsync(cancellationToken) > 0;

	//	DeSets Here

	protected override void OnConfiguring(DbContextOptionsBuilder options) {
		string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.ToUpper();

		// See Microsoft Doc "DbContextOptionsBuilder.EnableSensitiveDataLogging Method"
		if (!string.IsNullOrEmpty(environment) && "DEVELOPMENT".Equals(environment)) {
			options.EnableSensitiveDataLogging();
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
