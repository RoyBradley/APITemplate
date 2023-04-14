using System.Reflection;

using Microsoft.Extensions.DependencyInjection;


namespace Application.ServiceRegistration;


public static class ServiceExtensions
{
	public static void AddApplicationLayer(this IServiceCollection services) =>
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
}
