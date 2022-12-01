using Microsoft.OpenApi.Models;


namespace Api.Exceptions;


public static class SwaggerConfig
{
	public static void SwaggerServices(this IServiceCollection services) => services.AddSwaggerGen(options => {
		options.SwaggerDoc("v1",
			new OpenApiInfo {
				Title = ".NET 6 CA Web API",
				Version = "v1",
				Contact = new OpenApiContact { Name = "", Email = "", Url = null },
				Description = "Docs for .NET 6 Web API"
			});

		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
			// Set up Swagger to accept JWT tokens
			In = ParameterLocation.Header,
			Description =
				$"JWT Authorization header using the Bearer scheme.{Environment.NewLine}" +
				"Enter 'Bearer' [space] and then your token in the text input below." +
				$"{Environment.NewLine}Example: 'Bearer Token'{Environment.NewLine}",
			Name = "Authorization",
			Type = SecuritySchemeType.ApiKey
		});

		options.AddSecurityRequirement(new OpenApiSecurityRequirement {
			{
				new OpenApiSecurityScheme {
					Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
				},
				Array.Empty<string>()
			}
		});

		options.CustomSchemaIds(type => type.ToString());
	});
}
