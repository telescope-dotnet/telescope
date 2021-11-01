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

		public abstract bool False(bool input, string paramName = null, string message = null);

		public abstract T Null<T>(T input, string paramName = null, string message = null);

		public abstract bool True(bool input, string paramName = null, string message = null);
		
		public abstract T Equality<T>(T input, T comparator, string paramName = null, string message = null);

		public abstract T Unequality<T>(T input, T comparator, string paramName = null, string message = null);

		// -- numeric clauses

		public abstract T IsExact<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;

		public abstract T IsNot<T>(T input, string paramName = null, string message = null, params T[] comparators) where T : IComparable;

		public abstract T IsLarger<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;

		public abstract T IsLargerThanZero<T>(T input, string paramName = null, string message = null) where T : IComparable;

		public abstract T IsNotZero<T>(T input, string paramName = null, string message = null) where T : IComparable;

		public abstract T IsSmaller<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;

		// -- string clauses

		public abstract string IsNotNullOrEmpty(string input, string paramName = null, string message = null);

		public abstract string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null);

		// -- collection clauses

		public abstract IEnumerable<T> All<T>(IEnumerable<T> input, Func<T, bool> predicate, string paramName = null, string message = null);

		public abstract IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, string paramName = null, string message = null);

		public abstract IEnumerable<T> IsFilled<T>(IEnumerable<T> input, string paramName = null, string message = null);

		public abstract IEnumerable<T> Contains<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);

		public abstract IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);
	}
}
