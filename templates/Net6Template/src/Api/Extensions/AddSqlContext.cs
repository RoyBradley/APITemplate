using Microsoft.EntityFrameworkCore;

using Persistence.AppContext;


namespace Api.Extensions;


public static class AddSqlContext
{
	public static void EnableSqlContext(this IServiceCollection services, string connection) =>
		services.AddDbContext<ApiDbContext>(options => {
			_ = options.UseSqlServer(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
			_ = options.EnableDetailedErrors();
		});
}
