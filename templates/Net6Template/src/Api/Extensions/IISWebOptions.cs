namespace Api.Extensions;


public static class IISWebOptions
{
	public static void ConfigureIISServerOptions(this IServiceCollection services) =>
		services.Configure<IISServerOptions>(options => {
			// Since Core 3 this is disabled by default
			options.AllowSynchronousIO = true;
		});
}
