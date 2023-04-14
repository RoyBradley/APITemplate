using Microsoft.EntityFrameworkCore;


namespace Persistence.AppContext;


public class ApiDbContext : DbContext
{
	public ApiDbContext(DbContextOptions options) : base(options) { }

	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default) =>
		await SaveChangesAsync(cancellationToken) > 0;

	//	DeSets Here

	protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
