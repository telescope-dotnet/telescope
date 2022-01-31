using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This interface provides methods for <see cref="IEnumerable{T}"/> related guard clauses.
	/// The implementation of these methods should succeed, if they approve the statement of the method name.
	/// </summary>
	public interface ICollectionGuardable
	{
		/// <summary>
		/// The implementation shall check, if all elements of the input enumeration meet a predicate function. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="predicate">The test function, every element must meet.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		IEnumerable<T> All<T>(IEnumerable<T> input, Func<T, bool> predicate, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input enumeration is not empty. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if all elements of the input enumeration are not null. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		IEnumerable<T> IsFilled<T>(IEnumerable<T> input, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input enumeration contains a specific element. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="item">The test item, used for compare opertations.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		IEnumerable<T> Contains<T>(IEnumerable<T> input, T item, [CallerArgumentExpression("input")] string expression = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input enumeration doesn't contain a specific element. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="item">The test item, used for compare opertations.</param>
		/// <param name="expression">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, [CallerArgumentExpression("input")] string expression = null, string message = null);
	}
}
