using Microsoft.EntityFrameworkCore;

using Persistence.AppContext;


namespace Api.Extensions;


public static class AddSqlContext
{
	public static void EnableSqlContext(this IServiceCollection services, string connection) =>
		_ = services.AddDbContext<ApiDbContext>(options => {
			_ = options.UseSqlServer(connection, builder => {
				builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
			});
			_ = options.EnableDetailedErrors()
		});
}
