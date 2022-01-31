using System;
using System.Runtime.CompilerServices;

namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This interface provides methods for numeric related guard clauses.
	/// The implementation of these methods should succeed, if they approve the statement of the method name.
	/// </summary>
	public interface INumericGuardable
	{
		/// <summary>
		/// The implementation shall check, if the input number has exactly the same value of the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsExact<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number has not the same value of the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <param name="comparators">The potential multiple instance that shall be compared by an implementation of the method.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsNot<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null, params T[] comparators) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is not zero. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsNotZero<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is larger than zero. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsLargerThanZero<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is larger than the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsLarger<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is smaller than the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		T IsSmaller<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null) where T : IComparable;
	}
}
