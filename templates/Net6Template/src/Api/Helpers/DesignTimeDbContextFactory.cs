using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Persistence.AppContext;


namespace Api.Helpers;


public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
	public ApiDbContext CreateDbContext(string[] args) {
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("connections.json")
			.AddJsonFile("connections.Development.json")
			.Build();

		DbContextOptionsBuilder<ApiDbContext> builder = new();
		string connectionString = configuration.GetConnectionString("ApplicationCS");
		Console.WriteLine($"Connection String: {connectionString}");
		_ = builder.UseSqlServer(connectionString);

		return new ApiDbContext(builder.Options);
	}
}
