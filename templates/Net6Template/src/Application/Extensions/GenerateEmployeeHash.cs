using System.Globalization;
using System.Security.Cryptography;
using System.Text;


namespace Application.Extensions;


/// <summary>
///		GenerateEmployeeHash
///		This function create the employee hash equivalent to 
///		NG's Employee hash that is generated in the Oracle Database
///		Generate employee has from string containing employee's
///		ssn + "|" + birthday
///		This string is known a employeeInfo and passed to this function
///		-----------------------------------------------------------------------
///		The ssn is number only, remove dashes
///		The birthday is in the format "dd-MMM-yy"
///		Example: "368900000|22-FEB-50"
/// </summary>
public static class GenerateEmployeeHash
{
	public static string GetEmployeeHash(this string employeeInfo) {
		// Creates an instance of the default implementation of SHA256   
		using SHA256 sha256Hash = SHA256.Create();

		// Computes the SHA256 hash for the input data - returns byte array  
		byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(employeeInfo));

		// Convert the returned byte array to a string   
		StringBuilder builder = new();

		foreach (byte hashByte in hashBytes) {
			_ = builder.Append(hashByte.ToString("x2", CultureInfo.CurrentCulture));
		}

		// Return the employee hash
		return builder.ToString().ToUpper(CultureInfo.CurrentCulture);
	}
}
