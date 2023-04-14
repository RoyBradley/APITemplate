namespace Api.Extensions;


public static class Cors
{
	public static void ConfigureCors(this IServiceCollection services) =>
		// These are the basic settings for CORS policy this project will allow any origin, method, and header
		services.AddCors(options => {
			options.AddPolicy("CorsPolicy",
				builder => builder
					.SetIsOriginAllowed(_ => true)
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials());
		});
}
