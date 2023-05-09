using System.ComponentModel.DataAnnotations;
using System.Net;

using Domain.Exceptions;

using Newtonsoft.Json;


namespace Api.Middleware;


public class ErrorHandlerMiddleware
{
	private readonly ILogger _logger;
	private readonly RequestDelegate _next;

	public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) {
		_next = next;
		_logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
	}

	public async Task Invoke(HttpContext context, string message) {
		try {
			await _next(context);
		}
		catch (Exception error) {
			HttpResponse response = context.Response;

			response.StatusCode = error switch {
				ValidationException =>
					// Validation Exception
					(int)HttpStatusCode.BadRequest,
				ConflictException =>
					// Conflict 409
					(int)HttpStatusCode.Conflict,
				NoContentException =>
					// Not Content 204
					(int)HttpStatusCode.NoContent,
				NotFoundException =>
					// Not found 404
					(int)HttpStatusCode.NotFound,
				UnauthorizedException =>
					// bad credentials 401
					(int)HttpStatusCode.Unauthorized,
				AppException =>
					// custom application error 400
					(int)HttpStatusCode.BadRequest,
				ForbiddenException =>
					// Forbidden 403
					(int)HttpStatusCode.Forbidden,
				ServiceUnavailableException =>
					// Service Note available 503
					(int)HttpStatusCode.ServiceUnavailable,
				_ =>
					(int)HttpStatusCode.InternalServerError
			};

			string result = !error.Message[0].Equals('{')
				? JsonConvert.SerializeObject(new { message = error.Message })
				: error.Message;

			response.ContentType = "application/json";

			await response!.WriteAsync(result).ConfigureAwait(false);
		}
	}
}
