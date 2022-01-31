using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="ICollectionGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{

		public override IEnumerable<T> All<T>(
			IEnumerable<T> input, 
			Func<T, bool> predicate, 
			[CallerArgumentExpression("input")] string expression = null, 
			string message = null)
		{
			_ = Null(input, expression, message);
			_ = IsNotEmpty(input, expression, message);
			if (input.All(predicate))
			{
				return input;
			}
			throw new ArgumentException(message ?? $"The collection does not satisfy the condition for all elements.", expression);
		}

		public override IEnumerable<T> IsNotEmpty<T>(
			IEnumerable<T> input, 
			[CallerArgumentExpression("input")] string expression = null, 
			string message = null)
		{
			_ = Null(input, expression, message);
			if (input.Any())
			{
				return input;
			}
			throw new ArgumentException(message ?? $"The collection contains no elements.", expression);
		}

		public override IEnumerable<T> IsFilled<T>(
			IEnumerable<T> input,
			[CallerArgumentExpression("input")] string expression = null,
			string message = null)
		{
			_ = Null(input, expression, message);

			int i = 0;
			foreach (T item in input)
			{
				var msg = message ?? $"The collection has an empty item at index {i}.";

				if (item is null)
				{
					throw new ArgumentNullException(expression, msg);
				}
				else if (item is string)
				{
					_ = IsNotNullOrEmpty(item as string, expression, msg);
				}
				i++;
			}
			return input;
		}	

		public override IEnumerable<T> Contains<T>(
			IEnumerable<T> input,
			T item,
			[CallerArgumentExpression("input")] string expression = null,
			string message = null)
		{
			_ = Null(input, expression, message);
			if (input.Contains(item))
			{
				return input;
			}
			throw new ArgumentException(message ?? $"The collection should contain the element {item}.", expression);
		}

		public override IEnumerable<T> ContainsNot<T>(
			IEnumerable<T> input, 
			T item, 
			[CallerArgumentExpression("input")] string expression = null, 
			string message = null)
		{
			_ = Null(input, expression, message);
			if (input.Contains(item))
			{
				throw new ArgumentException(message ?? $"The collection must not contain the element {item}.", expression);
			}
			return input;
		}
	}
}
