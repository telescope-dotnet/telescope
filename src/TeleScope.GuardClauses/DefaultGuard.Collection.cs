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
				Message = message ?? $"The collection `{paramName}` does not satisfy the condition for all elements.";
				throw new ArgumentException(Name, Message);
			}

			return input;
		}

		public override IEnumerable<T> IsNotEmpty<T>(IEnumerable<T> input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			if (!input.Any())
			{
				Message = message ?? $"The collection `{paramName}` contains no elements.";
				throw new ArgumentException(Name, Message);
			}
			return input;
		}

		public override IEnumerable<T> IsFilled<T>(IEnumerable<T> input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			int i = 0;
			foreach (T item in input)
			{
				Message = message ?? $"The collection `{paramName}` has an empty item at index {i}.";

				if (item is null)
				{
					throw new ArgumentNullException(Name, Message);
				}
				else if (item is string)
				{
					_ = IsNotNullOrEmpty(item as string, Name, Message);
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
				Message = message ?? $"The collection `{paramName}` should have contained the element `{item}`.";
				throw new ArgumentException(Name, Message);
			}

			return input;
		}

		public override IEnumerable<T> ContainsNot<T>(IEnumerable<T> input, T item, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			if (input.Contains(item))
			{
				Message = message ?? $"The collection `{paramName}` must not contain the element `{item}`.";
				throw new ArgumentException(Name, Message);
			}

			return input;
		}
	}
}
