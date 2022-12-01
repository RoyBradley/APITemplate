using System.Globalization;


namespace Domain.Exceptions;

public class ForbiddenException : Exception
{
	public ForbiddenException() { }
	public ForbiddenException(string message) : base(message) { }

	public ForbiddenException(string message, params object[] args)
		: base(string.Format(CultureInfo.CurrentCulture, message, args)) { }

	public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}

