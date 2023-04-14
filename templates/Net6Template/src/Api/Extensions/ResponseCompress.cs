using System.IO.Compression;

using Microsoft.AspNetCore.ResponseCompression;


namespace Api.Extensions;


public static class ResponseCompress
{
	public static void EnableResponseCompress(this IServiceCollection services) {
		_ = services.AddResponseCompression(options => {
			options.MimeTypes =
				ResponseCompressionDefaults.MimeTypes.Concat(new[] {
					"application/xhtml+xml", "application/atom+xml", "image/svg+xml"
				});

			options.EnableForHttps = true;
		});

		_ = services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });

		_ = services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
	}
}
