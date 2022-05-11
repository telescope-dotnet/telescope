using System;
using System.Runtime.CompilerServices;

namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This interface provides methods for defensive or basic guard clauses.
	/// The implementation of these methods should fail, if they approve the statement of the method name.
	/// </summary>
	public interface IDefensiveGuardable
	{
		/// <summary>
		/// The implementation shall check the input against null and fail, if that is the case. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T Null<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		T Null<T>(T input, Func<Exception> callException);

		/// <summary>
		/// The implementation shall check the boolean input against `False` and fail, if that is the case. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		bool False(bool input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check the boolean input against `True` and fail, if that is the case. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		bool True(bool input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check the input against an unequal comparator
		/// and fail, if both instances are unequal. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T Unequality<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check the input against an equal comparator
		/// and fail, if both instances are equal. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T Equality<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null);
	}
}
