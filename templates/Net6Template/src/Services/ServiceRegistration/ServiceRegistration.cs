using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NETCore.MailKit.Core;


namespace Services.ServiceRegistration;


public static class ServiceRegistration
{
	public static void AddServiceInfrastructure(this IServiceCollection services, IConfiguration configuration) {
		services.AddScoped<IEmailService, EmailService>();
	}
}
