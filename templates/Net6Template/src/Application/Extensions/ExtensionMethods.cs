using System.Globalization;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Application.Extensions;


public static class ExtensionMethods
{
	/// <summary>
	///		Determines whether this string is json string.
	/// </summary>
	/// <param name="str">The string to test.</param>
	/// <returns>
	///   <c>true</c> if the specified string is json; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsJson(this string str) {
		if (string.IsNullOrWhiteSpace(str)) {
			return false;
		}
		str = str.Trim();
		if ((str.StartsWith("{", StringComparison.CurrentCultureIgnoreCase) &&
				str.EndsWith("}", StringComparison.CurrentCultureIgnoreCase)) || //For object
				(str.StartsWith("[", StringComparison.CurrentCultureIgnoreCase) &&
					str.EndsWith("]", StringComparison.CurrentCultureIgnoreCase))) { //For array
			try {
				JToken obj = JToken.Parse(str);
				return true;
			}
			catch (JsonReaderException jex) {
				//Exception in parsing json
				Console.WriteLine(jex.Message);
				return false;
			}
			catch (Exception ex) { //some other exception
				Console.WriteLine(ex.ToString());
				return false;
			}
		}
		else {
			return false;
		}
	}

	/// <summary>Determines whether this instance of a list is empty.</summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">list.</param>
	/// <returns>
	///     <c>true</c> if the specified list is empty; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsEmpty<T>(this IEnumerable<T> list) => list.Any() != true;

	/// <summary>
	///     ClearFormat String
	///     Remove all characters that are not numbers
	/// </summary>
	/// <param name="str"></param>
	/// <returns>A string with no formatting characters</returns>
	private static string ClearFormat(this string str) {
		Regex clear = new(@"[^\d]");

		return clear.Replace(str, "");
	}

	/// <summary>
	///     Format Social Security Number as a String
	///     First Remove all characters that are not numbers
	/// </summary>
	/// <param name="ssn"></param>
	/// <returns>A string with no formatting characters</returns>
	public static string FormatSsn(this string ssn) {
		if (string.IsNullOrEmpty(ssn)) {
			return ssn;
		}

		string s = ClearFormat(ssn);

		return !s.All(char.IsDigit)
			? string.Empty
			: s.Insert(5, "-").Insert(3, "-");
	}

	/// <summary>
	///     Format telephone string
	/// </summary>
	/// <param name="str"></param>
	/// <returns>A Telephone number formatted string</returns>
	public static string FormatPhone(this string str) {
		if (string.IsNullOrEmpty(str)) {
			return str;
		}

		// make sure string is free of format characters
		string s = str.ClearFormat();

		return s.Length != 10
			? s
			: $"({s[..3]}) {s.Substring(3, 3)}-{s[6..]}";
	}

	/// <summary>
	///     Format zip code string
	/// </summary>
	/// <param name="str"></param>
	/// <returns>A Zip Code formatted string</returns>
	public static string FormatZip(this string str) {
		if (string.IsNullOrEmpty(str)) {
			return str;
		}

		string s = str.ClearFormat(); // make sure string is free of format characters

		return string.IsNullOrEmpty(s.Trim()) || !s.All(char.IsDigit)
			? string.Empty
			: s.Length switch { 5 => s, 9 => $"{s[..5]}-{s[5..]}", _ => s };
	}

	/// <summary>
	///     Reformat String to title case
	/// </summary>
	/// <param name="str"></param>
	/// <returns>Title case formatted string</returns>
	public static string TitleCase(this string str) {
		if (string.IsNullOrEmpty(str)) {
			return string.Empty;
		}

		string tc = Regex.Replace(str, @"\s+", " ");
		string[] words = tc.Trim().Split(' ');

		string retStr = words.Aggregate("", (current, t) =>
			current + (TestArticle(t)
				? t + " "
				: char.ToUpper(t[0], CultureInfo.CurrentCulture) + t[1..].ToLower(CultureInfo.CurrentCulture) + " "));

		return retStr.Trim();
	}

	private static bool TestArticle(string str) {
		string[] conj = {
			"a", "an", "the", "and", "but", "or", "by", "nor", "yet", "so",
			"about", "above", "across", "after", "against", "along", "among", "around", "at", "before",
			"behind", "between", "beyond", "but", "by", "concerning", "despite", "down", "during",
			"except", "following", "for", "from", "in", "including", "into", "like", "near", "of",
			"off", "on", "out", "over", "plus", "since", "through", "throughout", "to", "towards",
			"under", "until", "up", "upon", "with", "within", "without"
		};

		return conj.Any(s => s == str);
	}
}
