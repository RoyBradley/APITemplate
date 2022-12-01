using Microsoft.EntityFrameworkCore;

using Persistence.AppContext;


namespace Api.Exceptions;


public static class AddSqlContext
{
	public static void EnableSqlContext(this IServiceCollection services, string connection) =>
		services.AddDbContext<ApiDbContext>(options => {
			options.UseSqlServer(connection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
			options.EnableDetailedErrors();
		});
}
