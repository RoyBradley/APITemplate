using System.Security.Claims;


namespace Application.Extensions;


public static class ClaimsPrincipalExtensions
{
	public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal) {
		if (principal == null) {
			throw new ArgumentNullException(nameof(principal));
		}

		Claim loggedInUserId = principal.FindFirst(ClaimTypes.NameIdentifier);

		if (typeof(T) == typeof(string)) {
			return (T)Convert.ChangeType(loggedInUserId, typeof(T));
		}

		if (typeof(T) == typeof(int) || typeof(T) == typeof(long)) {
			return loggedInUserId != null
				? (T)Convert.ChangeType(loggedInUserId, typeof(T))
				: (T)Convert.ChangeType(0, typeof(T));
		}

		throw new Exception("Invalid type provided");
	}

	public static string GetLoggedInUserName(this ClaimsPrincipal principal) {
		if (principal == null) {
			throw new ArgumentNullException(nameof(principal));
		}

		return principal.FindFirst(ClaimTypes.Name)?.ToString();
	}

	public static string GetLoggedInUserEmail(this ClaimsPrincipal principal) {
		if (principal == null) {
			throw new ArgumentNullException(nameof(principal));
		}

		return principal.FindFirst(ClaimTypes.Email)?.ToString();
	}
}
