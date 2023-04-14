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

	public async Task Invoke(HttpContext context) {
		try {
			await _next(context);
		}
		catch (Exception error) {
			HttpResponse response = context.Response;

			response.StatusCode = error switch {
				ValidationException _ =>
					// Validation Exception
					(int)HttpStatusCode.BadRequest,
				ConflictException _ =>
					// Conflict 409
					(int)HttpStatusCode.Conflict,
				NoContentException _ =>
					// Not Content 204
					(int)HttpStatusCode.NoContent,
				NotFoundException _ =>
					// Not found 404
					(int)HttpStatusCode.NotFound,
				UnauthorizedException _ =>
					// bad credentials 401
					(int)HttpStatusCode.Unauthorized,
				AppException _ =>
					// custom application error 400
					(int)HttpStatusCode.BadRequest,
				ForbiddenException _ =>
					// Forbidden 403
					(int)HttpStatusCode.Forbidden,
				ServiceUnavailableException _ =>
					// Service Note available 503
					(int)HttpStatusCode.ServiceUnavailable,
				_ =>
					(int)HttpStatusCode.InternalServerError
			};

			string result = !error.Message[0].Equals('{')
				? JsonConvert.SerializeObject(new { message = error.Message })
				: error.Message;

			response.ContentType = "application/json";

			_logger.LogError("#### Exception: {@statusCode}{@newline}{@errorMessage}",
				response.StatusCode, Environment.NewLine, error.Message);

			await response!.WriteAsync(result).ConfigureAwait(false);
		}
	}
}
