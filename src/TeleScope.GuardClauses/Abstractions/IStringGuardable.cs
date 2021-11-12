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
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsNotNullOrEmpty(string input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is Null or a whitespace. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null);
	}
}
