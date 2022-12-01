using System.Globalization;


namespace Domain.Exceptions;

public class ServiceUnavailableException : Exception
{
	public ServiceUnavailableException() { }
	public ServiceUnavailableException(string message) : base(message) { }

	public ServiceUnavailableException(string message, params object[] args)
		: base(string.Format(CultureInfo.CurrentCulture, message, args)) { }

	public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException) { }
}

