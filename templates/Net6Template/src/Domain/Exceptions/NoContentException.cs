using System.Globalization;


namespace Domain.Exceptions;

public class NoContentException : Exception
{
	public NoContentException() { }
	public NoContentException(string message) : base(message) { }

	public NoContentException(string message, params object[] args)
		: base(string.Format(CultureInfo.CurrentCulture, message, args)) { }

	public NoContentException(string message, Exception innerException) : base(message, innerException) { }
}

