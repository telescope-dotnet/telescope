using System;
using System.Collections.Generic;

namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This pure abstract base class implements all interfaces of guard functions
	/// in order to provide a complete set of implementations for user-defined guard classes.
	/// </summary>
	public abstract class GuardBase : IDefensiveGuardable, INumericGuardable, IStringGuardable, ICollectionGuardable
	{

		// -- standard clauses

		/// <summary>
		/// The implementation shall check the boolean input against `False` and fail, if that is the case. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract bool False(bool input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check the input against null and fail, if that is the case. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T Null<T>(T input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check the boolean input against `True` and fail, if that is the case. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract bool True(bool input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check the input against an equal comparator
		/// and fail, if both instances are equal. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T Equality<T>(T input, T comparator, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check the input against an unequal comparator
		/// and fail, if both instances are unequal. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T Unequality<T>(T input, T comparator, string paramName = null, string message = null);

		// -- numeric clauses


		/// <summary>
		/// The implementation shall check, if the input number has exactly the same value of the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsExact<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number has not the same value of the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <param name="comparators">The potential multiple instance that shall be compared by an implementation of the method.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsNot<T>(T input, string paramName = null, string message = null, params T[] comparators) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is larger than the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsLarger<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;


		/// <summary>
		/// The implementation shall check, if the input number is larger than zero. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsLargerThanZero<T>(T input, string paramName = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is not zero. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsNotZero<T>(T input, string paramName = null, string message = null) where T : IComparable;

		/// <summary>
		/// The implementation shall check, if the input number is smaller than the comparator. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="comparator">The instance that shall be compared by an implementation of the method.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract T IsSmaller<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;

		// -- string clauses

		/// <summary>
		/// The implementation shall check, if the input string is Null or an empty string. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract string IsNotNullOrEmpty(string input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input string is Null or a whitespace. 
		/// </summary>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null);

		// -- collection clauses

		/// <summary>
		/// The implementation shall check, if all elements of the input enumeration meet a predicate function. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="predicate">The test function, every element must meet.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract IEnumerable<T> All<T>(IEnumerable<T> input, Func<T, bool> predicate, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input enumeration is not empty. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check, if all elements of the input enumeration are not null. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract IEnumerable<T> IsFilled<T>(IEnumerable<T> input, string paramName = null, string message = null);


		/// <summary>
		/// The implementation shall check, if the input enumeration contains a specific element. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="item">The test item, used for compare opertations.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract IEnumerable<T> Contains<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);

		/// <summary>
		/// The implementation shall check, if the input enumeration doesn't contain a specific element. 
		/// </summary>
		/// <typeparam name="T">The type param under test.</typeparam>
		/// <param name="input">The instance under test.</param>
		/// <param name="item">The test item, used for compare opertations.</param>
		/// <param name="paramName">The optional parameter name under test.</param>
		/// <param name="message">The optional exception message that wil be used, if the method implementation throws.</param>
		/// <returns>The input data, if no exception will be thrown.</returns>
		public abstract IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);
	}
}
