using Microsoft.AspNetCore.ResponseCompression;

using System.IO.Compression;


namespace Api.Exceptions;


public static class ResponseCompress
{
	public static void EnableResponseCompress(this IServiceCollection services) {
		services.AddResponseCompression(options => {
			options.MimeTypes =
				ResponseCompressionDefaults.MimeTypes.Concat(new[] {
					"application/xhtml+xml", "application/atom+xml", "image/svg+xml"
				});

			options.EnableForHttps = true;
		});

		services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });

		services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
	}
}
