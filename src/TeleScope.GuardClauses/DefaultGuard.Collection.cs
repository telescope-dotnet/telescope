using System;
using System.Collections.Generic;
using System.Linq;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="ICollectionGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{

		public override IEnumerable<T> All<T>(IEnumerable<T> input, Func<T, bool> predicate, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			_ = IsNotEmpty(input, paramName, message);

			if (!input.All(predicate))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The collection `{paramName}` does not satisfy the condition for all elements.";
				throw new ArgumentException(name, msg);
			}
			return input;
		}

		public override IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			if (!input.Any())
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The collection `{paramName}` contains no elements.";
				throw new ArgumentException(name, msg);
			}
			return input;
		}

		public override IEnumerable<T> IsFilled<T>(IEnumerable<T> input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			int i = 0;
			foreach (T item in input)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The collection `{name}` has an empty item at index {i}.";

				if (item is null)
				{
					throw new ArgumentNullException(name, msg);
				}
				else if (item is string)
				{
					_ = IsNotNullOrEmpty(item as string, name, msg);
				}
				i++;
			}
			return input;
		}	

		public override IEnumerable<T> Contains<T>(IEnumerable<T> input, T item, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (!input.Contains(item))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The collection `{name}` should have contained the element `{item}`.";
				throw new ArgumentException(name, msg);
			}
			return input;
		}

		public override IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (input.Contains(item))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The collection `{name}` must not contain the element `{item}`.";
				throw new ArgumentException(name, msg);
			}
			return input;
		}
	}
}
