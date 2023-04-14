using System.Security.Claims;

using Domain.Exceptions;

namespace Application.Extensions;


public static class ClaimsPrincipalExtensions
{
	public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal) {
		if (principal == null) {
			throw new ArgumentNullException(nameof(principal));
		}

		//Claim loggedInUserId = principal.FindFirst(ClaimTypes.NameIdentifier);

		//return typeof(T) == typeof(string)
		//	? (T)Convert.ChangeType(loggedInUserId, typeof(T))
		//	: typeof(T) == typeof(int) || typeof(T) == typeof(long)
		//		? loggedInUserId != null
		//			? (T)Convert.ChangeType(loggedInUserId, typeof(T))
		//			: (T)Convert.ChangeType(0, typeof(T))
		//		: throw new AppException("Invalid type provided");
		return typeof(T) == typeof(string)
			? principal.GetLoggedInUserId<T>()
			: typeof(T) == typeof(int) || typeof(T) == typeof(long)
				? principal.GetLoggedInUserId<T>()
				: throw new AppException("Invalid type provided");
	}

	public static string GetLoggedInUserName(this ClaimsPrincipal principal) =>
		principal == null
			? throw new ArgumentNullException(nameof(principal))
			: principal.FindFirst(ClaimTypes.Name)?.ToString();

	public static string GetLoggedInUserEmail(this ClaimsPrincipal principal) =>
		principal == null
			? throw new ArgumentNullException(nameof(principal))
			: principal.FindFirst(ClaimTypes.Email)?.ToString();
}
