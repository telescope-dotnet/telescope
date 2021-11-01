using System;
using System.Collections.Generic;

namespace TeleScope.GuardClauses.Abstractions
{
	public interface ICollectionGuardable
	{
		IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, string paramName = null, string message = null);

		IEnumerable<T> IsFilled<T>(IEnumerable<T> input, string paramName = null, string message = null);

		IEnumerable<T> Contains<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);
		
		IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, string paramName = null, string message = null);

		IEnumerable<T> All<T>(IEnumerable<T> input, Func<T, bool> predicate, string paramName = null, string message = null);
	}
}
