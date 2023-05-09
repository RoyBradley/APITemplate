using System.Globalization;

using Api.Extensions;
using Api.Middleware;

using Application.Configuration;
using Application.ServiceRegistration;

using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using Persistence.ServiceRegistration;

using Serilog;

using Services.ServiceRegistration;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//	Setup controller Configuration
builder.Services.AddControllers()
	.AddNewtonsoftJson(options => {
		//	Ignore self references
		options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		//	Ignore null values during serialization
		options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
		//	Format json text output indented
		options.SerializerSettings.Formatting = Formatting.Indented;
		//	Use a non-public default constructor before falling
		//	back to a parameterized constructor
		options.SerializerSettings.ConstructorHandling =
			ConstructorHandling.AllowNonPublicDefaultConstructor;
	});

//	Application Settings
builder.WebHost.UseSetting("detailedErrors", "true");
//	The content root determines where the host searches for content files
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
//	Allows host captures exceptions during startup and attempts to start the server
builder.WebHost.CaptureStartupErrors(true);
//	Sets the FileProvider for file-based providers to a PhysicalFileProvider with the base path
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
//	Load the environment variables
builder.Configuration.AddEnvironmentVariables();

//	Setup AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddOptions();


//	Setup Swagger
builder.Services.SwaggerServices();
builder.Services.AddSwaggerGenNewtonsoftSupport();

//	Setup Serilog Logging configuration
builder.Logging.ClearProviders();
builder.Host.SerilogConfiguration(builder.Configuration);

//	Load custom json files
builder.Configuration.AddJsonFile("connections.json", true, true);
builder.Configuration.AddJsonFile(
	$"connections.{builder.Environment.EnvironmentName}.json", true, true);

//	Get connection string
string connection = builder.Configuration.GetConnectionString("ApplicationCS");
builder.Services.EnableSqlContext(connection);

// Get Cache Time
int cacheTime = Convert.ToInt32(builder.Configuration.GetSection("CacheTime").Value, new NumberFormatInfo());

//	Response Caching
builder.Services.AddResponseCaching(options => {
	options.MaximumBodySize = 1024;
});

//	Setup Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//	Setup other configuration items
builder.Services.ConfigureCors();
builder.Services.EnableResponseCompress();
builder.Services.ConfigureIISServerOptions();

//	Setup project services
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure();
builder.Services.AddServiceInfrastructure();

//	------------------------------------------------
WebApplication app = builder.Build();

app.UseHsts();

app.UseRouting();

app.UseHttpsRedirection();

//	Add Serilog
app.UseSerilogRequestLogging();

//	Add Security Headers
HeaderPolicyCollection policyCollection = SecurityHeaderPolicy.PolicyCollection();
app.UseSecurityHeaders(policyCollection);

//	Add Response Compression
app.UseResponseCompression();

//	Add CORS
app.UseCors("CorsPolicy");

//	Add Caching
app.UseResponseCaching();

app.Use(async (context, next) => {
	context.Response.GetTypedHeaders().CacheControl =
		new CacheControlHeaderValue {
			Public = true,
			MaxAge = TimeSpan.FromSeconds(cacheTime)
		};

	context.Response.Headers[HeaderNames.Vary] = new[] {
		"Accept-Encoding"
	};

	await next();
});

//	Add Global error handler middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

//	Add Swagger
app.UseSwagger(options =>
	options.PreSerializeFilters.Add((swagger, httpReq) => {
		swagger.Servers = new List<OpenApiServer> {
			new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
		};
	}));

app.UseSwaggerUI(c => {
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Api");
});

//	Adds endpoints for controller actions to the IEndpointRouteBuilder
//	without specifying any routes.
app.MapControllers();

try {
	Log.Information("Starting Web Api :: {Time}", DateTime.UtcNow);
	app.Run();
	Log.CloseAndFlush();
}
catch (Exception ex) {
	Log.Fatal(ex, "Web Api Terminated Unexpectedly");
	Log.CloseAndFlush();
}
