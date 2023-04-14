using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;


namespace Api.Extensions;


public static class SerilogConfig
{
	public static void SerilogConfiguration(this ConfigureHostBuilder host, ConfigurationManager configuration) =>
		host.UseSerilog((ctx, cfg) => {
			_ = cfg.Enrich.FromLogContext()
				.Enrich.WithMachineName()
				.Enrich.WithProcessId()
				.Enrich.WithThreadId()
				.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
				.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);

			if (ctx.HostingEnvironment.IsDevelopment()) {
				_ = cfg.WriteTo.Async(c => c.Console(new RenderedCompactJsonFormatter()));
				_ = cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
				_ = cfg.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Debug);
			}
			else {
				_ = cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

				_ = cfg.WriteTo.Async(f => f.File(configuration.GetSection("LogPath").ToString() ?? string.Empty,
					rollingInterval: RollingInterval.Day,
					fileSizeLimitBytes: 40000000,
					shared: true,
					retainedFileCountLimit: 14,
					rollOnFileSizeLimit: true));
			}
		});
}
