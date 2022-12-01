using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;


namespace Api.Exceptions;


public static class SerilogConfig
{
	public static void SerilogConfiguration(this ConfigureHostBuilder host, ConfigurationManager configuration) =>
		host.UseSerilog((ctx, cfg) => {
			cfg.Enrich.FromLogContext()
				.Enrich.WithMachineName()
				.Enrich.WithProcessId()
				.Enrich.WithThreadId()
				.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
				.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);

			if (ctx.HostingEnvironment.IsDevelopment()) {
				cfg.WriteTo.Async(c => c.Console(new RenderedCompactJsonFormatter()));
				cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
				cfg.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Debug);
			}
			else {
				cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

				cfg.WriteTo.Async(f => f.File(configuration.GetSection("LogPath").ToString() ?? string.Empty,
					rollingInterval: RollingInterval.Day,
					fileSizeLimitBytes: 40000000,
					shared: true,
					retainedFileCountLimit: 14,
					rollOnFileSizeLimit: true));
			}
		});
}
