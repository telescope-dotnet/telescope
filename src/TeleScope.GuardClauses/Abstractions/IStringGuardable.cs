using System;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using TeleScope.GuardClauses.Enumerations;

namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This interface provides methods for string related guard clauses.
	/// The implementation of these methods should succeed, if they approve the statement of the method name.
	/// </summary>
	public interface IStringGuardable
	{
		/// <summary>
		/// The implementation shall check, if the input string is Null or an empty string. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsNotNullOrEmpty(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is Null or a whitespace. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsNotNullOrWhiteSpace(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is a valid email address. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsMailAddress(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is a valid email address. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data as <see cref="MailAddress"/>, if no exception will be thrown.</returns>
		MailAddress ToMailAddress(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is a valid IP address. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="protocol">The protocol version that needs to match during the guard precedure. 
		/// The default value is <see cref="InternetProtocols.IPv4"/>.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsIpAddress(
			string input,
			InternetProtocols protocol = InternetProtocols.IPv4, 
			[CallerArgumentExpression("input")] string expression = null,
			string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is a valid Uri.
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="kind">The uri kind. The default value is <see cref="UriKind.RelativeOrAbsolute"/>.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsUri(string input, UriKind kind = UriKind.RelativeOrAbsolute, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is a valid Uri.
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsWebUri(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);


		/// <summary>
		/// The implementation shall check, if the input string is a valid Uri and 
		/// shall convert the value into an <see cref="Uri"/> in a successful case. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data as <see cref="Uri"/>, if no exception will be thrown.</returns>
		Uri ToUri(string input, [CallerArgumentExpression("input")] string expression = null, string message = null);
	}
}
